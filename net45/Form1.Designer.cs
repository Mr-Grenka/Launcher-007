namespace Contra
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.RadioEN = new System.Windows.Forms.RadioButton();
            this.RadioRU = new System.Windows.Forms.RadioButton();
            this.RadioENQuotes = new System.Windows.Forms.RadioButton();
            this.MNew = new System.Windows.Forms.RadioButton();
            this.MStandard = new System.Windows.Forms.RadioButton();
            this.DefaultPics = new System.Windows.Forms.RadioButton();
            this.GoofyPics = new System.Windows.Forms.RadioButton();
            this.voicespanel = new System.Windows.Forms.Panel();
            this.RadioRUQuotes = new System.Windows.Forms.RadioButton();
            this.languagepanel = new System.Windows.Forms.Panel();
            this.musicpanel = new System.Windows.Forms.Panel();
            this.portraitspanel = new System.Windows.Forms.Panel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.buttonDiscord009 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.buttonLaunch = new System.Windows.Forms.Button();
            this.RadioFlag_GB = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RadioFlag_RU = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.PatchDLProgressBar = new System.Windows.Forms.ProgressBar();
            this.PatchDLPanel = new System.Windows.Forms.Panel();
            this.ModDLCurrentFileLabel = new System.Windows.Forms.Label();
            this.DLPercentLabel = new System.Windows.Forms.Label();
            this.ModDLFileSizeLabel = new System.Windows.Forms.Label();
            this.CancelModDLBtn = new System.Windows.Forms.Button();
            this.ModDLLabel = new System.Windows.Forms.Label();
            this.buttonDiscordClassic = new System.Windows.Forms.Button();
            this.moreOptions = new System.Windows.Forms.Button();
            this.MOTD = new Contra.Marquee();
            this.voicespanel.SuspendLayout();
            this.languagepanel.SuspendLayout();
            this.musicpanel.SuspendLayout();
            this.portraitspanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PatchDLPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // RadioEN
            // 
            resources.ApplyResources(this.RadioEN, "RadioEN");
            this.RadioEN.Checked = true;
            this.RadioEN.ForeColor = System.Drawing.Color.White;
            this.RadioEN.Name = "RadioEN";
            this.RadioEN.TabStop = true;
            this.RadioEN.UseVisualStyleBackColor = true;
            // 
            // RadioRU
            // 
            resources.ApplyResources(this.RadioRU, "RadioRU");
            this.RadioRU.ForeColor = System.Drawing.Color.White;
            this.RadioRU.Name = "RadioRU";
            this.RadioRU.UseVisualStyleBackColor = true;
            // 
            // RadioENQuotes
            // 
            resources.ApplyResources(this.RadioENQuotes, "RadioENQuotes");
            this.RadioENQuotes.Checked = true;
            this.RadioENQuotes.ForeColor = System.Drawing.Color.White;
            this.RadioENQuotes.Name = "RadioENQuotes";
            this.RadioENQuotes.TabStop = true;
            this.RadioENQuotes.UseVisualStyleBackColor = true;
            // 
            // MNew
            // 
            resources.ApplyResources(this.MNew, "MNew");
            this.MNew.Checked = true;
            this.MNew.ForeColor = System.Drawing.Color.White;
            this.MNew.Name = "MNew";
            this.MNew.TabStop = true;
            this.MNew.UseVisualStyleBackColor = true;
            this.MNew.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged_1);
            // 
            // MStandard
            // 
            resources.ApplyResources(this.MStandard, "MStandard");
            this.MStandard.ForeColor = System.Drawing.Color.White;
            this.MStandard.Name = "MStandard";
            this.MStandard.UseVisualStyleBackColor = true;
            this.MStandard.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged_1);
            // 
            // DefaultPics
            // 
            resources.ApplyResources(this.DefaultPics, "DefaultPics");
            this.DefaultPics.Checked = true;
            this.DefaultPics.ForeColor = System.Drawing.Color.White;
            this.DefaultPics.Name = "DefaultPics";
            this.DefaultPics.TabStop = true;
            this.DefaultPics.UseVisualStyleBackColor = true;
            // 
            // GoofyPics
            // 
            resources.ApplyResources(this.GoofyPics, "GoofyPics");
            this.GoofyPics.ForeColor = System.Drawing.Color.White;
            this.GoofyPics.Name = "GoofyPics";
            this.GoofyPics.UseVisualStyleBackColor = true;
            this.GoofyPics.CheckedChanged += new System.EventHandler(this.GoofyPics_CheckedChanged);
            // 
            // voicespanel
            // 
            this.voicespanel.BackColor = System.Drawing.Color.Transparent;
            this.voicespanel.Controls.Add(this.RadioRUQuotes);
            this.voicespanel.Controls.Add(this.RadioENQuotes);
            resources.ApplyResources(this.voicespanel, "voicespanel");
            this.voicespanel.Name = "voicespanel";
            this.voicespanel.Paint += new System.Windows.Forms.PaintEventHandler(this.voicespanel_Paint);
            // 
            // RadioRUQuotes
            // 
            resources.ApplyResources(this.RadioRUQuotes, "RadioRUQuotes");
            this.RadioRUQuotes.Cursor = System.Windows.Forms.Cursors.Default;
            this.RadioRUQuotes.ForeColor = System.Drawing.Color.White;
            this.RadioRUQuotes.Name = "RadioRUQuotes";
            this.RadioRUQuotes.UseVisualStyleBackColor = true;
            // 
            // languagepanel
            // 
            this.languagepanel.BackColor = System.Drawing.Color.Transparent;
            this.languagepanel.Controls.Add(this.RadioRU);
            this.languagepanel.Controls.Add(this.RadioEN);
            this.languagepanel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.languagepanel, "languagepanel");
            this.languagepanel.Name = "languagepanel";
            // 
            // musicpanel
            // 
            this.musicpanel.BackColor = System.Drawing.Color.Transparent;
            this.musicpanel.Controls.Add(this.MStandard);
            this.musicpanel.Controls.Add(this.MNew);
            this.musicpanel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.musicpanel, "musicpanel");
            this.musicpanel.Name = "musicpanel";
            // 
            // portraitspanel
            // 
            this.portraitspanel.BackColor = System.Drawing.Color.Transparent;
            this.portraitspanel.Controls.Add(this.GoofyPics);
            this.portraitspanel.Controls.Add(this.DefaultPics);
            this.portraitspanel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.portraitspanel, "portraitspanel");
            this.portraitspanel.Name = "portraitspanel";
            // 
            // versionLabel
            // 
            this.versionLabel.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.ForeColor = System.Drawing.Color.White;
            this.versionLabel.Name = "versionLabel";
            // 
            // buttonDiscord009
            // 
            this.buttonDiscord009.BackColor = System.Drawing.Color.Transparent;
            this.buttonDiscord009.BackgroundImage = global::Contra.Properties.Resources.discord_009_red;
            this.buttonDiscord009.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonDiscord009.FlatAppearance.BorderSize = 0;
            this.buttonDiscord009.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonDiscord009.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.buttonDiscord009, "buttonDiscord009");
            this.buttonDiscord009.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonDiscord009.Name = "buttonDiscord009";
            this.buttonDiscord009.UseVisualStyleBackColor = false;
            this.buttonDiscord009.Click += new System.EventHandler(this.buttonChat_Click);
            this.buttonDiscord009.MouseEnter += new System.EventHandler(this.buttonChat_MouseEnter);
            this.buttonDiscord009.MouseLeave += new System.EventHandler(this.buttonChat_MouseLeave);
            // 
            // button18
            // 
            this.button18.BackColor = System.Drawing.Color.Transparent;
            this.button18.BackgroundImage = global::Contra.Properties.Resources.exit_red;
            resources.ApplyResources(this.button18, "button18");
            this.button18.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button18.FlatAppearance.BorderSize = 0;
            this.button18.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button18.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button18.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button18.Name = "button18";
            this.button18.UseVisualStyleBackColor = false;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            this.button18.MouseEnter += new System.EventHandler(this.button18_MouseEnter);
            this.button18.MouseLeave += new System.EventHandler(this.button18_MouseLeave);
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.Color.Transparent;
            this.button17.BackgroundImage = global::Contra.Properties.Resources.min_red;
            resources.ApplyResources(this.button17, "button17");
            this.button17.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button17.FlatAppearance.BorderSize = 0;
            this.button17.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button17.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button17.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button17.Name = "button17";
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            this.button17.MouseEnter += new System.EventHandler(this.button17_MouseEnter);
            this.button17.MouseLeave += new System.EventHandler(this.button17_MouseLeave);
            // 
            // buttonLaunch
            // 
            this.buttonLaunch.BackColor = System.Drawing.Color.Transparent;
            this.buttonLaunch.BackgroundImage = global::Contra.Properties.Resources.play_button_red;
            this.buttonLaunch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonLaunch.FlatAppearance.BorderSize = 0;
            this.buttonLaunch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonLaunch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.buttonLaunch, "buttonLaunch");
            this.buttonLaunch.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonLaunch.Name = "buttonLaunch";
            this.buttonLaunch.UseVisualStyleBackColor = false;
            this.buttonLaunch.Click += new System.EventHandler(this.button1_Click);
            this.buttonLaunch.MouseEnter += new System.EventHandler(this.button1_MouseEnter);
            this.buttonLaunch.MouseLeave += new System.EventHandler(this.button1_MouseLeave);
            // 
            // RadioFlag_GB
            // 
            resources.ApplyResources(this.RadioFlag_GB, "RadioFlag_GB");
            this.RadioFlag_GB.BackColor = System.Drawing.Color.Transparent;
            this.RadioFlag_GB.BackgroundImage = global::Contra.Properties.Resources.flag_gb;
            this.RadioFlag_GB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RadioFlag_GB.FlatAppearance.BorderSize = 0;
            this.RadioFlag_GB.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.RadioFlag_GB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RadioFlag_GB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RadioFlag_GB.Name = "RadioFlag_GB";
            this.RadioFlag_GB.TabStop = true;
            this.RadioFlag_GB.UseVisualStyleBackColor = false;
            this.RadioFlag_GB.CheckedChanged += new System.EventHandler(this.RadioFlag_GB_CheckedChanged);
            this.RadioFlag_GB.MouseEnter += new System.EventHandler(this.RadioFlag_GB_MouseEnter);
            this.RadioFlag_GB.MouseLeave += new System.EventHandler(this.RadioFlag_GB_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.RadioFlag_RU);
            this.panel1.Controls.Add(this.RadioFlag_GB);
            this.panel1.Name = "panel1";
            // 
            // RadioFlag_RU
            // 
            resources.ApplyResources(this.RadioFlag_RU, "RadioFlag_RU");
            this.RadioFlag_RU.BackColor = System.Drawing.Color.Transparent;
            this.RadioFlag_RU.BackgroundImage = global::Contra.Properties.Resources.flag_ru;
            this.RadioFlag_RU.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RadioFlag_RU.FlatAppearance.BorderSize = 0;
            this.RadioFlag_RU.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.RadioFlag_RU.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RadioFlag_RU.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RadioFlag_RU.Name = "RadioFlag_RU";
            this.RadioFlag_RU.TabStop = true;
            this.RadioFlag_RU.UseVisualStyleBackColor = false;
            this.RadioFlag_RU.CheckedChanged += new System.EventHandler(this.RadioFlag_RU_CheckedChanged);
            this.RadioFlag_RU.MouseEnter += new System.EventHandler(this.RadioFlag_RU_MouseEnter);
            this.RadioFlag_RU.MouseLeave += new System.EventHandler(this.RadioFlag_RU_MouseLeave);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // PatchDLProgressBar
            // 
            resources.ApplyResources(this.PatchDLProgressBar, "PatchDLProgressBar");
            this.PatchDLProgressBar.Name = "PatchDLProgressBar";
            // 
            // PatchDLPanel
            // 
            this.PatchDLPanel.BackColor = System.Drawing.Color.White;
            this.PatchDLPanel.BackgroundImage = global::Contra.Properties.Resources.vpnbg;
            this.PatchDLPanel.Controls.Add(this.ModDLCurrentFileLabel);
            this.PatchDLPanel.Controls.Add(this.DLPercentLabel);
            this.PatchDLPanel.Controls.Add(this.ModDLFileSizeLabel);
            this.PatchDLPanel.Controls.Add(this.CancelModDLBtn);
            this.PatchDLPanel.Controls.Add(this.ModDLLabel);
            this.PatchDLPanel.Controls.Add(this.PatchDLProgressBar);
            resources.ApplyResources(this.PatchDLPanel, "PatchDLPanel");
            this.PatchDLPanel.Name = "PatchDLPanel";
            // 
            // ModDLCurrentFileLabel
            // 
            resources.ApplyResources(this.ModDLCurrentFileLabel, "ModDLCurrentFileLabel");
            this.ModDLCurrentFileLabel.BackColor = System.Drawing.Color.Transparent;
            this.ModDLCurrentFileLabel.ForeColor = System.Drawing.Color.White;
            this.ModDLCurrentFileLabel.Name = "ModDLCurrentFileLabel";
            // 
            // DLPercentLabel
            // 
            resources.ApplyResources(this.DLPercentLabel, "DLPercentLabel");
            this.DLPercentLabel.BackColor = System.Drawing.Color.Transparent;
            this.DLPercentLabel.ForeColor = System.Drawing.Color.White;
            this.DLPercentLabel.Name = "DLPercentLabel";
            // 
            // ModDLFileSizeLabel
            // 
            resources.ApplyResources(this.ModDLFileSizeLabel, "ModDLFileSizeLabel");
            this.ModDLFileSizeLabel.BackColor = System.Drawing.Color.Transparent;
            this.ModDLFileSizeLabel.ForeColor = System.Drawing.Color.White;
            this.ModDLFileSizeLabel.Name = "ModDLFileSizeLabel";
            // 
            // CancelModDLBtn
            // 
            this.CancelModDLBtn.BackColor = System.Drawing.Color.Transparent;
            this.CancelModDLBtn.BackgroundImage = global::Contra.Properties.Resources.btnOk1;
            this.CancelModDLBtn.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.CancelModDLBtn, "CancelModDLBtn");
            this.CancelModDLBtn.ForeColor = System.Drawing.Color.White;
            this.CancelModDLBtn.Name = "CancelModDLBtn";
            this.CancelModDLBtn.UseVisualStyleBackColor = false;
            this.CancelModDLBtn.Click += new System.EventHandler(this.CancelModDLBtn_Click);
            // 
            // ModDLLabel
            // 
            resources.ApplyResources(this.ModDLLabel, "ModDLLabel");
            this.ModDLLabel.BackColor = System.Drawing.Color.Transparent;
            this.ModDLLabel.ForeColor = System.Drawing.Color.White;
            this.ModDLLabel.Name = "ModDLLabel";
            // 
            // buttonDiscordClassic
            // 
            this.buttonDiscordClassic.BackColor = System.Drawing.Color.Transparent;
            this.buttonDiscordClassic.BackgroundImage = global::Contra.Properties.Resources.discord_classic_red;
            this.buttonDiscordClassic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonDiscordClassic.FlatAppearance.BorderSize = 0;
            this.buttonDiscordClassic.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonDiscordClassic.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.buttonDiscordClassic, "buttonDiscordClassic");
            this.buttonDiscordClassic.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonDiscordClassic.Name = "buttonDiscordClassic";
            this.buttonDiscordClassic.UseVisualStyleBackColor = false;
            this.buttonDiscordClassic.Click += new System.EventHandler(this.buttonDiscordClassic_Click);
            this.buttonDiscordClassic.MouseEnter += new System.EventHandler(this.buttonDiscordClassic_MouseEnter);
            this.buttonDiscordClassic.MouseLeave += new System.EventHandler(this.buttonDiscordClassic_MouseLeave);
            // 
            // moreOptions
            // 
            this.moreOptions.BackColor = System.Drawing.Color.Transparent;
            this.moreOptions.BackgroundImage = global::Contra.Properties.Resources.settings_button_red;
            this.moreOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.moreOptions.FlatAppearance.BorderSize = 0;
            this.moreOptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.moreOptions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.moreOptions, "moreOptions");
            this.moreOptions.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.moreOptions.Name = "moreOptions";
            this.moreOptions.UseVisualStyleBackColor = false;
            this.moreOptions.Click += new System.EventHandler(this.buttonSettings_Click);
            this.moreOptions.MouseEnter += new System.EventHandler(this.moreOptions_MouseEnter);
            this.moreOptions.MouseLeave += new System.EventHandler(this.moreOptions_MouseLeave);
            // 
            // MOTD
            // 
            this.MOTD.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.MOTD, "MOTD");
            this.MOTD.ForeColor = System.Drawing.Color.White;
            this.MOTD.Name = "MOTD";
            this.MOTD.Speed = 1;
            this.MOTD.yOffset = 0;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::Contra.Properties.Resources.background;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.PatchDLPanel);
            this.Controls.Add(this.moreOptions);
            this.Controls.Add(this.buttonDiscordClassic);
            this.Controls.Add(this.MOTD);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.voicespanel);
            this.Controls.Add(this.languagepanel);
            this.Controls.Add(this.portraitspanel);
            this.Controls.Add(this.musicpanel);
            this.Controls.Add(this.buttonDiscord009);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.buttonLaunch);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.voicespanel.ResumeLayout(false);
            this.voicespanel.PerformLayout();
            this.languagepanel.ResumeLayout(false);
            this.languagepanel.PerformLayout();
            this.musicpanel.ResumeLayout(false);
            this.musicpanel.PerformLayout();
            this.portraitspanel.ResumeLayout(false);
            this.portraitspanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.PatchDLPanel.ResumeLayout(false);
            this.PatchDLPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLaunch;
        private System.Windows.Forms.RadioButton RadioEN;
        private System.Windows.Forms.RadioButton RadioRU;
        private System.Windows.Forms.RadioButton RadioENQuotes;
        private System.Windows.Forms.RadioButton MStandard;
        private System.Windows.Forms.RadioButton MNew;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button buttonDiscord009;
        private System.Windows.Forms.RadioButton DefaultPics;
        private System.Windows.Forms.RadioButton GoofyPics;
        private System.Windows.Forms.Panel voicespanel;
        private System.Windows.Forms.Panel languagepanel;
        private System.Windows.Forms.Panel musicpanel;
        private System.Windows.Forms.Panel portraitspanel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.RadioButton RadioFlag_GB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton RadioFlag_RU;
        private System.Windows.Forms.ToolTip toolTip1;
        private Marquee MOTD;
        private System.Windows.Forms.ProgressBar PatchDLProgressBar;
        private System.Windows.Forms.Panel PatchDLPanel;
        private System.Windows.Forms.Label ModDLLabel;
        private System.Windows.Forms.Button CancelModDLBtn;
        private System.Windows.Forms.Label ModDLFileSizeLabel;
        private System.Windows.Forms.Label DLPercentLabel;
        private System.Windows.Forms.Label ModDLCurrentFileLabel;
        private System.Windows.Forms.RadioButton RadioRUQuotes;
        private System.Windows.Forms.Button buttonDiscordClassic;
        private System.Windows.Forms.Button moreOptions;
    }
}

