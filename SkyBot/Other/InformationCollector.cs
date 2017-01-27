// Skybot 2013-2017

using System;
using System.Windows.Forms;

namespace SkyBot
{
    public static class InformationCollector
    {
        public static MainForm Form;

        public static void Error(object source, string text)
        {
            string result = "(" + DateTime.Now + ") " + source.GetType().Name + ": " + text + Environment.NewLine;

            Log(result);
        }

        public static void Info(object source, string text)
        {
            string result = source.GetType().Name + ": " + text + Environment.NewLine;

            Log(result);
        }

        public static void Log(string text)
        {
            if (!Form.IsDisposed)
            {
                if (Form.InvokeRequired)
                    Form.Invoke((MethodInvoker)delegate {
                        Form.debugText.Text += text;
                    });
                else
                    Form.debugText.Text += text;
            }

            Accessories.WriteStringToFile(text, Application.StartupPath + "/log.txt");
        }
    }
}
