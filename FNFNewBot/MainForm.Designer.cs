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
            treeViewJsonFile = new TreeView();
            logTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // textBoxFolder
            // 
            textBoxFolder.Location = new Point(12, 12);
            textBoxFolder.Name = "textBoxFolder";
            textBoxFolder.Size = new Size(275, 23);
            textBoxFolder.TabIndex = 0;
            // 
            // buttonChooseFolder
            // 
            buttonChooseFolder.Location = new Point(293, 12);
            buttonChooseFolder.Name = "buttonChooseFolder";
            buttonChooseFolder.Size = new Size(141, 23);
            buttonChooseFolder.TabIndex = 1;
            buttonChooseFolder.Text = "Choose Folder";
            buttonChooseFolder.UseVisualStyleBackColor = true;
            // 
            // treeViewJsonFile
            // 
            treeViewJsonFile.Location = new Point(12, 163);
            treeViewJsonFile.Name = "treeViewJsonFile";
            treeViewJsonFile.Size = new Size(422, 275);
            treeViewJsonFile.TabIndex = 2;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(440, 12);
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(348, 426);
            logTextBox.TabIndex = 3;
            logTextBox.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(logTextBox);
            Controls.Add(treeViewJsonFile);
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
        private TreeView treeViewJsonFile;
        private RichTextBox logTextBox;
    }
}
