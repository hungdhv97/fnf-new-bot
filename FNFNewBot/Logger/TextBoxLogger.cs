namespace FNFNewBot.Logger
{
    public class RichTextBoxLogger : ILogger
    {
        private readonly RichTextBox _logRichTextBox;

        public RichTextBoxLogger(RichTextBox logRichTextBox)
        {
            _logRichTextBox = logRichTextBox;
        }

        public void Log(string message, Color? color = null)
        {
            if (_logRichTextBox.InvokeRequired)
            {
                _logRichTextBox.Invoke(new Action<string, Color?>(Log), message, color);
            }
            else
            {
                AppendLogMessage(message, color);
            }
        }

        private void AppendLogMessage(string message, Color? color)
        {
            if (color.HasValue)
            {
                _logRichTextBox.SelectionStart = _logRichTextBox.TextLength;
                _logRichTextBox.SelectionLength = 0;
                _logRichTextBox.SelectionColor = color.Value;
            }

            _logRichTextBox.AppendText(message + Environment.NewLine);
            if (color.HasValue)
            {
                _logRichTextBox.SelectionColor = _logRichTextBox.ForeColor;
            }

            _logRichTextBox.ScrollToCaret();
        }
    }
}
