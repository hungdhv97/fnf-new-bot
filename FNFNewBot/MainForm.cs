using FNFNewBot.DTO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace FNFNewBot
{
    public partial class MainForm : Form
    {
        private IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc proc;
        private static List<Note> easyNotes = new List<Note>();

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
        private static LowLevelKeyboardProc _proc = HookCallback;
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

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                if (key == Keys.X)
                {
                    MessageBox.Show("Stop", "Thread Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (key == Keys.Z)
                {
                    MessageBox.Show("Run", "Thread Running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (key == Keys.Enter)
                {
                    ExecuteEasyNotesInParallel();
                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private static void ExecuteEasyNotesInParallel()
        {
            var notesByDirection = easyNotes.GroupBy(note => note.Direction).ToDictionary(g => g.Key, g => g.ToList());

            List<Thread> threads = new List<Thread>();

            foreach (var directionNotes in notesByDirection)
            {
                Thread thread = new Thread(() => ExecuteNotes(directionNotes.Value));
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        private static void ExecuteNotes(List<Note> notes)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var note in notes)
            {
                WaitForNanoseconds(stopwatch, (long)(note.Time * 1000000));
                PressKey(note.Direction, note.Length);
            }

            stopwatch.Stop();
        }

        private static void PressKey(int direction, double? length)
        {
            byte keyCode = direction switch
            {
                0 => (byte)Keys.Left,
                1 => (byte)Keys.Up,
                2 => (byte)Keys.Right,
                3 => (byte)Keys.Down,
                _ => (byte)0
            };

            keybd_event(keyCode, 0, 0, 0);
            if (length.HasValue)
            {
                WaitForNanoseconds(null, (long)(length.Value * 1000000));
            }
            keybd_event(keyCode, 0, 2, 0);
        }

        private static void WaitForNanoseconds(Stopwatch? stopwatch, long nanoseconds)
        {
            long ticksPerNanosecond = Stopwatch.Frequency / 1000000000;
            long targetTicks = nanoseconds * ticksPerNanosecond;

            SpinWait spinWait = new SpinWait();

            if (stopwatch != null)
            {
                while (stopwatch.ElapsedTicks < targetTicks)
                {
                    spinWait.SpinOnce();
                }
            }
            else
            {
                var sw = Stopwatch.StartNew();
                while (sw.ElapsedTicks < targetTicks)
                {
                    spinWait.SpinOnce();
                }
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
                ReadAndDisplayJsonFile(filePath);
            }
        }

        private void ReadAndDisplayJsonFile(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                Song song = JsonConvert.DeserializeObject<Song>(jsonContent);
                easyNotes = song.Notes.Easy;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading JSON file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(hookId);
        }
    }
}
