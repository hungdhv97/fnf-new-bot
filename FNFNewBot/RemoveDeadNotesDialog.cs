namespace FNFNewBot
{
    public partial class RemoveDeadNotesDialog : Form
    {
        public RemoveDeadNotesDialog(List<int> items)
        {
            InitializeComponent();
            LoadCheckboxes(items);
        }

        private void LoadCheckboxes(List<int> items)
        {
            int yPos = 10;
            int itemHeight = 20;
            foreach (var item in items)
            {
                var checkbox = new CheckBox
                {
                    Text = item.ToString(),
                    Location = new Point((ClientSize.Width - 30) / 2, yPos),
                    AutoSize = true
                };
                Controls.Add(checkbox);
                yPos += itemHeight;
            }

            ClientSize = new Size(ClientSize.Width, yPos + 30);
            buttonOK.Location = new Point((ClientSize.Width - buttonOK.Width) / 2, yPos);
        }

        public List<int> GetCheckedItems()
        {
            var checkedItems = new List<int>();
            foreach (var control in Controls)
            {
                if (control is CheckBox checkbox && checkbox.Checked)
                {
                    if (int.TryParse(checkbox.Text, out int value))
                    {
                        checkedItems.Add(value);
                    }
                }
            }
            return checkedItems;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
