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
            logRichTextBox = new RichTextBox();
            comboBoxDifficulty = new ComboBox();
            textBoxKeyMap = new TextBox();
            label1 = new Label();
            label2 = new Label();
            buttonChangeKeyMap = new Button();
            label3 = new Label();
            nUDSalt = new NumericUpDown();
            buttonChooseDifficulty = new Button();
            nUDPressTime = new NumericUpDown();
            label4 = new Label();
            nUDHoldTime = new NumericUpDown();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)nUDSalt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nUDPressTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nUDHoldTime).BeginInit();
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
            treeViewJsonFiles.Location = new Point(12, 158);
            treeViewJsonFiles.Name = "treeViewJsonFiles";
            treeViewJsonFiles.Size = new Size(363, 280);
            treeViewJsonFiles.TabIndex = 2;
            treeViewJsonFiles.NodeMouseDoubleClick += treeViewJsonFiles_NodeMouseDoubleClick;
            // 
            // logRichTextBox
            // 
            logRichTextBox.Location = new Point(381, 43);
            logRichTextBox.Name = "logRichTextBox";
            logRichTextBox.ReadOnly = true;
            logRichTextBox.Size = new Size(352, 395);
            logRichTextBox.TabIndex = 3;
            logRichTextBox.Text = "";
            logRichTextBox.WordWrap = false;
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
            label3.Location = new Point(12, 104);
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
            nUDSalt.Name = "nUDSalt";
            nUDSalt.Size = new Size(71, 23);
            nUDSalt.TabIndex = 10;
            nUDSalt.TextAlign = HorizontalAlignment.Center;
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
            // nUDPressTime
            // 
            nUDPressTime.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            nUDPressTime.Location = new Point(304, 100);
            nUDPressTime.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nUDPressTime.Name = "nUDPressTime";
            nUDPressTime.Size = new Size(71, 23);
            nUDPressTime.TabIndex = 16;
            nUDPressTime.TextAlign = HorizontalAlignment.Center;
            nUDPressTime.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(166, 104);
            label4.Name = "label4";
            label4.Size = new Size(128, 15);
            label4.TabIndex = 15;
            label4.Text = "Normal Key Press Time";
            // 
            // nUDHoldTime
            // 
            nUDHoldTime.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            nUDHoldTime.Location = new Point(80, 129);
            nUDHoldTime.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nUDHoldTime.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            nUDHoldTime.Name = "nUDHoldTime";
            nUDHoldTime.Size = new Size(71, 23);
            nUDHoldTime.TabIndex = 18;
            nUDHoldTime.TextAlign = HorizontalAlignment.Center;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 133);
            label5.Name = "label5";
            label5.Size = new Size(62, 15);
            label5.TabIndex = 17;
            label5.Text = "Hold Time";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(742, 446);
            Controls.Add(nUDHoldTime);
            Controls.Add(label5);
            Controls.Add(nUDPressTime);
            Controls.Add(label4);
            Controls.Add(buttonChooseDifficulty);
            Controls.Add(buttonChangeKeyMap);
            Controls.Add(nUDSalt);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxKeyMap);
            Controls.Add(comboBoxDifficulty);
            Controls.Add(logRichTextBox);
            Controls.Add(treeViewJsonFiles);
            Controls.Add(buttonChooseFolder);
            Controls.Add(textBoxFolder);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "FNF New Version Bot";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)nUDSalt).EndInit();
            ((System.ComponentModel.ISupportInitialize)nUDPressTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)nUDHoldTime).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxFolder;
        private Button buttonChooseFolder;
        private TreeView treeViewJsonFiles;
        private RichTextBox logRichTextBox;
        private ComboBox comboBoxDifficulty;
        private TextBox textBoxKeyMap;
        private Label label1;
        private Label label2;
        private Button buttonChangeKeyMap;
        private Label label3;
        private NumericUpDown nUDSalt;
        private Button buttonChooseDifficulty;
        private NumericUpDown nUDPressTime;
        private Label label4;
        private NumericUpDown nUDHoldTime;
        private Label label5;
    }
}
