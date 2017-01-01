// Skybot 2013-2016

using System;
using System.Windows.Forms;

namespace SkyBot
{
    public static class InformationCollector
    {
        public static MainForm Form;

        public static void Error(object source, string text)
        {
            if (Form.InvokeRequired)
                Form.Invoke( (MethodInvoker) delegate { Form.debugText.Text += "(" + DateTime.Now + ") " + source.GetType().Name + ": " + text + Environment.NewLine; });
            else
                Form.debugText.Text += "(" + DateTime.Now + ") " + source.GetType().Name + ": " + text + Environment.NewLine;
        }

        public static void Info(object source, string text)
        {
            if (Form.InvokeRequired)
                Form.Invoke((MethodInvoker)delegate { Form.debugText.Text += source.GetType().Name + ": " + text + Environment.NewLine; });
            else
                Form.debugText.Text += source.GetType().Name + ": " + text + Environment.NewLine;
        }
    }
}
