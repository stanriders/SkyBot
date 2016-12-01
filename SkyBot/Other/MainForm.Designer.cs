namespace SkyBot
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.enableSkype = new System.Windows.Forms.CheckBox();
            this.enableVK = new System.Windows.Forms.CheckBox();
            this.enableTelegram = new System.Windows.Forms.CheckBox();
            this.enableTest = new System.Windows.Forms.CheckBox();
            this.vkLogin = new System.Windows.Forms.TextBox();
            this.vkPassword = new System.Windows.Forms.TextBox();
            this.vkAppID = new System.Windows.Forms.TextBox();
            this.vkSave = new System.Windows.Forms.Button();
            this.l1 = new System.Windows.Forms.Label();
            this.l2 = new System.Windows.Forms.Label();
            this.l3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // enableSkype
            // 
            this.enableSkype.AutoSize = true;
            this.enableSkype.Enabled = false;
            this.enableSkype.Location = new System.Drawing.Point(13, 13);
            this.enableSkype.Name = "enableSkype";
            this.enableSkype.Size = new System.Drawing.Size(56, 17);
            this.enableSkype.TabIndex = 0;
            this.enableSkype.Text = "Skype";
            this.enableSkype.UseVisualStyleBackColor = true;
            this.enableSkype.CheckedChanged += new System.EventHandler(this.enableSkype_CheckedChanged);
            // 
            // enableVK
            // 
            this.enableVK.AutoSize = true;
            this.enableVK.Location = new System.Drawing.Point(6, 19);
            this.enableVK.Name = "enableVK";
            this.enableVK.Size = new System.Drawing.Size(59, 17);
            this.enableVK.TabIndex = 1;
            this.enableVK.Text = "Enable";
            this.enableVK.UseVisualStyleBackColor = true;
            this.enableVK.CheckedChanged += new System.EventHandler(this.enableVK_CheckedChanged);
            // 
            // enableTelegram
            // 
            this.enableTelegram.AutoSize = true;
            this.enableTelegram.Enabled = false;
            this.enableTelegram.Location = new System.Drawing.Point(13, 114);
            this.enableTelegram.Name = "enableTelegram";
            this.enableTelegram.Size = new System.Drawing.Size(70, 17);
            this.enableTelegram.TabIndex = 2;
            this.enableTelegram.Text = "Telegram";
            this.enableTelegram.UseVisualStyleBackColor = true;
            // 
            // enableTest
            // 
            this.enableTest.AutoSize = true;
            this.enableTest.Location = new System.Drawing.Point(13, 137);
            this.enableTest.Name = "enableTest";
            this.enableTest.Size = new System.Drawing.Size(43, 17);
            this.enableTest.TabIndex = 3;
            this.enableTest.Text = "test";
            this.enableTest.UseVisualStyleBackColor = true;
            this.enableTest.CheckedChanged += new System.EventHandler(this.enableTest_CheckedChanged);
            // 
            // vkLogin
            // 
            this.vkLogin.Location = new System.Drawing.Point(71, 35);
            this.vkLogin.Name = "vkLogin";
            this.vkLogin.Size = new System.Drawing.Size(85, 20);
            this.vkLogin.TabIndex = 4;
            this.vkLogin.Text = "login";
            // 
            // vkPassword
            // 
            this.vkPassword.Location = new System.Drawing.Point(162, 35);
            this.vkPassword.Name = "vkPassword";
            this.vkPassword.Size = new System.Drawing.Size(79, 20);
            this.vkPassword.TabIndex = 5;
            this.vkPassword.Text = "password";
            this.vkPassword.UseSystemPasswordChar = true;
            // 
            // vkAppID
            // 
            this.vkAppID.Location = new System.Drawing.Point(247, 35);
            this.vkAppID.Name = "vkAppID";
            this.vkAppID.Size = new System.Drawing.Size(56, 20);
            this.vkAppID.TabIndex = 6;
            this.vkAppID.Text = "appID";
            // 
            // vkSave
            // 
            this.vkSave.Location = new System.Drawing.Point(309, 33);
            this.vkSave.Name = "vkSave";
            this.vkSave.Size = new System.Drawing.Size(59, 23);
            this.vkSave.TabIndex = 7;
            this.vkSave.Text = "Save";
            this.vkSave.UseVisualStyleBackColor = true;
            this.vkSave.Click += new System.EventHandler(this.vkSave_Click);
            // 
            // l1
            // 
            this.l1.AutoSize = true;
            this.l1.Location = new System.Drawing.Point(68, 20);
            this.l1.Name = "l1";
            this.l1.Size = new System.Drawing.Size(33, 13);
            this.l1.TabIndex = 8;
            this.l1.Text = "Login";
            // 
            // l2
            // 
            this.l2.AutoSize = true;
            this.l2.Location = new System.Drawing.Point(159, 20);
            this.l2.Name = "l2";
            this.l2.Size = new System.Drawing.Size(53, 13);
            this.l2.TabIndex = 9;
            this.l2.Text = "Password";
            // 
            // l3
            // 
            this.l3.AutoSize = true;
            this.l3.Location = new System.Drawing.Point(244, 20);
            this.l3.Name = "l3";
            this.l3.Size = new System.Drawing.Size(36, 13);
            this.l3.TabIndex = 10;
            this.l3.Text = "appID";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.enableVK);
            this.groupBox1.Controls.Add(this.l3);
            this.groupBox1.Controls.Add(this.vkLogin);
            this.groupBox1.Controls.Add(this.l2);
            this.groupBox1.Controls.Add(this.vkPassword);
            this.groupBox1.Controls.Add(this.l1);
            this.groupBox1.Controls.Add(this.vkAppID);
            this.groupBox1.Controls.Add(this.vkSave);
            this.groupBox1.Location = new System.Drawing.Point(13, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 72);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "VK";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 165);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.enableTest);
            this.Controls.Add(this.enableTelegram);
            this.Controls.Add(this.enableSkype);
            this.Name = "MainForm";
            this.Text = "SkyBot";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox enableSkype;
        private System.Windows.Forms.CheckBox enableVK;
        private System.Windows.Forms.CheckBox enableTelegram;
        private System.Windows.Forms.CheckBox enableTest;
        private System.Windows.Forms.TextBox vkLogin;
        private System.Windows.Forms.TextBox vkPassword;
        private System.Windows.Forms.TextBox vkAppID;
        private System.Windows.Forms.Button vkSave;
        private System.Windows.Forms.Label l1;
        private System.Windows.Forms.Label l2;
        private System.Windows.Forms.Label l3;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

