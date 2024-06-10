namespace FNFNewBot
{
    public partial class ListCheckBoxDialog : Form
    {
        public ListCheckBoxDialog(List<string> items)
        {
            InitializeComponent();
            LoadCheckboxes(items);
        }

        private void LoadCheckboxes(List<string> items)
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

            ClientSize = ClientSize with { Height = yPos + 30 };
            buttonOK.Location = new Point((ClientSize.Width - buttonOK.Width) / 2, yPos);
        }

        public List<string> GetCheckedItems()
        {
            var checkedItems = new List<string>();
            foreach (var control in Controls)
            {
                if (control is CheckBox checkbox && checkbox.Checked)
                {
                    checkedItems.Add(checkbox.Text);
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
