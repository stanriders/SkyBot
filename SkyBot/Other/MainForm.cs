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
            ExceptionCollector.Form = this;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Skybot = new SkyBot( this );

            vkAppID.Text = Config.Read("VK", "appID");
            vkLogin.Text = Config.Read("VK", "login");
            vkPassword.Text = Config.Read("VK", "password");

            tgToken.Text = Config.Read("Telegram", "token");

            discordToken.Text = Config.Read("Discord", "token");

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

        private void enableTelegram_CheckedChanged(object sender, EventArgs e)
        {
            if (enableTelegram.Checked)
                Skybot.EnableAPI(APIList.Telegram);
            else
                Skybot.DisableAPI(APIList.Telegram);
        }

        private void enableDiscord_CheckedChanged(object sender, EventArgs e)
        {
            if (enableDiscord.Checked)
                Skybot.EnableAPI(APIList.Discord);
            else
                Skybot.DisableAPI(APIList.Discord);
        }

        private void vkSave_Click(object sender, EventArgs e)
        {
            Config.Write("VK", "appID", vkAppID.Text);
            Config.Write("VK", "login", vkLogin.Text);
            Config.Write("VK", "password", vkPassword.Text);
        }

        private void tgSave_Click(object sender, EventArgs e)
        {
            Config.Write("Telegram", "token", tgToken.Text);
        }

        private void discordSave_Click(object sender, EventArgs e)
        {
            Config.Write("Discord", "token", discordToken.Text);
        }

        private void moduleList_DoubleClick(object sender, EventArgs e)
        {
            if (moduleList.SelectedItem != null)
            {
                IModule module = Skybot.Modules.Find(m => m.ID.ToString() == moduleList.SelectedItem.ToString());
                CreateConfigForm(module);
            }
        }

        private void CreateConfigForm(IModule module)
        {
            int configurablesCount = module.Configurables.Count;
            if (configurablesCount > 0)
            {
                Form config = new Form()
                {
                    Width = 335,
                    Text = module.ID.ToString() + " Config",
                };
                for (int i = 0; i < configurablesCount; i++)
                {
                    config.Controls.Add(new Label
                    {
                        AutoSize = true,
                        Location = new System.Drawing.Point(10, 10 + i * 40),
                        Text = module.Configurables[i]
                    });
                    config.Controls.Add(new TextBox
                    {
                        Width = 300,
                        Height = 20,
                        Location = new System.Drawing.Point(10, 25 + i * 40),
                        Name = module.Configurables[i],
                        Text = Config.Read(module.ID.ToString(), module.Configurables[i])
                    });
                }
                Button savebutton;
                config.Controls.Add(savebutton = new Button
                {
                    Width = 75,
                    Height = 25,
                    Location = new System.Drawing.Point(10, 10 + configurablesCount * 40),
                    Text = "Save"
                });
                savebutton.Click += delegate (object sender, EventArgs e)
                {
                    foreach (Control t in config.Controls)
                    {
                        if (t is TextBox)
                            Config.Write(module.ID.ToString(), t.Name, t.Text);
                    }
                    config.Close();
                };
                config.Height = 85 + configurablesCount * 40;
                config.Show();
            }
        }
    }
}
