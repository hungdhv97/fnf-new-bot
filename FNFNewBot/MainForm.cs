using FNFNewBot.DTO;
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
        private const long OneBillion = 1_000_000_000;

        private IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc proc;
        private static List<Note> easyNotes = new List<Note>();
        private static List<Note> normalNotes = new List<Note>();
        private static List<Note> hardNotes = new List<Note>();
        private static List<Note> erectNotes = new List<Note>();
        private Stopwatch stopwatch;
        private bool isExecuting = false;
        private bool isClosing = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            proc = HookCallback;
            hookId = SetHook(proc);
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
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                if (key == Keys.Escape)
                {
                    isClosing = true;
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
            if (isExecuting)
            {
                Log($"{DateTime.Now:HH:mm:ss.fff}\tRunning");
                return;
            }

            var selectedDifficulty = comboBoxDifficulty.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDifficulty))
            {
                Log($"{DateTime.Now:HH:mm:ss.fff}\tPlease select a difficulty", Color.Red);
                return;
            }

            var notes = GetNotesForDifficulty(selectedDifficulty);
            if (!notes.Any())
            {
                Log($"{DateTime.Now:HH:mm:ss.fff}\tNo notes available for the selected difficulty", Color.Red);
                return;
            }

            Log($"{DateTime.Now:HH:mm:ss.fff}\tStart");

            isExecuting = true;
            await Task.Run(() => ExecuteNotesInParallel(notes));
            isExecuting = false;
            isClosing = false;

            Log($"{DateTime.Now:HH:mm:ss.fff}\tStop");
        }


        private List<Note> GetNotesForDifficulty(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => easyNotes,
                "Normal" => normalNotes,
                "Hard" => hardNotes,
                "Erect" => erectNotes,
                _ => new List<Note>()
            };
        }


        private void ExecuteNotesInParallel(List<Note> notes)
        {
            var notesByDirection = notes
                .Where(note => note.Direction >= 0 && note.Direction <= 3)
                .GroupBy(note => note.Direction)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<Task> tasks = new List<Task>();

            foreach (var directionNotes in notesByDirection)
            {
                var directionNoteList = directionNotes.Value;
                tasks.Add(Task.Run(() => ExecuteNotes(directionNoteList)));
            }
            stopwatch = Stopwatch.StartNew();
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
            foreach (var note in notes)
            {
                long targetTimeNanoseconds = (long)(note.Time * OneMillion);
                WaitForNanoseconds(stopwatch, targetTimeNanoseconds);
                if (isClosing) break;
                PressKey(note.Direction, note.Length);
            }
        }

        private void PressKey(int direction, double? length)
        {
            byte keyCode = direction switch
            {
                0 => (byte)Keys.Left,
                1 => (byte)Keys.Up,
                2 => (byte)Keys.Right,
                3 => (byte)Keys.Down,
                _ => (byte)0
            };

            string keyName = direction switch
            {
                0 => "←",
                1 => "↑",
                2 => "→",
                3 => "↓",
                _ => string.Empty
            };

            Color keyColor = direction switch
            {
                0 => Color.Brown,
                1 => Color.Green,
                2 => Color.Blue,
                3 => Color.Purple,
                _ => Color.Black
            };

            Log($"{stopwatch.ElapsedMilliseconds}\t{new string('\t', direction)}{keyName}{new string('\t', 4 - direction)}{length}", keyColor);

            keybd_event(keyCode, 0, 0, 0);

            if (length.HasValue && length.Value > 0)
            {
                WaitForNanoseconds(Stopwatch.StartNew(), (long)(length.Value * OneMillion));
            }

            keybd_event(keyCode, 0, 2, 0);
        }

        private void WaitForNanoseconds(Stopwatch stopwatch, long nanoseconds)
        {
            long targetTicks = (long)(nanoseconds * 0.01);
            while (stopwatch.ElapsedTicks < targetTicks)
            {
                if (isClosing) break;
                Thread.SpinWait(1);
            }
        }

        private void buttonChooseFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    textBoxFolder.Text = selectedPath;
                    PopulateTreeView(selectedPath);
                }
            }
        }

        private void PopulateTreeView(string path)
        {
            treeViewJsonFiles.Nodes.Clear();
            DirectoryInfo rootDirectory = new DirectoryInfo(path);
            TreeNode rootNode = new TreeNode(rootDirectory.Name) { Tag = rootDirectory };
            GetDirectories(rootDirectory.GetDirectories(), rootNode);
            treeViewJsonFiles.Nodes.Add(rootNode);
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

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            foreach (DirectoryInfo subDir in subDirs)
            {
                TreeNode aNode = new TreeNode(subDir.Name, 0, 0) { Tag = subDir };
                DirectoryInfo[] subSubDirs = subDir.GetDirectories();
                FileInfo[] files = subDir.GetFiles("*.json");
                foreach (FileInfo file in files)
                {
                    if (!file.Name.Contains("metadata", StringComparison.OrdinalIgnoreCase))
                    {
                        TreeNode fileNode = new TreeNode(file.Name) { Tag = file.FullName };
                        aNode.Nodes.Add(fileNode);
                    }
                }
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
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
                Song song = JsonConvert.DeserializeObject<Song>(jsonContent);
                easyNotes = song.Notes.Easy;
                normalNotes = song.Notes.Normal;
                hardNotes = song.Notes.Hard;
                erectNotes = song.Notes.Erect;
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
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
            UnhookWindowsHookEx(hookId);
        }
    }
}
