namespace FNFNewBot
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBoxFolder = new TextBox();
            buttonChooseFolder = new Button();
            treeViewJsonFiles = new TreeView();
            logTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // textBoxFolder
            // 
            textBoxFolder.Location = new Point(12, 12);
            textBoxFolder.Name = "textBoxFolder";
            textBoxFolder.ReadOnly = true;
            textBoxFolder.Size = new Size(629, 23);
            textBoxFolder.TabIndex = 0;
            // 
            // buttonChooseFolder
            // 
            buttonChooseFolder.Location = new Point(647, 12);
            buttonChooseFolder.Name = "buttonChooseFolder";
            buttonChooseFolder.Size = new Size(141, 23);
            buttonChooseFolder.TabIndex = 1;
            buttonChooseFolder.Text = "Choose Folder";
            buttonChooseFolder.UseVisualStyleBackColor = true;
            buttonChooseFolder.Click += buttonChooseFolder_Click;
            // 
            // treeViewJsonFiles
            // 
            treeViewJsonFiles.Location = new Point(12, 242);
            treeViewJsonFiles.Name = "treeViewJsonFiles";
            treeViewJsonFiles.Size = new Size(422, 196);
            treeViewJsonFiles.TabIndex = 2;
            treeViewJsonFiles.NodeMouseDoubleClick += treeViewJsonFiles_NodeMouseDoubleClick;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(440, 43);
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(348, 395);
            logTextBox.TabIndex = 3;
            logTextBox.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(logTextBox);
            Controls.Add(treeViewJsonFiles);
            Controls.Add(buttonChooseFolder);
            Controls.Add(textBoxFolder);
            Name = "MainForm";
            Text = "FNF New Version Bot";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxFolder;
        private Button buttonChooseFolder;
        private TreeView treeViewJsonFiles;
        private RichTextBox logTextBox;
    }
}
