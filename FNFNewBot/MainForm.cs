using FNFNewBot.Dto;
using FNFNewBot.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FNFNewBot
{
    public partial class MainForm : Form
    {
        private const long OneMillion = 1_000_000;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static List<KeyType> _keyTypes = new();

        private IntPtr _hookId = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;
        private Stopwatch _stopwatch;
        private bool _isExecuting;
        private bool _isClosing;
        private SongInfo? _currentSongInfo;
        private Song1? _currentSong1;
        private Song2? _currentSong2;
        private Song3? _currentSong3;
        private string _selectedDifficulty;
        private RichTextBoxLogger _logger;

        public MainForm()
        {
            InitializeComponent();
            _logger = new RichTextBoxLogger(logRichTextBox);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _proc = HookCallback;
            _hookId = SetHook(_proc);

            SetupAutoComplete();
            ChangeKeyTypes(textBoxKeyMap.Text);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isClosing = true;
            UnhookWindowsHookEx(_hookId);
        }

        private void buttonChooseFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath;
                textBoxFolder.Text = selectedPath;
                PopulateTreeView(selectedPath);
            }
        }

        private void buttonChangeKeyMap_Click(object sender, EventArgs e)
        {
            string input = textBoxKeyMap.Text;
            ChangeKeyTypes(input);
        }

        private void buttonChangeDifficulty_Click(object sender, EventArgs e)
        {
            if (comboBoxDifficulty.SelectedItem == null)
            {
                _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tPlease choose chart");
                return;
            }

            _selectedDifficulty = comboBoxDifficulty.SelectedItem.ToString()!;
            var section = _currentSongInfo!.Sections.FirstOrDefault(s => s.Mode.ToString() == _selectedDifficulty);
            RemoveSpecialNotes(section!.GetSpecialNotes());
            _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tDifficult Mode: {_selectedDifficulty}");
        }

        private void treeViewJsonFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is string filePath && filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                ReadJsonFile(filePath);
            }
        }

        private void SetupAutoComplete()
        {
            textBoxKeyMap.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxKeyMap.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection keyMapSuggestions = new AutoCompleteStringCollection
            {
                "left_down_up_right",
                "a_s_w_d",
                "a_s_d_left_down_right",
                "a_s_d_f_space_left_down_up_right",
                "a_s_d_f_space_h_j_k_l"
            };
            textBoxKeyMap.AutoCompleteCustomSource = keyMapSuggestions;
        }

        private void ChangeKeyTypes(string input)
        {
            try
            {
                string[] keys = input.Split('_');
                _keyTypes.Clear();
                foreach (var key in keys)
                {
                    _keyTypes.Add(KeyType.FromString(key));
                }

                _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tKeyMap: {string.Join(", ", _keyTypes.Select(k => k.Name))}");
                UpdateCurrentSongInfo();
            }
            catch (ArgumentException ex)
            {
                _logger.Log("Error: " + ex.Message, Color.Red);
            }
        }

        private void UpdateCurrentSongInfo()
        {
            if (_currentSongInfo == null) return;

            _currentSongInfo = _currentSongInfo.Version switch
            {
                1 when _currentSong1 != null => SongInfo.From(_currentSongInfo.Name, _currentSong1, _keyTypes),
                2 when _currentSong2 != null => SongInfo.From(_currentSongInfo.Name, _currentSong2, _keyTypes),
                3 when _currentSong3 != null => SongInfo.From(_currentSongInfo.Name, _currentSong3, _keyTypes),
                _ => _currentSongInfo
            };
        }

        private async void ExecuteNotesInParallelAsync()
        {
            if (_isExecuting)
            {
                _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tRunning");
                return;
            }

            if (_currentSongInfo == null)
            {
                _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tPlease choose song");
                return;
            }

            List<NoteInfo> notes = GetNotesForDifficulty(_selectedDifficulty);

            if (notes.Count == 0)
            {
                _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tNo notes available for the selected difficulty", Color.Red);
                return;
            }

            _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tStart");

            _isExecuting = true;
            await Task.Run(() => ExecuteNotesInParallel(notes));
            _isExecuting = false;
            _isClosing = false;

            _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tStop");
        }

        private List<NoteInfo> GetNotesForDifficulty(string difficulty)
        {
            var section = _currentSongInfo!.Sections.FirstOrDefault(s => s.Mode.ToString() == difficulty);
            return section?.ToStart().Notes ?? new List<NoteInfo>();
        }

        private void ExecuteNotesInParallel(List<NoteInfo> notes)
        {
            var notesByDirection = notes
                .GroupBy(note => note.Direction)
                .ToDictionary(g => g.Key, g => g.ToList());

            _stopwatch = Stopwatch.StartNew();
            List<Task> tasks = notesByDirection
                .Select(directionNotes => Task.Run(() => ExecuteNotes(directionNotes.Value)))
                .ToList();

            try
            {
                Task.WhenAll(tasks).Wait();
            }
            catch (AggregateException ex)
            {
                _logger.Log($"Execution cancelled: {ex.Message}");
            }
        }

        private void ExecuteNotes(List<NoteInfo> notes)
        {
            foreach (NoteInfo note in notes)
            {
                long targetTimeNanoseconds = (long)(note.Time * OneMillion);
                WaitForNanoseconds(_stopwatch, targetTimeNanoseconds);
                if (_isClosing) break;
                PressKey(note);
            }
        }

        private void PressKey(NoteInfo note)
        {
            int direction = note.Direction;
            double? length = note.Length;

            KeyType keyType = note.KeyType;

            uint scanCode = MapVirtualKey(keyType.Code, 0);

            keybd_event(keyType.Code, (byte)scanCode, 0, 0);

            if (length is > 0)
            {
                _logger.Log($"{_stopwatch.ElapsedMilliseconds}\t{new string('\t', direction)}{keyType.Name}{new string('\t', _keyTypes.Count - direction)}{length.Value + (int)nUDHoldTime.Value}", keyType.Color);
                WaitForNanoseconds(Stopwatch.StartNew(), (long)((length.Value + (int)nUDHoldTime.Value) * OneMillion));
            }
            else
            {
                _logger.Log($"{_stopwatch.ElapsedMilliseconds}\t{new string('\t', direction)}{keyType.Name}{new string('\t', _keyTypes.Count - direction)}{nUDPressTime.Value}", keyType.Color);
                WaitForNanoseconds(Stopwatch.StartNew(), (int)nUDPressTime.Value * OneMillion);
            }

            keybd_event(keyType.Code, (byte)scanCode, 2, 0);
        }

        private void WaitForNanoseconds(Stopwatch stopwatch, long nanoseconds)
        {
            long targetTicks = (long)(nanoseconds * 0.01);
            while (stopwatch.ElapsedTicks < targetTicks + (int)nUDSalt.Value * 10_000)
            {
                if (_isClosing) break;
                Thread.SpinWait(1);
            }
        }

        private void PopulateTreeView(string path)
        {
            treeViewJsonFiles.Nodes.Clear();
            DirectoryInfo rootDirectory = new DirectoryInfo(path);
            TreeNode rootNode = new TreeNode(rootDirectory.Name) { Tag = rootDirectory };
            AddDirectoriesToTreeView(rootDirectory.GetDirectories(), rootNode);
            treeViewJsonFiles.Nodes.Add(rootNode);
        }

        private void AddDirectoriesToTreeView(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            foreach (DirectoryInfo subDir in subDirs)
            {
                TreeNode dirNode = new TreeNode(subDir.Name) { Tag = subDir };
                AddFilesToTreeView(subDir.GetFiles("*.json"), dirNode);
                AddDirectoriesToTreeView(subDir.GetDirectories(), dirNode);
                nodeToAddTo.Nodes.Add(dirNode);
            }
        }

        private void AddFilesToTreeView(FileInfo[] files, TreeNode node)
        {
            foreach (FileInfo file in files)
            {
                if (!file.Name.Contains("metadata", StringComparison.OrdinalIgnoreCase))
                {
                    TreeNode fileNode = new TreeNode(file.Name) { Tag = file.FullName };
                    node.Nodes.Add(fileNode);
                }
            }
        }

        private void PopulateDifficultyComboBox(List<NoteSection> noteSections)
        {
            comboBoxDifficulty.Items.Clear();

            foreach (NoteSection section in noteSections.Where(section => section.Notes.Count != 0))
            {
                comboBoxDifficulty.Items.Add(section.Mode.ToString());
            }

            if (comboBoxDifficulty.Items.Count > 0)
            {
                comboBoxDifficulty.SelectedIndex = 0;
                _selectedDifficulty = comboBoxDifficulty.SelectedItem?.ToString()!;
                NoteSection? section = _currentSongInfo!.Sections.FirstOrDefault(s => s.Mode.ToString() == _selectedDifficulty);
                RemoveSpecialNotes(section!.GetSpecialNotes());
                _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tDifficult Mode: {comboBoxDifficulty.SelectedItem}");
            }
        }

        private void ReadJsonFile(string filePath)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string jsonContent = File.ReadAllText(filePath);

                var songHandlers = new Dictionary<int, Func<string, bool>>
                {
                    { 1, content => TryDeserializeSong<Song1>(content, out _currentSong1) },
                    { 2, content => TryDeserializeSong<Song2>(content, out _currentSong2) },
                    { 3, content => TryDeserializeSong<Song3>(content, out _currentSong3) }
                };

                foreach (var handler in songHandlers)
                {
                    if (handler.Value(jsonContent))
                    {
                        _currentSongInfo = handler.Key switch
                        {
                            1 => SongInfo.From(fileName, _currentSong1!, _keyTypes),
                            2 => SongInfo.From(fileName, _currentSong2!, _keyTypes),
                            3 => SongInfo.From(fileName, _currentSong3!, _keyTypes),
                            _ => _currentSongInfo
                        };

                        PopulateDifficultyComboBox(_currentSongInfo!.Sections);
                        _logger.Log($"{DateTime.Now:HH:mm:ss.fff}\tSelected chart v{_currentSongInfo.Version}: {fileName}");
                        return;
                    }
                }

                _logger.Log($"Error reading JSON file: Unknown format", Color.Red);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error reading JSON file: {ex.Message}", Color.Red);
            }
        }

        private bool TryDeserializeSong<T>(string jsonContent, out T? song) where T : class
        {
            song = JsonConvert.DeserializeObject<T>(jsonContent);
            if (song == null) return false;

            var jsonObject = JObject.Parse(jsonContent);
            return typeof(T) == typeof(Song1) && jsonObject.ContainsKey("song") && jsonObject["song"]?["notes"] != null
                || typeof(T) == typeof(Song2) && jsonObject.ContainsKey("notes")
                || typeof(T) == typeof(Song3) && jsonObject.ContainsKey("strumLines");
        }

        private void RemoveSpecialNotes(List<int> specialNotes)
        {
            if (specialNotes.Any())
            {
                using var dialog = new ListCheckBoxDialog(specialNotes);
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                var checkedItems = dialog.GetCheckedItems();
                _logger.Log("Removed special notes: " + string.Join(", ", checkedItems));
            }
        }

        private void AdjustValue(NumericUpDown numericUpDown, bool increase)
        {
            if (increase)
            {
                if (numericUpDown.Value < numericUpDown.Maximum)
                {
                    numericUpDown.Value += numericUpDown.Increment;
                }
            }
            else
            {
                if (numericUpDown.Value > numericUpDown.Minimum)
                {
                    numericUpDown.Value -= numericUpDown.Increment;
                }
            }

            string logMessage = numericUpDown == nUDSalt ? $"Salt: {numericUpDown.Value}" :
                                numericUpDown == nUDPressTime ? $"Press Time: {numericUpDown.Value} ms" :
                                $"Hold Time: {numericUpDown.Value} ms";
            _logger.Log(logMessage);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using Process curProcess = Process.GetCurrentProcess();
            using ProcessModule curModule = curProcess.MainModule!;
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                switch (key)
                {
                    case Keys.Escape:
                        _isClosing = _isExecuting;
                        break;
                    case Keys.D9:
                        ExecuteNotesInParallelAsync();
                        break;
                    case Keys.D1:
                        AdjustValue(nUDSalt, false);
                        break;
                    case Keys.D2:
                        AdjustValue(nUDSalt, true);
                        break;
                    case Keys.D3:
                        AdjustValue(nUDPressTime, false);
                        break;
                    case Keys.D4:
                        AdjustValue(nUDPressTime, true);
                        break;
                    case Keys.D5:
                        AdjustValue(nUDHoldTime, false);
                        break;
                    case Keys.D6:
                        AdjustValue(nUDHoldTime, true);
                        break;
                }
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}
