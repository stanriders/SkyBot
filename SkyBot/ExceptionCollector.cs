// Skybot 2013-2016

using System.Windows.Forms;

namespace SkyBot
{
    public static class ExceptionCollector
    {
        public static MainForm Form;
        public static void Error(string text)
        {
            if (Form.InvokeRequired)
            {
                Form.Invoke((MethodInvoker)delegate
                {
                    Form.debugText.Text += text + "\n";
                });
            }
            else
                Form.debugText.Text += text + "\n";
        }
    }
}
