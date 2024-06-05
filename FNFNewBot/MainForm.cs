using Newtonsoft.Json;
using System.Xml;

namespace FNFNewBot
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

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
                    TreeNode fileNode = new TreeNode(file.Name) { Tag = file.FullName };
                    aNode.Nodes.Add(fileNode);
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
                var jsonObject = JsonConvert.DeserializeObject(jsonContent);
                MessageBox.Show(JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented), "JSON Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading JSON file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
