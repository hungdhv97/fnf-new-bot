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
            buttonChangeKeyMap = new Button();
            label3 = new Label();
            nUDSalt = new NumericUpDown();
            buttonChooseDifficulty = new Button();
            ((System.ComponentModel.ISupportInitialize)nUDSalt).BeginInit();
            SuspendLayout();
            // 
            // textBoxFolder
            // 
            textBoxFolder.Location = new Point(12, 12);
            textBoxFolder.Name = "textBoxFolder";
            textBoxFolder.ReadOnly = true;
            textBoxFolder.Size = new Size(574, 23);
            textBoxFolder.TabIndex = 0;
            // 
            // buttonChooseFolder
            // 
            buttonChooseFolder.Location = new Point(592, 12);
            buttonChooseFolder.Name = "buttonChooseFolder";
            buttonChooseFolder.Size = new Size(141, 25);
            buttonChooseFolder.TabIndex = 1;
            buttonChooseFolder.Text = "Choose Folder";
            buttonChooseFolder.UseVisualStyleBackColor = true;
            buttonChooseFolder.Click += buttonChooseFolder_Click;
            // 
            // treeViewJsonFiles
            // 
            treeViewJsonFiles.Location = new Point(12, 129);
            treeViewJsonFiles.Name = "treeViewJsonFiles";
            treeViewJsonFiles.Size = new Size(363, 309);
            treeViewJsonFiles.TabIndex = 2;
            treeViewJsonFiles.NodeMouseDoubleClick += treeViewJsonFiles_NodeMouseDoubleClick;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(381, 43);
            logTextBox.Name = "logTextBox";
            logTextBox.ReadOnly = true;
            logTextBox.Size = new Size(352, 395);
            logTextBox.TabIndex = 3;
            logTextBox.Text = "";
            logTextBox.WordWrap = false;
            // 
            // comboBoxDifficulty
            // 
            comboBoxDifficulty.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDifficulty.FormattingEnabled = true;
            comboBoxDifficulty.Location = new Point(73, 43);
            comboBoxDifficulty.Name = "comboBoxDifficulty";
            comboBoxDifficulty.Size = new Size(188, 23);
            comboBoxDifficulty.TabIndex = 4;
            // 
            // textBoxKeyMap
            // 
            textBoxKeyMap.Location = new Point(73, 72);
            textBoxKeyMap.Name = "textBoxKeyMap";
            textBoxKeyMap.Size = new Size(188, 23);
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
            // buttonChangeKeyMap
            // 
            buttonChangeKeyMap.Location = new Point(267, 71);
            buttonChangeKeyMap.Name = "buttonChangeKeyMap";
            buttonChangeKeyMap.Size = new Size(108, 25);
            buttonChangeKeyMap.TabIndex = 13;
            buttonChangeKeyMap.Text = "Change Key Map";
            buttonChangeKeyMap.UseVisualStyleBackColor = true;
            buttonChangeKeyMap.Click += buttonChangeKeyMap_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 102);
            label3.Name = "label3";
            label3.Size = new Size(26, 15);
            label3.TabIndex = 9;
            label3.Text = "Salt";
            // 
            // nUDSalt
            // 
            nUDSalt.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            nUDSalt.Location = new Point(73, 100);
            nUDSalt.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nUDSalt.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            nUDSalt.Name = "nUDSalt";
            nUDSalt.Size = new Size(71, 23);
            nUDSalt.TabIndex = 10;
            nUDSalt.TextAlign = HorizontalAlignment.Center;
            nUDSalt.ValueChanged += nUDSalt_ValueChanged;
            // 
            // buttonChooseDifficulty
            // 
            buttonChooseDifficulty.Location = new Point(267, 40);
            buttonChooseDifficulty.Name = "buttonChooseDifficulty";
            buttonChooseDifficulty.Size = new Size(108, 25);
            buttonChooseDifficulty.TabIndex = 14;
            buttonChooseDifficulty.Text = "Change Difficulty";
            buttonChooseDifficulty.UseVisualStyleBackColor = true;
            buttonChooseDifficulty.Click += buttonChangeDifficulty_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(742, 446);
            Controls.Add(buttonChooseDifficulty);
            Controls.Add(buttonChangeKeyMap);
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
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "FNF New Version Bot";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)nUDSalt).EndInit();
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
        private Button buttonChangeKeyMap;
        private Label label3;
        private NumericUpDown nUDSalt;
        private Button buttonChooseDifficulty;
    }
}
