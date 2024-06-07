using FNFNewBot.Dto;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNFNewBot
{
    public partial class MainForm : Form
    {
        private const long OneMillion = 1_000_000;
        private IntPtr _hookId = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;
        private static List<Note> _easyNotes = new();
        private static List<Note> _normalNotes = new();
        private static List<Note> _hardNotes = new();
        private static List<Note> _erectNotes = new();
        private static List<KeyType> _keyMap = [KeyType.Left, KeyType.Down, KeyType.Up, KeyType.Right];
        private Stopwatch _stopwatch;
        private bool _isExecuting;
        private bool _isClosing;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _proc = HookCallback;
            _hookId = SetHook(_proc);
            SetupAutoComplete();
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod,
            uint dwThreadId);

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
                    _isClosing = true;
                }
                else if (key == Keys.Enter)
                {
                    ExecuteNotesInParallelAsync();
                }
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private async void ExecuteNotesInParallelAsync()
        {
            if (_isExecuting)
            {
                Log($"{DateTime.Now:HH:mm:ss.fff}\tRunning");
                return;
            }

            string selectedDifficulty = comboBoxDifficulty.SelectedItem != null
                ? comboBoxDifficulty.SelectedItem.ToString()!
                : "Easy";

            List<Note> notes = GetNotesForDifficulty(selectedDifficulty);
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

        private static List<Note> GetNotesForDifficulty(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => _easyNotes,
                "Normal" => _normalNotes,
                "Hard" => _hardNotes,
                "Erect" => _erectNotes,
                _ => new List<Note>()
            };
        }

        private void ExecuteNotesInParallel(List<Note> notes)
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

        private void ExecuteNotes(List<Note> notes)
        {
            foreach (Note note in notes)
            {
                long targetTimeNanoseconds = (long)(note.Time * OneMillion);
                WaitForNanoseconds(_stopwatch, targetTimeNanoseconds);
                if (_isClosing) break;
                PressKey(note);
            }
        }

        private void PressKey(Note note)
        {
            int direction = note.Direction;
            double? length = note.Length;

            KeyType keyType = _keyMap[note.Direction];

            Log(
                $"{_stopwatch.ElapsedMilliseconds}\t{new string('\t', direction)}{keyType.Name}{new string('\t', 4 - direction)}{length}",
                keyType.Color);

            keybd_event(keyType.Code, 0, 0, 0);

            if (length is > 0)
            {
                WaitForNanoseconds(Stopwatch.StartNew(), (long)(length.Value * OneMillion));
            }

            keybd_event(keyType.Code, 0, 2, 0);
        }

        private void WaitForNanoseconds(Stopwatch stopwatch, long nanoseconds)
        {
            long targetTicks = (long)(nanoseconds * 0.01);
            while (stopwatch.ElapsedTicks < targetTicks)
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
            }
        }

        private void treeViewJsonFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is string filePath && filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
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
                _easyNotes = song.Notes.Easy;
                _normalNotes = song.Notes.Normal;
                _hardNotes = song.Notes.Hard;
                _erectNotes = song.Notes.Erect;
                PopulateDifficultyComboBox(song.Notes);
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
            AutoCompleteStringCollection keyMapSuggestions = ["left_down_up_right", "a_s_d_f"];
            textBoxKeyMap.AutoCompleteCustomSource = keyMapSuggestions;
        }
        private void buttonChangeKeyMap_Click(object sender, EventArgs e)
        {
            try
            {
                string input = textBoxKeyMap.Text;
                string[] keys = input.Split('_');
                _keyMap.Clear();
                foreach (var key in keys)
                {
                    _keyMap.Add(KeyType.FromString(key));
                }
                Log("KeyMap: " + string.Join(", ", _keyMap.Select(k => k.Name)));
            }
            catch (ArgumentException ex)
            {
                Log("Error: " + ex.Message);
            }
        }
    }
}