﻿using FNFNewBot.Dto;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FNFNewBot
{
    public partial class MainForm : Form
    {
        private const long OneMillion = 1_000_000;
        private IntPtr _hookId = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;
        private static List<KeyType> _keyTypes = new();
        private Stopwatch _stopwatch;
        private bool _isExecuting;
        private bool _isClosing;
        private static int _salt;
        private SongInfo _currentSongInfo;
        private Song _currentSong;
        private string _selectedDifficulty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _proc = HookCallback;
            _hookId = SetHook(_proc);

            _salt = (int)nUDSalt.Value;
            SetupAutoComplete();
            ChangeKeyTypes(textBoxKeyMap.Text);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

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

                if (key == Keys.Escape)
                {
                    _isClosing = true && _isExecuting;
                }
                else if (key == Keys.X)
                {
                    ExecuteNotesInParallelAsync();
                }
                else if (key == Keys.Oemplus)
                {
                    IncrementSalt();
                }
                else if (key == Keys.OemMinus)
                {
                    DecrementSalt();
                }
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private void IncrementSalt()
        {
            if (nUDSalt.Value < nUDSalt.Maximum) nUDSalt.Value += (int)nUDSalt.Increment;
        }

        private void DecrementSalt()
        {
            if (nUDSalt.Value > nUDSalt.Minimum) nUDSalt.Value -= (int)nUDSalt.Increment;
        }

        private async void ExecuteNotesInParallelAsync()
        {
            if (_isExecuting)
            {
                Log($"{DateTime.Now:HH:mm:ss.fff}\tRunning");
                return;
            }

            var notes = GetNotesForDifficulty(_selectedDifficulty);

            if (!notes.Any())
            {
                Log($"{DateTime.Now:HH:mm:ss.fff}\tNo notes available for the selected difficulty", Color.Red);
                return;
            }

            Log($"{DateTime.Now:HH:mm:ss.fff}\tStart");

            _isExecuting = true;
            await Task.Run(() => ExecuteNotesInParallel(notes));
            _isExecuting = false;
            _isClosing = false;

            Log($"{DateTime.Now:HH:mm:ss.fff}\tStop");
        }

        private List<NoteInfo> GetNotesForDifficulty(string difficulty)
        {
            var section = _currentSongInfo.Sections.FirstOrDefault(s => s.Mode.ToString() == difficulty);
            return section?.Notes ?? new List<NoteInfo>();
        }

        private void ExecuteNotesInParallel(List<NoteInfo> notes)
        {
            var notesByDirection = notes
                .Where(note => note.Direction is >= 0 and <= 3)
                .GroupBy(note => note.Direction)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<Task> tasks = notesByDirection
                .Select(directionNotes => Task.Run(() => ExecuteNotes(directionNotes.Value)))
                .ToList();

            _stopwatch = Stopwatch.StartNew();
            try
            {
                Task.WhenAll(tasks).Wait();
            }
            catch (AggregateException ex)
            {
                Log($"Execution cancelled: {ex.Message}");
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

            Log(
                $"{_stopwatch.ElapsedMilliseconds}\t{new string('\t', direction)}{keyType.Name}{new string('\t', 4 - direction)}{length}",
                keyType.Color);

            keybd_event(keyType.Code, 0, 0, 0);

            if (length is > 0)
            {
                WaitForNanoseconds(Stopwatch.StartNew(), (long)(length.Value * OneMillion));
            }
            else
            {
                WaitForNanoseconds(Stopwatch.StartNew(), 10 * OneMillion);
            }

            keybd_event(keyType.Code, 0, 2, 0);
        }

        private void WaitForNanoseconds(Stopwatch stopwatch, long nanoseconds)
        {
            long targetTicks = (long)(nanoseconds * 0.01);
            while (stopwatch.ElapsedTicks < targetTicks + _salt * 10_000)
            {
                if (_isClosing) break;
                Thread.SpinWait(1);
            }
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

        private void PopulateDifficultyComboBox(Difficulty notes)
        {
            comboBoxDifficulty.Items.Clear();
            if (notes.Easy.Any()) comboBoxDifficulty.Items.Add("Easy");
            if (notes.Normal.Any()) comboBoxDifficulty.Items.Add("Normal");
            if (notes.Hard.Any()) comboBoxDifficulty.Items.Add("Hard");
            if (notes.Erect.Any()) comboBoxDifficulty.Items.Add("Erect");
            if (comboBoxDifficulty.Items.Count > 0)
            {
                comboBoxDifficulty.SelectedIndex = 0;
                Log($"{DateTime.Now:HH:mm:ss.fff}\tDifficult Mode: {comboBoxDifficulty.SelectedItem?.ToString()}");
            }
        }

        private void treeViewJsonFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is string filePath && filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                Log($"{DateTime.Now:HH:mm:ss.fff}\tSelected chart: {fileName}");

                ReadJsonFile(filePath);
            }
        }

        private void ReadJsonFile(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                Song? song = JsonConvert.DeserializeObject<Song>(jsonContent);
                if (song == null) return;
                _currentSong = song;
                _currentSongInfo = SongInfo.From(1, Path.GetFileNameWithoutExtension(filePath), _currentSong, _keyTypes);
                PopulateDifficultyComboBox(_currentSong.Notes);
            }
            catch (Exception ex)
            {
                Log($"Error reading JSON file: {ex.Message}", Color.Red);
            }
        }

        private void Log(string message, Color? color = null)
        {
            if (logTextBox.InvokeRequired)
            {
                logTextBox.Invoke(new Action<string, Color?>(Log), message, color);
            }
            else
            {
                AppendLogMessage(message, color);
            }
        }

        private void AppendLogMessage(string message, Color? color)
        {
            if (color.HasValue)
            {
                logTextBox.SelectionStart = logTextBox.TextLength;
                logTextBox.SelectionLength = 0;
                logTextBox.SelectionColor = color.Value;
            }

            logTextBox.AppendText(message + Environment.NewLine);
            if (color.HasValue)
            {
                logTextBox.SelectionColor = logTextBox.ForeColor;
            }

            logTextBox.ScrollToCaret();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isClosing = true;
            UnhookWindowsHookEx(_hookId);
        }

        private void SetupAutoComplete()
        {
            textBoxKeyMap.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxKeyMap.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection keyMapSuggestions = new AutoCompleteStringCollection
            {
                "left_down_up_right",
                "a_s_d_f",
                "a_s_d_f_space_left_down_up_right"
            };
            textBoxKeyMap.AutoCompleteCustomSource = keyMapSuggestions;
        }

        private void buttonChangeKeyMap_Click(object sender, EventArgs e)
        {
            string input = textBoxKeyMap.Text;
            ChangeKeyTypes(input);
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
                Log($"{DateTime.Now:HH:mm:ss.fff}\tKeyMap: {(string.Join(", ", _keyTypes.Select(k => k.Name)))}");
                UpdateCurrentSongInfo();
            }
            catch (ArgumentException ex)
            {
                Log("Error: " + ex.Message);
            }
        }

        private void UpdateCurrentSongInfo()
        {
            if (_currentSong != null && _currentSongInfo != null)
            {
                _currentSongInfo = SongInfo.From(_currentSongInfo.Version, _currentSongInfo.Name, _currentSong, _keyTypes);
            }
        }

        private void nUDSalt_ValueChanged(object sender, EventArgs e)
        {
            _salt = (int)nUDSalt.Value;
            Log($"Salt: {_salt} ms");
        }

        private void buttonChangeDifficulty_Click(object sender, EventArgs e)
        {
            if (comboBoxDifficulty.SelectedItem == null)
            {
                Log($"{DateTime.Now:HH:mm:ss.fff}\tPlease choose chart");
                return;
            }

            _selectedDifficulty = comboBoxDifficulty.SelectedItem.ToString()!;
            Log($"{DateTime.Now:HH:mm:ss.fff}\tDifficult Mode: {_selectedDifficulty}");
        }
    }
}
