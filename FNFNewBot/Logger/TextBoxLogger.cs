namespace FNFNewBot.Logger;

public class TextBoxLogger : ILogger
{
    private readonly TextBox _logTextBox;

    public TextBoxLogger(TextBox logTextBox)
    {
        _logTextBox = logTextBox;
    }

    public void Log(string message, Color? color = null)
    {
        if (_logTextBox.InvokeRequired)
        {
            _logTextBox.Invoke(new Action<string, Color?>(Log), message, color);
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
            _logTextBox.SelectionStart = _logTextBox.TextLength;
            _logTextBox.SelectionLength = 0;
            _logTextBox.SelectionColor = color.Value;
        }

        _logTextBox.AppendText(message + Environment.NewLine);
        if (color.HasValue)
        {
            _logTextBox.SelectionColor = _logTextBox.ForeColor;
        }

        _logTextBox.ScrollToCaret();
    }
}
