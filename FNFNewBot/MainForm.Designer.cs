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
            comboBoxDifficulty = new ComboBox();
            textBoxKeyMap = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            nUDSalt = new NumericUpDown();
            nUDWaiting = new NumericUpDown();
            label4 = new Label();
            buttonChangeKeyMap = new Button();
            ((System.ComponentModel.ISupportInitialize)nUDSalt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nUDWaiting).BeginInit();
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
            treeViewJsonFiles.Location = new Point(12, 131);
            treeViewJsonFiles.Name = "treeViewJsonFiles";
            treeViewJsonFiles.Size = new Size(406, 307);
            treeViewJsonFiles.TabIndex = 2;
            treeViewJsonFiles.NodeMouseDoubleClick += treeViewJsonFiles_NodeMouseDoubleClick;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(424, 43);
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(364, 395);
            logTextBox.TabIndex = 3;
            logTextBox.Text = "";
            // 
            // comboBoxDifficulty
            // 
            comboBoxDifficulty.FormattingEnabled = true;
            comboBoxDifficulty.Location = new Point(96, 43);
            comboBoxDifficulty.Name = "comboBoxDifficulty";
            comboBoxDifficulty.Size = new Size(322, 23);
            comboBoxDifficulty.TabIndex = 4;
            // 
            // textBoxKeyMap
            // 
            textBoxKeyMap.Location = new Point(96, 72);
            textBoxKeyMap.Name = "textBoxKeyMap";
            textBoxKeyMap.Size = new Size(208, 23);
            textBoxKeyMap.TabIndex = 5;
            textBoxKeyMap.Text = "left_down_up_right";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 46);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 6;
            label1.Text = "Difficulty";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 75);
            label2.Name = "label2";
            label2.Size = new Size(53, 15);
            label2.TabIndex = 7;
            label2.Text = "Key map";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 104);
            label3.Name = "label3";
            label3.Size = new Size(26, 15);
            label3.TabIndex = 9;
            label3.Text = "Salt";
            // 
            // nUDSalt
            // 
            nUDSalt.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            nUDSalt.Location = new Point(96, 102);
            nUDSalt.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nUDSalt.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            nUDSalt.Name = "nUDSalt";
            nUDSalt.Size = new Size(115, 23);
            nUDSalt.TabIndex = 10;
            nUDSalt.TextAlign = HorizontalAlignment.Center;
            nUDSalt.ValueChanged += nUDSalt_ValueChanged;
            // 
            // nUDWaiting
            // 
            nUDWaiting.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            nUDWaiting.Location = new Point(303, 102);
            nUDWaiting.Maximum = new decimal(new int[] { 20000, 0, 0, 0 });
            nUDWaiting.Name = "nUDWaiting";
            nUDWaiting.Size = new Size(115, 23);
            nUDWaiting.TabIndex = 12;
            nUDWaiting.TextAlign = HorizontalAlignment.Center;
            nUDWaiting.Value = new decimal(new int[] { 4500, 0, 0, 0 });
            nUDWaiting.ValueChanged += nUDWaiting_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(231, 104);
            label4.Name = "label4";
            label4.Size = new Size(48, 15);
            label4.TabIndex = 11;
            label4.Text = "Waiting";
            // 
            // buttonChangeKeyMap
            // 
            buttonChangeKeyMap.Location = new Point(310, 71);
            buttonChangeKeyMap.Name = "buttonChangeKeyMap";
            buttonChangeKeyMap.Size = new Size(108, 23);
            buttonChangeKeyMap.TabIndex = 13;
            buttonChangeKeyMap.Text = "Change Key Map";
            buttonChangeKeyMap.UseVisualStyleBackColor = true;
            buttonChangeKeyMap.Click += buttonChangeKeyMap_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonChangeKeyMap);
            Controls.Add(nUDWaiting);
            Controls.Add(label4);
            Controls.Add(nUDSalt);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxKeyMap);
            Controls.Add(comboBoxDifficulty);
            Controls.Add(logTextBox);
            Controls.Add(treeViewJsonFiles);
            Controls.Add(buttonChooseFolder);
            Controls.Add(textBoxFolder);
            Name = "MainForm";
            Text = "FNF New Version Bot";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)nUDSalt).EndInit();
            ((System.ComponentModel.ISupportInitialize)nUDWaiting).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxFolder;
        private Button buttonChooseFolder;
        private TreeView treeViewJsonFiles;
        private RichTextBox logTextBox;
        private ComboBox comboBoxDifficulty;
        private TextBox textBoxKeyMap;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown nUDSalt;
        private NumericUpDown nUDWaiting;
        private Label label4;
        private Button buttonChangeKeyMap;
    }
}
