// Skybot 2013-2016

using System;
using System.Windows.Forms;

namespace SkyBot
{
    public partial class MainForm : Form
    {
        SkyBot Skybot;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Skybot = new SkyBot( this );

            vkAppID.Text = Config.Instance.Read("VK", "appID");
            vkLogin.Text = Config.Instance.Read("VK", "login");
            vkPassword.Text = Config.Instance.Read("VK", "password");

            tgToken.Text = Config.Instance.Read("Telegram", "token");

            foreach (IModule module in Skybot.Modules)
                moduleList.Items.Add(module.ID);
        }

        private void enableSkype_CheckedChanged(object sender, EventArgs e)
        {
            if (enableSkype.Checked)
                Skybot.EnableAPI(APIList.Skype);
            else
                Skybot.DisableAPI(APIList.Skype);
        }

        private void enableTest_CheckedChanged(object sender, EventArgs e)
        {
            if (enableTest.Checked)
                Skybot.EnableAPI(APIList.Test);
            else
                Skybot.DisableAPI(APIList.Test);
        }

        private void enableVK_CheckedChanged(object sender, EventArgs e)
        {
            if (enableVK.Checked)
                Skybot.EnableAPI(APIList.VK);
            else
                Skybot.DisableAPI(APIList.VK);
        }

        private void vkSave_Click(object sender, EventArgs e)
        {
            Config.Instance.Write("VK", "appID", vkAppID.Text);
            Config.Instance.Write("VK", "login", vkLogin.Text);
            Config.Instance.Write("VK", "password", vkPassword.Text);
        }

        private void enableTelegram_CheckedChanged(object sender, EventArgs e)
        {
            if (enableTelegram.Checked)
                Skybot.EnableAPI(APIList.Telegram);
            else
                Skybot.DisableAPI(APIList.Telegram);
        }

        private void tgSave_Click(object sender, EventArgs e)
        {
            Config.Instance.Write("Telegram", "token", tgToken.Text);
        }
    }
}
