using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Linq;

namespace Contra
{
    public partial class moreOptionsForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(
      string deviceName, int modeNum, ref DEVMODE devMode);
        const int ENUM_CURRENT_SETTINGS = -1;

        const int ENUM_REGISTRY_SETTINGS = -2;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        public moreOptionsForm()
        {
            InitializeComponent();
            LangFilterCheckBox.TabStop = false;
            button17.TabStop = false;
            button18.TabStop = false;
            comboBox1.TabStop = false;
            resOkButton.TabStop = false;

            DEVMODE vDevMode = new DEVMODE();
            int i = 0;
            var dataSource = new List<string>();
            while (EnumDisplaySettings(null, i, ref vDevMode))
            {
                dataSource.Add(vDevMode.dmPelsWidth.ToString() + "x" + vDevMode.dmPelsHeight.ToString());
                i++;
            }
            var noDupes = dataSource.Distinct().ToList();

            this.comboBox1.DataSource = noDupes;

            if (Globals.GB_Checked == true)
            {
                toolTip3.SetToolTip(Shadows3DCheckBox, "Toggle showing 3D shadows in game.\nTurn off for improved performance."); ;
                toolTip3.SetToolTip(Shadows2DCheckBox, "Toggle showing 2D shadows in game.\nTurn off for improved performance.");
                toolTip3.SetToolTip(CloudShadowsCheckBox, "Toggle showing cloud shadows on terrain.\nTurn off for improved performance.");
                toolTip3.SetToolTip(ExtraGroundLightingCheckBox, "Toggle showing detailed lighting on terrain.\nTurn off for improved performance.");
                toolTip3.SetToolTip(SmoothWaterBordersCheckBox, "Toggle smoothing of water borders.\nTurn off for improved performance.");
                toolTip3.SetToolTip(BehindBuildingsCheckBox, "Toggle showing units behind buildings.\nTurn off for improved performance.");
                toolTip3.SetToolTip(ShowPropsCheckBox, "Toggle displaying game props.\nTurn off for improved performance.");
                toolTip3.SetToolTip(ExtraAnimationsCheckBox, "Toggle showing optional animations like tree sway.\nTurn off for improved performance.");
                toolTip3.SetToolTip(DisableDynamicLODCheckBox, "Disable automatic detail adjustment.\nTurn off for improved performance.");
                toolTip3.SetToolTip(HeatEffectsCheckBox, "Toggle showing heat distortion effects.\nTurn this off if your screen randomly turns black while playing.");
                toolTip3.SetToolTip(LangFilterCheckBox, "Disabling the language filter will show bad words written by players in chat.");
                toolTip3.SetToolTip(camHeightLabel, "The camera height setting changes the default and maximum player view distance in-game.\nThe higher this value is, the further away the view will be.");
                toolTip3.SetToolTip(WinCheckBox, "Starts Contra in a window instead of full screen.");
                toolTip3.SetToolTip(QSCheckBox, "Disables intro and shellmap (game starts up faster).");
            }
            else if (Globals.RU_Checked == true)
            {
                toolTip3.SetToolTip(Shadows3DCheckBox, "Переключить отображение трехмерных теней в игре.\nВыключите для повышения производительности.");
                toolTip3.SetToolTip(Shadows2DCheckBox, "Переключить отображение двумерных теней в игре.\nВыключите для повышения производительности.");
                toolTip3.SetToolTip(CloudShadowsCheckBox, "Переключить отображение тени облаков на местности.\nВыключите для повышения производительности.");
                toolTip3.SetToolTip(ExtraGroundLightingCheckBox, "Переключить детализированного освещения земли.\nВыключите для повышения производительности.");
                toolTip3.SetToolTip(SmoothWaterBordersCheckBox, "Переключить сглаживание границ воды.\nВыключите для повышения производительности.");
                toolTip3.SetToolTip(BehindBuildingsCheckBox, "Переключить отображение единицы за зданиями.\nВыключите для повышения производительности.");
                toolTip3.SetToolTip(ShowPropsCheckBox, "Переключить отображение маленькие объекты.\nВыключите для повышения производительности.");
                toolTip3.SetToolTip(ExtraAnimationsCheckBox, "Переключить отображение дополнительных анимаций, таких как раскачивание деревьев.\nВыключите для повышения производительности.");
                toolTip3.SetToolTip(DisableDynamicLODCheckBox, "Переключить автоматическую регулировку деталей.\nВыключите для повышения производительности.");
                toolTip3.SetToolTip(HeatEffectsCheckBox, "Переключите отображение тепловых эффектов.\nВыключите, если ваш экран случайно становится черным во время игры.");
                toolTip3.SetToolTip(LangFilterCheckBox, "Отключение языкового фильтра покажет плохие слова, написанные игроками в чате.");
                toolTip3.SetToolTip(camHeightLabel, "Настройка высоты камеры изменяет стандартное и максимальное расстояние поле зрения игрока.\nЧем выше это значение, тем дальше будет поле зрения.");
                toolTip3.SetToolTip(WinCheckBox, "Запуск Contra в режиме окна вместо полноэкранного.");
                toolTip3.SetToolTip(QSCheckBox, "Отключает интро и шелмапу (игра запускается быстрее).");

                labelResolution.Text = "Разрешение экрана:";
                Shadows3DCheckBox.Text = "3D Тени";
                Shadows2DCheckBox.Text = "2D Тени";
                CloudShadowsCheckBox.Text = "Тени облаков";
                ExtraGroundLightingCheckBox.Text = "Дополнит. освещение земли";
                SmoothWaterBordersCheckBox.Text = "Ровные края воды";
                BehindBuildingsCheckBox.Text = "Единицы за зданиями";
                ShowPropsCheckBox.Text = "Показывать маленькие объекты";
                ExtraAnimationsCheckBox.Text = "Дополнительные анимации";
                DisableDynamicLODCheckBox.Text = "Откл. Динам. Уровень Детализации";
                HeatEffectsCheckBox.Text = "Тепловые эффекты";
                LangFilterCheckBox.Text = "Языковый фильтр";
                camHeightLabel.Text = "Высота камеры: ?";
                WinCheckBox.Text = "Режим окна";
                QSCheckBox.Text = "Быстр. старт";
            }

            //// Make CTR Options.ini active
            //try
            //{
            //    if (File.Exists(UserDataLeafName() + "Options_CTR.ini"))
            //    {
            //        File.SetAttributes(UserDataLeafName() + "Options.ini", FileAttributes.Normal);
            //        File.SetAttributes(UserDataLeafName() + "Options_CTR.ini", FileAttributes.Normal);
            //        File.SetAttributes(UserDataLeafName() + "Options_ZH.ini", FileAttributes.Normal);
            //        File.Copy(UserDataLeafName() + "Options.ini", UserDataLeafName() + "Options_ZH.ini", true);
            //        File.Copy(UserDataLeafName() + "Options_CTR.ini", UserDataLeafName() + "Options.ini", true);
            //    }
            //    else if (File.Exists(myDocPath + "Options_CTR.ini"))
            //    {
            //        File.SetAttributes(myDocPath + "Options.ini", FileAttributes.Normal);
            //        File.SetAttributes(myDocPath + "Options_CTR.ini", FileAttributes.Normal);
            //        File.SetAttributes(myDocPath + "Options_ZH.ini", FileAttributes.Normal);
            //        File.Copy(myDocPath + "Options.ini", myDocPath + "Options_ZH.ini", true);
            //        File.Copy(myDocPath + "Options_CTR.ini", myDocPath + "Options.ini", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            // Read from Options.ini and check/uncheck settings depending on values there
            if (Directory.Exists(UserDataLeafName()))
            {
                string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                {
                    if (text.Contains("StaticGameLOD = Low") || text.Contains("StaticGameLOD = Medium") || text.Contains("StaticGameLOD = High"))
                    {
                        File.WriteAllText(UserDataLeafName() + "Options.ini", Regex.Replace(File.ReadAllText(UserDataLeafName() + "Options.ini"), "\r?\nStaticGameLOD = .*", "\r\nStaticGameLOD = Custom" + "\r"));
                    }

                    if (text.Contains("UseShadowVolumes = No") || text.Contains("UseShadowVolumes = no"))
                    {
                        Shadows3DCheckBox.Checked = false;
                    }
                    else if (text.Contains("UseShadowVolumes = Yes") || text.Contains("UseShadowVolumes = yes"))
                    {
                        Shadows3DCheckBox.Checked = true;
                    }

                    if (text.Contains("UseShadowDecals = No") || text.Contains("UseShadowDecals = no"))
                    {
                        Shadows2DCheckBox.Checked = false;
                    }
                    else if (text.Contains("UseShadowDecals = Yes") || text.Contains("UseShadowDecals = yes"))
                    {
                        Shadows2DCheckBox.Checked = true;
                    }

                    if (text.Contains("UseCloudMap = No") || text.Contains("UseCloudMap = no"))
                    {
                        CloudShadowsCheckBox.Checked = false;
                    }
                    else if (text.Contains("UseCloudMap = Yes") || text.Contains("UseCloudMap = yes"))
                    {
                        CloudShadowsCheckBox.Checked = true;
                    }

                    if (text.Contains("UseLightMap = No") || text.Contains("UseLightMap = no"))
                    {
                        ExtraGroundLightingCheckBox.Checked = false;
                    }
                    else if (text.Contains("UseLightMap = Yes") || text.Contains("UseLightMap = yes"))
                    {
                        ExtraGroundLightingCheckBox.Checked = true;
                    }

                    if (text.Contains("ShowSoftWaterEdge = No") || text.Contains("ShowSoftWaterEdge = no"))
                    {
                        SmoothWaterBordersCheckBox.Checked = false;
                    }
                    else if (text.Contains("ShowSoftWaterEdge = Yes") || text.Contains("ShowSoftWaterEdge = yes"))
                    {
                        SmoothWaterBordersCheckBox.Checked = true;
                    }

                    if (text.Contains("BuildingOcclusion = No") || text.Contains("BuildingOcclusion = no"))
                    {
                        BehindBuildingsCheckBox.Checked = false;
                    }
                    else if (text.Contains("BuildingOcclusion = Yes") || text.Contains("BuildingOcclusion = yes"))
                    {
                        BehindBuildingsCheckBox.Checked = true;
                    }

                    if (text.Contains("ShowTrees = No") || text.Contains("ShowTrees = no"))
                    {
                        ShowPropsCheckBox.Checked = false;
                    }
                    else if (text.Contains("ShowTrees = Yes") || text.Contains("ShowTrees = yes"))
                    {
                        ShowPropsCheckBox.Checked = true;
                    }

                    if (text.Contains("ExtraAnimations = No") || text.Contains("ExtraAnimations = no"))
                    {
                        ExtraAnimationsCheckBox.Checked = false;
                    }
                    else if (text.Contains("ExtraAnimations = Yes") || text.Contains("ExtraAnimations = yes"))
                    {
                        ExtraAnimationsCheckBox.Checked = true;
                    }

                    if (text.Contains("DynamicLOD = No") || text.Contains("DynamicLOD = no"))
                    {
                        DisableDynamicLODCheckBox.Checked = true;
                    }
                    else if (text.Contains("DynamicLOD = Yes") || text.Contains("DynamicLOD = yes"))
                    {
                        DisableDynamicLODCheckBox.Checked = false;
                    }

                    if (text.Contains("HeatEffects = No") || text.Contains("HeatEffects = no"))
                    {
                        HeatEffectsCheckBox.Checked = false;
                    }
                    else if (text.Contains("HeatEffects = Yes") || text.Contains("HeatEffects = yes"))
                    {
                        HeatEffectsCheckBox.Checked = true;
                    }
                }
            }
            else if (Directory.Exists(myDocPath))
            {
                string text = File.ReadAllText(myDocPath + "Options.ini");
                {
                    if (text.Contains("StaticGameLOD = Low") || text.Contains("StaticGameLOD = Medium") || text.Contains("StaticGameLOD = High"))
                    {
                        File.WriteAllText(myDocPath + "Options.ini", Regex.Replace(File.ReadAllText(myDocPath + "Options.ini"), "\r?\nStaticGameLOD = .*", "\r\nStaticGameLOD = Custom" + "\r"));
                    }

                    if (text.Contains("UseShadowVolumes = No") || text.Contains("UseShadowVolumes = no"))
                    {
                        Shadows3DCheckBox.Checked = false;
                    }
                    else if (text.Contains("UseShadowVolumes = Yes") || text.Contains("UseShadowVolumes = yes"))
                    {
                        Shadows3DCheckBox.Checked = true;
                    }

                    if (text.Contains("UseShadowDecals = No") || text.Contains("UseShadowDecals = no"))
                    {
                        Shadows2DCheckBox.Checked = false;
                    }
                    else if (text.Contains("UseShadowDecals = Yes") || text.Contains("UseShadowDecals = yes"))
                    {
                        Shadows2DCheckBox.Checked = true;
                    }

                    if (text.Contains("UseCloudMap = No") || text.Contains("UseCloudMap = no"))
                    {
                        CloudShadowsCheckBox.Checked = false;
                    }
                    else if (text.Contains("UseCloudMap = Yes") || text.Contains("UseCloudMap = yes"))
                    {
                        CloudShadowsCheckBox.Checked = true;
                    }

                    if (text.Contains("UseLightMap = No") || text.Contains("UseLightMap = no"))
                    {
                        ExtraGroundLightingCheckBox.Checked = false;
                    }
                    else if (text.Contains("UseLightMap = Yes") || text.Contains("UseLightMap = yes"))
                    {
                        ExtraGroundLightingCheckBox.Checked = true;
                    }

                    if (text.Contains("ShowSoftWaterEdge = No") || text.Contains("ShowSoftWaterEdge = no"))
                    {
                        SmoothWaterBordersCheckBox.Checked = false;
                    }
                    else if (text.Contains("ShowSoftWaterEdge = Yes") || text.Contains("ShowSoftWaterEdge = yes"))
                    {
                        SmoothWaterBordersCheckBox.Checked = true;
                    }

                    if (text.Contains("BuildingOcclusion = No") || text.Contains("BuildingOcclusion = no"))
                    {
                        BehindBuildingsCheckBox.Checked = false;
                    }
                    else if (text.Contains("BuildingOcclusion = Yes") || text.Contains("BuildingOcclusion = yes"))
                    {
                        BehindBuildingsCheckBox.Checked = true;
                    }

                    if (text.Contains("ShowTrees = No") || text.Contains("ShowTrees = no"))
                    {
                        ShowPropsCheckBox.Checked = false;
                    }
                    else if (text.Contains("ShowTrees = Yes") || text.Contains("ShowTrees = yes"))
                    {
                        ShowPropsCheckBox.Checked = true;
                    }

                    if (text.Contains("ExtraAnimations = No") || text.Contains("ExtraAnimations = no"))
                    {
                        ExtraAnimationsCheckBox.Checked = false;
                    }
                    else if (text.Contains("ExtraAnimations = Yes") || text.Contains("ExtraAnimations = yes"))
                    {
                        ExtraAnimationsCheckBox.Checked = true;
                    }

                    if (text.Contains("DynamicLOD = No") || text.Contains("DynamicLOD = no"))
                    {
                        DisableDynamicLODCheckBox.Checked = true;
                    }
                    else if (text.Contains("DynamicLOD = Yes") || text.Contains("DynamicLOD = yes"))
                    {
                        DisableDynamicLODCheckBox.Checked = false;
                    }

                    if (text.Contains("HeatEffects = No") || text.Contains("HeatEffects = no"))
                    {
                        HeatEffectsCheckBox.Checked = false;
                    }
                    else if (text.Contains("HeatEffects = Yes") || text.Contains("HeatEffects = yes"))
                    {
                        HeatEffectsCheckBox.Checked = true;
                    }
                }
            }


            // Get current camera height
            if (File.Exists("!Contra_Classic_GameData.big"))
            {
                try
                {
                    string s = File.ReadAllText("!Contra_Classic_GameData.big");
                    List<string> found = new List<string>();
                    string line;
                    using (StringReader file = new StringReader(s))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.Contains(" MaxCameraHeight ="))
                            {
                                found.Add(line);
                                line = line.Substring(0, line.IndexOf(".") + 1);
                                line = Regex.Replace(line, @"[^\d]", "");
                                //if ((AspectRatio(x, y) == "16:9") && isGentoolInstalled("d3d8.dll"))
                                //{
                                //    int value;
                                //    value = Convert.ToInt32(line);
                                //    camTrackBar.Value = value + 110;
                                //}
                                //else
                                //{
                                //    camTrackBar.Value = Convert.ToInt32(line);
                                //}
                                camTrackBar.Value = Convert.ToInt32(line);
                                if (Globals.GB_Checked == true)
                                {
                                    camHeightLabel.Text = "Camera Height: " + camTrackBar.Value.ToString() + ".0";
                                }
                                else if (Globals.RU_Checked == true)
                                {
                                    camHeightLabel.Text = "Высота камеры: " + camTrackBar.Value.ToString() + ".0";
                                }
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    if (Globals.GB_Checked == true)
                    {
                        MessageBox.Show("Please close !Contra_Classic_GameData.big in order to change camera height.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (Globals.RU_Checked == true)
                    {
                        MessageBox.Show("Пожалуйста, закройте !Contra_Classic_GameData.big, чтобы изменить высоту камеры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // Get current resolution
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string s = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    List<string> found = new List<string>();
                    string line;
                    using (StringReader file = new StringReader(s))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.Contains("Resolution ="))
                            {
                                found.Add(line);
                                s = line;
                                s = s.Substring(s.IndexOf('=') + 2);
                                s = s.TrimEnd();
                                string s2 = s.Replace(" ", "x");
                                //                        MessageBox.Show(s2); //shows current res
                                Properties.Settings.Default.Res = s2;
                                Properties.Settings.Default.Save();
                            }
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string s = File.ReadAllText(myDocPath + "Options.ini");
                    List<string> found = new List<string>();
                    string line;
                    using (StringReader file = new StringReader(s))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.Contains("Resolution ="))
                            {
                                found.Add(line);
                                s = line;
                                s = s.Substring(s.IndexOf('=') + 2);
                                s = s.TrimEnd();
                                string s2 = s.Replace(" ", "x");
                                //                        MessageBox.Show(s2); //shows current res
                                Properties.Settings.Default.Res = s2;
                                Properties.Settings.Default.Save();
                            }
                        }
                    }
                }
                comboBox1.Text = Properties.Settings.Default.Res;

                WinCheckBox.Checked = Properties.Settings.Default.Windowed;
                QSCheckBox.Checked = Properties.Settings.Default.Quickstart;
                LangFilterCheckBox.Checked = Properties.Settings.Default.LangF;
                Shadows3DCheckBox.Checked = Properties.Settings.Default.Shadows3D;
                DisableDynamicLODCheckBox.Checked = Properties.Settings.Default.DisableDynamicLOD;
            }
            if (!File.Exists(UserDataLeafName() + "Options.ini") && (!File.Exists(myDocPath + "Options.ini")))
            {
                if (Globals.GB_Checked == true)
                {
                    MessageBox.Show("Options.ini not found! Could not load current resolution.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Globals.RU_Checked == true)
                {
                    MessageBox.Show("Файл \"Options.ini\" не найден! Не удалось загрузить текущее разрешение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static string UserDataLeafName()
        {
            //o gets "Command and Conquer Generals Zero Hour Data" from registry. It varies depending on language
            var o = string.Empty;
            if (Globals.userOS == "32")
            {
                var userDataRegistryPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Electronic Arts\EA Games\Command and Conquer Generals Zero Hour");
                if (userDataRegistryPath != null)
                {
                    o = userDataRegistryPath.GetValue("UserDataLeafName") as string;
                }
            }
            else if (Globals.userOS == "64")
            {
                var userDataRegistryPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Electronic Arts\EA Games\Command and Conquer Generals Zero Hour");
                if (userDataRegistryPath != null)
                {
                    o = userDataRegistryPath.GetValue("UserDataLeafName") as string;
                }
            }
            if (o != null)
            {
                return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + o + @"\";
            }
            else
            {
                return null;
            }
        }

        public static string myDocPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Command and Conquer Generals Zero Hour Data\";


        //**********DRAG FORM CODE START**********
        const int WM_NCLBUTTONDBLCLK = 0xA3;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCLBUTTONDBLCLK)
                return;

            base.WndProc(ref m);
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }
            base.WndProc(ref m);
        }
        //**********DRAG FORM CODE END**********


        private void OnApplicationExit(object sender, EventArgs e) //MoreOptionsWindowExit
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.Close(); //OnApplicationExit(sender, e);
        }

        private void resOkButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(UserDataLeafName()))
            {
                string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                {
                    if (!Regex.IsMatch(comboBox1.Text, @"^[0-9]{3,4}x[0-9]{3,4}$")) //if selected res doesn't match valid input (input must match the regex)
                    {
                        if (Globals.GB_Checked == true)
                        {
                            MessageBox.Show("This resolution is not valid.", "Error");
                        }
                        else if (Globals.RU_Checked == true)
                        {
                            MessageBox.Show("Это разрешение экрана не является действительным.", "Ошибка");
                        }
                        //return;
                    }
                    else
                    {
                        string fixedText = comboBox1.Text.Replace("x", " ");
                        File.WriteAllText(UserDataLeafName() + "Options.ini", Regex.Replace(File.ReadAllText(UserDataLeafName() + "Options.ini"), "\r?\nResolution =.*", "\r\nResolution = " + fixedText + "\r"));
                        if (Globals.GB_Checked == true)
                        {
                            MessageBox.Show("Resolution changed successfully!");
                        }
                        else if (Globals.RU_Checked == true)
                        {
                            MessageBox.Show("Разрешение экрана успешно изменено!");
                        }
                        IsGeneralsRunning();
                    }
                }
            }
            else if (Directory.Exists(myDocPath))
            {
                string text = File.ReadAllText(myDocPath + "Options.ini");
                {
                    if (!Regex.IsMatch(comboBox1.Text, @"^[0-9]{3,4}x[0-9]{3,4}$")) //if selected res doesn't match valid input (input must match the regex)
                    {
                        if (Globals.GB_Checked == true)
                        {
                            MessageBox.Show("This resolution is not valid.", "Error");
                        }
                        else if (Globals.RU_Checked == true)
                        {
                            MessageBox.Show("Это разрешение экрана не является действительным.", "Ошибка");
                        }
                        //return;
                    }
                    else
                    {
                        string fixedText = comboBox1.Text.Replace("x", " ");
                        File.WriteAllText(myDocPath + "Options.ini", Regex.Replace(File.ReadAllText(myDocPath + "Options.ini"), "\r?\nResolution =.*", "\r\nResolution = " + fixedText + "\r\n"));
                        if (Globals.GB_Checked == true)
                        {
                            MessageBox.Show("Resolution changed successfully!");
                        }
                        else if (Globals.RU_Checked == true)
                        {
                            MessageBox.Show("Разрешение экрана успешно изменено!");
                        }
                    }
                }
            }
            if (!File.Exists(UserDataLeafName() + "Options.ini") && (!File.Exists(myDocPath + "Options.ini")))
            {
                if (Globals.GB_Checked == true)
                {
                    MessageBox.Show("Options.ini not found! Could not set new resolution.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Globals.RU_Checked == true)
                {
                    MessageBox.Show("Файл \"Options.ini\" не найден! Не удалось установить новое разрешение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button18_MouseEnter(object sender, EventArgs e)
        {
            button18.BackgroundImage = Properties.Resources.exit_yellow;
            button18.ForeColor = SystemColors.ButtonHighlight;
            button18.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void button18_MouseLeave(object sender, EventArgs e)
        {
            button18.BackgroundImage = Properties.Resources.exit_red;
            button18.ForeColor = SystemColors.ButtonHighlight;
            button18.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void button17_MouseEnter(object sender, EventArgs e)
        {
            button17.BackgroundImage = Properties.Resources.min_yellow;
            button17.ForeColor = SystemColors.ButtonHighlight;
            button17.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void button17_MouseLeave(object sender, EventArgs e)
        {
            button17.BackgroundImage = Properties.Resources.min_red;
            button17.ForeColor = SystemColors.ButtonHighlight;
            button17.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void LangFilterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!LangFilterCheckBox.Checked)
            {
                Properties.Settings.Default.LangF = false;
                Properties.Settings.Default.Save();
            }
            else Properties.Settings.Default.LangF = true;
            Properties.Settings.Default.Save();
            IsGeneralsRunning();
        }

        private void resOkButton_MouseDown(object sender, MouseEventArgs e)
        {
            resOkButton.BackgroundImage = Properties.Resources.btnOk3a;
            resOkButton.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void resOkButton_MouseLeave(object sender, EventArgs e)
        {
            resOkButton.BackgroundImage = Properties.Resources.btnOk3;
            resOkButton.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        public static Tuple<int, int> getScreenResolution() => Tuple.Create(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        int x = getScreenResolution().Item1;
        int y = getScreenResolution().Item2;

        public string AspectRatio(int x, int y)
        {
            double value = (double)x / y;
            if (value > 1.7)
                return "16:9";
            else
                return "4:3";
        }

        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetFileVersionInfoSize(string lptstrFilename, out int lpdwHandle);
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetFileVersionInfo(string lptstrFilename, int dwHandle, int dwLen, byte[] lpData);
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool VerQueryValue(byte[] pBlock, string lpSubBlock, out IntPtr lplpBuffer, out int puLen);

        public static bool isGentoolInstalled(string gentoolPath)
        {
            try
            {
                var size = GetFileVersionInfoSize(gentoolPath, out _);
                if (size == 0) { throw new Win32Exception(); };
                var bytes = new byte[size];
                bool success = GetFileVersionInfo(gentoolPath, 0, size, bytes);
                if (!success) { throw new Win32Exception(); }

                VerQueryValue(bytes, @"\StringFileInfo\040904E4\ProductName", out IntPtr ptr, out _);
                return Marshal.PtrToStringUni(ptr) == "GenTool";
            }
            catch //(Exception ex)
            {
                //Console.Error.WriteLine(ex);
                return false;
            }
        }

        private void ChangeCamHeight()
        {
            if (File.Exists("!Contra_Classic.big") || File.Exists("!Contra_Classic.ctr"))
            {
                if (File.Exists("!Contra_Classic_GameData.big"))
                {
                    Encoding encoding = Encoding.GetEncoding("windows-1252");
                    var regex = "";
                    //if ((AspectRatio(x, y) == "16:9") && isGentoolInstalled("d3d8.dll"))
                    //{
                    //    regex = Regex.Replace(File.ReadAllText("!Contra_Classic_GameData.big", encoding), "  MaxCameraHeight = .*\r?\n", "  MaxCameraHeight = " + (camTrackBar.Value - 110) + ".0" + " ;350.0\r\n");
                    //}
                    //else
                    //{
                    //    regex = Regex.Replace(File.ReadAllText("!Contra_Classic_GameData.big", encoding), "  MaxCameraHeight = .*\r?\n", "  MaxCameraHeight = " + camTrackBar.Value + ".0" + " ;350.0\r\n");
                    //}
                    //File.WriteAllText("!Contra_Classic_GameData.big", regex, encoding);

                    regex = Regex.Replace(File.ReadAllText("!Contra_Classic_GameData.big", encoding), "  MaxCameraHeight = .*\r?\n", "  MaxCameraHeight = " + camTrackBar.Value + ".0" + " ;350.0\r\n");
                    File.WriteAllText("!Contra_Classic_GameData.big", regex, encoding);

                    if (camTrackBar.Value > 350)
                    {
                        var regex2 = Regex.Replace(File.ReadAllText("!Contra_Classic_GameData.big", encoding), "  DrawEntireTerrain = No\r?\n", "  DrawEntireTerrain = Yes\r\n");
                        File.WriteAllText("!Contra_Classic_GameData.big", regex2, encoding);
                    }
                    else
                    {
                        var regex2 = Regex.Replace(File.ReadAllText("!Contra_Classic_GameData.big", encoding), "  DrawEntireTerrain = Yes\r?\n", "  DrawEntireTerrain = No\r\n");
                        File.WriteAllText("!Contra_Classic_GameData.big", regex2, encoding);
                    }

                    if (Globals.GB_Checked == true)
                    {
                        MessageBox.Show("Camera height changed!\n\nNOTE: High camera height values may decrease performance.");
                    }
                    else if (Globals.RU_Checked == true)
                    {
                        MessageBox.Show("Высота камеры изменена!\n\nЗаметка. Высокие значения высоты камеры могут снизить производительность.");
                    }
                }
                else
                {
                    if (Globals.GB_Checked == true)
                    {
                        MessageBox.Show("\"!Contra_Classic_GameData.big\" file not found!", "Error");
                    }
                    else if (Globals.RU_Checked == true)
                    {
                        MessageBox.Show("Файл \"!Contra_Classic_GameData.big\" не найден!", "Ошибка");
                    }
                }
            }
        }

        private void camOkButton_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCamHeight();
            }
            catch (IOException)
            {
                if (File.Exists("!Contra_Classic_GameData.big"))
                {
                    if (Globals.GB_Checked == true)
                    {
                        MessageBox.Show("Please close !Contra_Classic_GameData.big in order to change camera height.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (Globals.RU_Checked == true)
                    {
                        MessageBox.Show("Пожалуйста, закройте !Contra_Classic_GameData.big, чтобы изменить высоту камеры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void camTrackBar_Scroll(object sender, EventArgs e)
        {
            if (Globals.GB_Checked == true)
            {
                camHeightLabel.Text = "Camera Height: " + camTrackBar.Value.ToString() + ".0";
            }
            else if (Globals.RU_Checked == true)
            {
                camHeightLabel.Text = "Высота камеры: " + camTrackBar.Value.ToString() + ".0";
            }
        }

        public void IsGeneralsRunning()
        {
            if (Form.ActiveForm == this)
            {
                Process[] genByName = Process.GetProcessesByName("generals");
                if (genByName.Length > 0) //if the game is already running
                {
                    if (Globals.GB_Checked == true)
                    {
                        MessageBox.Show("Changes will take effect after game restart.", "Warning");
                    }
                    else if (Globals.RU_Checked == true)
                    {
                        MessageBox.Show("Изменения вступят в силу после перезапуска игры.", "Предупреждение");
                    }
                }
            }
        }

        private void Shadows3DCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!Shadows3DCheckBox.Checked)
            {
                Properties.Settings.Default.Shadows3D = false;
                Properties.Settings.Default.Save();

                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("UseShadowVolumes = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseShadowVolumes = Yes", "UseShadowVolumes = No"));
                        }
                        else if (text.Contains("UseShadowVolumes = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseShadowVolumes = yes", "UseShadowVolumes = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("UseShadowVolumes = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseShadowVolumes = Yes", "UseShadowVolumes = No"));
                        }
                        else if (text.Contains("UseShadowVolumes = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseShadowVolumes = yes", "UseShadowVolumes = No"));
                        }
                    }
                }
            }
            else
            {
                Properties.Settings.Default.Shadows3D = true;
                Properties.Settings.Default.Save();

                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("UseShadowVolumes = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseShadowVolumes = No", "UseShadowVolumes = Yes"));
                        }
                        else if (text.Contains("UseShadowVolumes = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseShadowVolumes = no", "UseShadowVolumes = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("UseShadowVolumes = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseShadowVolumes = No", "UseShadowVolumes = Yes"));
                        }
                        else if (text.Contains("UseShadowVolumes = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseShadowVolumes = no", "UseShadowVolumes = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void Shadows2DCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!Shadows2DCheckBox.Checked)
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("UseShadowDecals = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseShadowDecals = Yes", "UseShadowDecals = No"));
                        }
                        else if (text.Contains("UseShadowDecals = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseShadowDecals = yes", "UseShadowDecals = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("UseShadowDecals = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseShadowDecals = Yes", "UseShadowDecals = No"));
                        }
                        else if (text.Contains("UseShadowDecals = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseShadowDecals = yes", "UseShadowDecals = No"));
                        }
                    }
                }
            }
            else
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("UseShadowDecals = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseShadowDecals = No", "UseShadowDecals = Yes"));
                        }
                        else if (text.Contains("UseShadowDecals = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseShadowDecals = no", "UseShadowDecals = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("UseShadowDecals = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseShadowDecals = No", "UseShadowDecals = Yes"));
                        }
                        else if (text.Contains("UseShadowDecals = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseShadowDecals = no", "UseShadowDecals = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void CloudShadowsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!CloudShadowsCheckBox.Checked)
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("UseCloudMap = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseCloudMap = Yes", "UseCloudMap = No"));
                        }
                        else if (text.Contains("UseCloudMap = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseCloudMap = yes", "UseCloudMap = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("UseCloudMap = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseCloudMap = Yes", "UseCloudMap = No"));
                        }
                        else if (text.Contains("UseCloudMap = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseCloudMap = yes", "UseCloudMap = No"));
                        }
                    }
                }
            }
            else
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("UseCloudMap = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseCloudMap = No", "UseCloudMap = Yes"));
                        }
                        else if (text.Contains("UseCloudMap = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseCloudMap = no", "UseCloudMap = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("UseCloudMap = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseCloudMap = No", "UseCloudMap = Yes"));
                        }
                        else if (text.Contains("UseCloudMap = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseCloudMap = no", "UseCloudMap = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void ExtraGroundLightingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!ExtraGroundLightingCheckBox.Checked)
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("UseLightMap = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseLightMap = Yes", "UseLightMap = No"));
                        }
                        else if (text.Contains("UseLightMap = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseLightMap = yes", "UseLightMap = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("UseLightMap = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseLightMap = Yes", "UseLightMap = No"));
                        }
                        else if (text.Contains("UseLightMap = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseLightMap = yes", "UseLightMap = No"));
                        }
                    }
                }
            }
            else
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("UseLightMap = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseLightMap = No", "UseLightMap = Yes"));
                        }
                        else if (text.Contains("UseLightMap = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("UseLightMap = no", "UseLightMap = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("UseLightMap = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseLightMap = No", "UseLightMap = Yes"));
                        }
                        else if (text.Contains("UseLightMap = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("UseLightMap = no", "UseLightMap = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void SmoothWaterBordersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!SmoothWaterBordersCheckBox.Checked)
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("ShowSoftWaterEdge = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ShowSoftWaterEdge = Yes", "ShowSoftWaterEdge = No"));
                        }
                        else if (text.Contains("ShowSoftWaterEdge = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ShowSoftWaterEdge = yes", "ShowSoftWaterEdge = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("ShowSoftWaterEdge = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ShowSoftWaterEdge = Yes", "ShowSoftWaterEdge = No"));
                        }
                        else if (text.Contains("ShowSoftWaterEdge = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ShowSoftWaterEdge = yes", "ShowSoftWaterEdge = No"));
                        }
                    }
                }
            }
            else
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("ShowSoftWaterEdge = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ShowSoftWaterEdge = No", "ShowSoftWaterEdge = Yes"));
                        }
                        else if (text.Contains("ShowSoftWaterEdge = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ShowSoftWaterEdge = no", "ShowSoftWaterEdge = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("ShowSoftWaterEdge = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ShowSoftWaterEdge = No", "ShowSoftWaterEdge = Yes"));
                        }
                        else if (text.Contains("ShowSoftWaterEdge = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ShowSoftWaterEdge = no", "ShowSoftWaterEdge = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void BehindBuildingsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!BehindBuildingsCheckBox.Checked)
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("BuildingOcclusion = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("BuildingOcclusion = Yes", "BuildingOcclusion = No"));
                        }
                        else if (text.Contains("BuildingOcclusion = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("BuildingOcclusion = yes", "BuildingOcclusion = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("BuildingOcclusion = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("BuildingOcclusion = Yes", "BuildingOcclusion = No"));
                        }
                        else if (text.Contains("BuildingOcclusion = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("BuildingOcclusion = yes", "BuildingOcclusion = No"));
                        }
                    }
                }
            }
            else
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("BuildingOcclusion = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("BuildingOcclusion = No", "BuildingOcclusion = Yes"));
                        }
                        else if (text.Contains("BuildingOcclusion = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("BuildingOcclusion = no", "BuildingOcclusion = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("BuildingOcclusion = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("BuildingOcclusion = No", "BuildingOcclusion = Yes"));
                        }
                        else if (text.Contains("BuildingOcclusion = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("BuildingOcclusion = no", "BuildingOcclusion = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void ShowPropsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!ShowPropsCheckBox.Checked)
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("ShowTrees = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ShowTrees = Yes", "ShowTrees = No"));
                        }
                        else if (text.Contains("ShowTrees = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ShowTrees = yes", "ShowTrees = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("ShowTrees = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ShowTrees = Yes", "ShowTrees = No"));
                        }
                        else if (text.Contains("ShowTrees = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ShowTrees = yes", "ShowTrees = No"));
                        }
                    }
                }
            }
            else
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("ShowTrees = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ShowTrees = No", "ShowTrees = Yes"));
                        }
                        else if (text.Contains("ShowTrees = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ShowTrees = no", "ShowTrees = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("ShowTrees = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ShowTrees = No", "ShowTrees = Yes"));
                        }
                        else if (text.Contains("ShowTrees = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ShowTrees = no", "ShowTrees = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void ExtraAnimationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!ExtraAnimationsCheckBox.Checked)
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("ExtraAnimations = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ExtraAnimations = Yes", "ExtraAnimations = No"));
                        }
                        else if (text.Contains("ExtraAnimations = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ExtraAnimations = yes", "ExtraAnimations = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("ExtraAnimations = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ExtraAnimations = Yes", "ExtraAnimations = No"));
                        }
                        else if (text.Contains("ExtraAnimations = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ExtraAnimations = yes", "ExtraAnimations = No"));
                        }
                    }
                }
            }
            else
            {
                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("ExtraAnimations = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ExtraAnimations = No", "ExtraAnimations = Yes"));
                        }
                        else if (text.Contains("ExtraAnimations = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("ExtraAnimations = no", "ExtraAnimations = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("ExtraAnimations = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ExtraAnimations = No", "ExtraAnimations = Yes"));
                        }
                        else if (text.Contains("ExtraAnimations = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("ExtraAnimations = no", "ExtraAnimations = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void DisableDynamicLODCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DisableDynamicLODCheckBox.Checked)
            {
                Properties.Settings.Default.DisableDynamicLOD = true;
                Properties.Settings.Default.Save();

                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("DynamicLOD = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("DynamicLOD = Yes", "DynamicLOD = No"));
                        }
                        else if (text.Contains("DynamicLOD = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("DynamicLOD = yes", "DynamicLOD = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("DynamicLOD = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("DynamicLOD = Yes", "DynamicLOD = No"));
                        }
                        else if (text.Contains("DynamicLOD = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("DynamicLOD = yes", "DynamicLOD = No"));
                        }
                    }
                }
            }
            else
            {
                Properties.Settings.Default.DisableDynamicLOD = false;
                Properties.Settings.Default.Save();

                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("DynamicLOD = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("DynamicLOD = No", "DynamicLOD = Yes"));
                        }
                        else if (text.Contains("DynamicLOD = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("DynamicLOD = no", "DynamicLOD = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("DynamicLOD = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("DynamicLOD = No", "DynamicLOD = Yes"));
                        }
                        else if (text.Contains("DynamicLOD = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("DynamicLOD = no", "DynamicLOD = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void HeatEffectsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!HeatEffectsCheckBox.Checked)
            {
                Properties.Settings.Default.HeatEffects = false;
                Properties.Settings.Default.Save();

                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("HeatEffects = Yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("HeatEffects = Yes", "HeatEffects = No"));
                        }
                        else if (text.Contains("HeatEffects = yes"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("HeatEffects = yes", "HeatEffects = No"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("HeatEffects = Yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("HeatEffects = Yes", "HeatEffects = No"));
                        }
                        else if (text.Contains("HeatEffects = yes"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("HeatEffects = yes", "HeatEffects = No"));
                        }
                    }
                }
            }
            else
            {
                Properties.Settings.Default.HeatEffects = true;
                Properties.Settings.Default.Save();

                if (Directory.Exists(UserDataLeafName()))
                {
                    string text = File.ReadAllText(UserDataLeafName() + "Options.ini");
                    {
                        if (text.Contains("HeatEffects = No"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("HeatEffects = No", "HeatEffects = Yes"));
                        }
                        else if (text.Contains("HeatEffects = no"))
                        {
                            File.WriteAllText(UserDataLeafName() + "Options.ini", File.ReadAllText(UserDataLeafName() + "Options.ini").Replace("HeatEffects = no", "HeatEffects = Yes"));
                        }
                    }
                }
                else if (Directory.Exists(myDocPath))
                {
                    string text = File.ReadAllText(myDocPath + "Options.ini");
                    {
                        if (text.Contains("HeatEffects = No"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("HeatEffects = No", "HeatEffects = Yes"));
                        }
                        else if (text.Contains("HeatEffects = no"))
                        {
                            File.WriteAllText(myDocPath + "Options.ini", File.ReadAllText(myDocPath + "Options.ini").Replace("HeatEffects = no", "HeatEffects = Yes"));
                        }
                    }
                }
            }
            IsGeneralsRunning();
        }

        private void WinCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!WinCheckBox.Checked)
            {
                Properties.Settings.Default.Windowed = false;
                Properties.Settings.Default.Save();
            }
            else Properties.Settings.Default.Windowed = true;
            Properties.Settings.Default.Save();
            IsGeneralsRunning();
        }

        private void QSCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!QSCheckBox.Checked)
            {
                Properties.Settings.Default.Quickstart = false;
                Properties.Settings.Default.Save();
            }
            else Properties.Settings.Default.Quickstart = true;
            Properties.Settings.Default.Save();
            IsGeneralsRunning();
        }
    }
}