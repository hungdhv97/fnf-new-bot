namespace FNFNewBot
{
    partial class ListCheckBoxDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonOK = new Button();
            SuspendLayout();
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(101, 78);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 0;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // RemoveDeadNotesDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(279, 113);
            Controls.Add(buttonOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "ListCheckBoxDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "RemoveDeadNotes";
            ResumeLayout(false);
        }

        #endregion

        private Button buttonOK;
    }
}