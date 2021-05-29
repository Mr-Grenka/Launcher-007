using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Net;
using System.Runtime.InteropServices;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;
using System.Management;

namespace Contra
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            //Determine OS bitness
            if (IntPtr.Size == 8)
            {
                Globals.userOS = "64";
            }
            else
            {
                Globals.userOS = "32";
            }

            DelTmpChunk();
        }

        string currentFileLabel;
        //string currentFile;

        string newVersion, genToolFileName = "";

        //int modVersionLocalInt;
        //bool patch1Found, patch2Found;
        string versions_url = "https://raw.githubusercontent.com/ContraMod/Launcher-007/master/Versions.txt";
        string launcher_url = "https://github.com/ContraMod/Launcher-007/releases/download/";
        string patch_url = "http://cnc-contra.ru/download/update/"; //"http://contra.cncguild.net/Downloads/";

        int patchNumberInt = 0;

        bool applyNewLauncher = false;

        static string launcherExecutingPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetFileVersionInfoSize(string lptstrFilename, out int lpdwHandle);
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetFileVersionInfo(string lptstrFilename, int dwHandle, int dwLen, byte[] lpData);
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool VerQueryValue(byte[] pBlock, string lpSubBlock, out IntPtr lplpBuffer, out int puLen);

        public static readonly CancellationTokenSource httpCancellationToken = new CancellationTokenSource();

        public async void GetLauncherUpdate(string versionsTXT, string launcher_url)
        {
            string launcher_ver = versionsTXT.Substring(versionsTXT.LastIndexOf("Launcher: ") + 10);
            newVersion = launcher_ver.Substring(0, launcher_ver.IndexOf("$"));
            string zip_url = launcher_url + launcher_ver.Substring(0, launcher_ver.IndexOf("$")) + @"/Contra007Classic_Launcher.zip";
            string zip_path = zip_url.Split('/').Last();

            // If there is a new launcher version, call the DownloadUpdate method
            if (newVersion != Application.ProductVersion)
            {
                try
                {
                    var updatePendingText = new Dictionary<Tuple<string, string>, bool>
                    {
                        { Tuple.Create($"Contra Launcher version {newVersion} is available! Click OK to update and restart!", "Update Available"), Globals.GB_Checked},
                        { Tuple.Create($"Версия Contra Launcher {newVersion} доступна! Нажмите «ОК», чтобы обновить и перезапустить!", "Доступно обновление"), Globals.RU_Checked},
                    }.Single(l => l.Value).Key;
                    MessageBox.Show(new Form { TopMost = true }, updatePendingText.Item1, updatePendingText.Item2, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await DownloadFile(zip_url, zip_path, TimeSpan.FromMinutes(5), httpCancellationToken.Token);

                    using (ZipArchive archive = await Task.Run(() => ZipFile.OpenRead(zip_path)))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name == "Contra007Classic_Launcher.exe") continue;
                            await Task.Run(() => entry.ExtractToFile(entry.Name, true));
                        }
                    }

                    File.Delete(zip_path);
                    applyNewLauncher = true;

                    // Show a message when the launcher download has completed
                    var updateDoneText = new Dictionary<Tuple<string, string>, bool>
                    {
                        { Tuple.Create("Your application is now up-to-date!\n\nThe application will now restart!", "Update Complete"), Globals.GB_Checked},
                        { Tuple.Create("Ваше приложение теперь обновлено!\n\nПриложение будет перезагружено!", "Обновление завершено"), Globals.RU_Checked},
                    }.Single(l => l.Value).Key;
                    MessageBox.Show(new Form { TopMost = true }, updateDoneText.Item1, updateDoneText.Item2, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //this.Close();
                    Application.Restart();
                    Environment.Exit(0);
                }
                catch (OperationCanceledException)
                {
                    applyNewLauncher = false;
                    File.Delete(zip_path); // Clean-up partial download
                    PatchDLPanel.Hide();
                }
                catch (Exception ex)
                {
                    applyNewLauncher = false;
                    File.Delete(zip_path); // Clean-up partial download
                    PatchDLPanel.Hide();
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void UpdateLogic()
        {
            string versionsTXT = (new WebClient { Encoding = Encoding.UTF8 }).DownloadString(versions_url);

            // Update launcher
            GetLauncherUpdate(versionsTXT, launcher_url);

            // Update patch
            string launcher_ver = versionsTXT.Substring(versionsTXT.LastIndexOf("Launcher: ") + 10);
            newVersion = launcher_ver.Substring(0, launcher_ver.IndexOf("$"));

            // Get patch number
            string patchNumber = versionsTXT.Substring(versionsTXT.LastIndexOf("Patch: ") + 7); // The latest patch number
            patchNumber = patchNumber.Substring(0, patchNumber.IndexOf("$"));
            patchNumberInt = int.Parse(patchNumber);

            //patchNumberInt = 0;
            string exclamationMark = "";

            // If launcher is up to date and patches are missing, update the mod
            if ((newVersion == Application.ProductVersion) && (File.Exists("!Contra_Classic.ctr") || File.Exists("!Contra_Classic.big") && patchNumberInt > 0))
            {
                for (int i = 1; i < patchNumberInt + 1; i++)
                {
                    exclamationMark += "!";
                    if (!File.Exists(exclamationMark + "!Contra_Classic_Patch" + i + ".ctr") && !File.Exists(exclamationMark + "!Contra_Classic_Patch" + i + ".big"))
                    {
                        //MessageBox.Show(exclamationMark);
                        //MessageBox.Show(exclamationMark + "!Contra_Classic_Patch" + i + ".ctr");
                        //MessageBox.Show("getting " + i.ToString());
                        GetModUpdate(versionsTXT, patch_url, i);
                        break;
                    }
                }
            }

            //Load MOTD
            new Thread(() => ThreadProcSafeMOTD(versionsTXT)) { IsBackground = true }.Start();
        }

        private void RadioFlag_GB_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioFlag_GB.Checked)
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
                resources.ApplyResources(this, "$this");
                applyResources(resources, Controls);
                Globals.RU_Checked = false;
                Globals.GB_Checked = true;
                this.BackgroundImage = Properties.Resources.background_en;
                buttonLaunch.BackgroundImage = Properties.Resources.play_button_red;
                moreOptions.BackgroundImage = Properties.Resources.settings_button_red;
                toolTip1.SetToolTip(RadioENQuotes, "Each faction's units will speak English language.");
                toolTip1.SetToolTip(RadioRUQuotes, "Each faction's units will speak Russian language.");
                toolTip1.SetToolTip(RadioEN, "English in-game language.");
                toolTip1.SetToolTip(RadioRU, "Russian in-game language.");
                toolTip1.SetToolTip(MNew, "Use new soundtracks.");
                toolTip1.SetToolTip(MStandard, "Use standard Zero Hour soundtracks.");
                toolTip1.SetToolTip(DefaultPics, "Use default general portraits.");
                toolTip1.SetToolTip(GoofyPics, "Use funny general portraits.");
                currentFileLabel = "File: ";
                ModDLLabel.Text = "Download progress: ";
                CancelModDLBtn.Text = "Cancel";
                string verString, yearString = "";
                if (File.Exists("!Contra_Classic.big") || File.Exists("!Contra_Classic.ctr"))
                {
                    verString = "1";
                    yearString = "2021";
                }
                else
                {
                    verString = "???";
                    yearString = "2021";
                }
                //versionLabel.Text = "Contra Classic " + yearString + " - Version " + verString + " - Launcher: " + Application.ProductVersion;
                versionLabel.Text = "Contra Classic " + yearString + " - Launcher: " + Application.ProductVersion;

                // Temporary hack so update runs on main thread, versionsTXT should be rewritten to be async if possible
                try
                {
                    UpdateLogic();
                }
                catch { }
            }
        }

        private void RadioFlag_RU_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioFlag_RU.Checked)
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
                ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
                resources.ApplyResources(this, "$this");
                applyResources(resources, Controls);
                Globals.GB_Checked = false;
                Globals.RU_Checked = true;
                this.BackgroundImage = Properties.Resources.background;
                buttonLaunch.BackgroundImage = Properties.Resources.play_button_red_rus;
                moreOptions.BackgroundImage = Properties.Resources.settings_button_red_rus;
                toolTip1.SetToolTip(RadioENQuotes, "Юниты каждой фракций будут разговаривать на английском языке.");
                toolTip1.SetToolTip(RadioRUQuotes, "Юниты каждой фракций будут разговаривать на русском языке.");
                toolTip1.SetToolTip(RadioEN, "Английский язык.");
                toolTip1.SetToolTip(RadioRU, "Русский язык.");
                toolTip1.SetToolTip(MNew, "Включить новые саундтреки.");
                toolTip1.SetToolTip(MStandard, "Включить стандартные саундтреки Zero Hour.");
                toolTip1.SetToolTip(DefaultPics, "Включить портреты Генералов по умолчанию.");
                toolTip1.SetToolTip(GoofyPics, "Включить смешные портреты Генералов.");
                RadioENQuotes.Text = "Англ.";
                RadioRUQuotes.Text = "Русский";
                MNew.Text = "Новая";
                MStandard.Text = "ZH";
                RadioEN.Text = "Англ.";
                RadioRU.Text = "Русский";
                DefaultPics.Text = "По умолч.";
                GoofyPics.Text = "Смешные";
                currentFileLabel = "Файл: ";
                ModDLLabel.Text = "Прогресс загрузки: ";
                CancelModDLBtn.Text = "Отмена";
                string verString, yearString = "";
                if (File.Exists("!Contra_Classic.big") || File.Exists("!Contra_Classic.ctr"))
                {
                    verString = "1";
                    yearString = "2021";
                }
                else
                {
                    verString = "???";
                    yearString = "2021";
                }
                //versionLabel.Text = "Contra Classic " + yearString + " - Версия " + verString + " - Launcher: " + Application.ProductVersion;
                versionLabel.Text = "Contra Classic " + yearString + " - Launcher: " + Application.ProductVersion;

                // Temporary hack so update runs on main thread, versionsTXT should be rewritten to be async if possible
                try
                {
                    UpdateLogic();
                }
                catch { }
            }
        }

        public async void GetModUpdate(string versionsTXT, string patch_url, int patchNumber)
        {
            string zip_url = null;

            zip_url = patch_url + @"/ContraClassicPatch" + patchNumber + ".zip";
            //MessageBox.Show(patch_url + @"/ContraClassicPatch" + patchNumber + ".zip");

            string zip_path = zip_url.Split('/').Last();

            // Get mod version text
            string modVersionText = versionsTXT.Substring(versionsTXT.LastIndexOf("Mod Text: ") + 10); // The latest patch name
            modVersionText = modVersionText.Substring(0, modVersionText.IndexOf("$"));

            try
            {
                var updatePendingText = new Dictionary<Tuple<string, string>, bool>
                    {
                        { Tuple.Create($"Contra version {modVersionText} is available!\n\nNote: If you play online, you should download the new version at all costs, otherwise the game will mismatch!\n\nWould you like to download and update now?", "Update Available"), Globals.GB_Checked},
                        { Tuple.Create($"Версия Contra {modVersionText} доступна!\n\nПримечание: Если вы играете онлайн, вам следует загрузить новую версию любой ценой, иначе игра выдаст ошибку несоответствия!\n\nХотите скачать и обновить сейчас?", "Доступно обновление"), Globals.RU_Checked},
                    }.Single(l => l.Value).Key;
                DialogResult dialogResult = MessageBox.Show(new Form { TopMost = true }, updatePendingText.Item1, updatePendingText.Item2, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    await DownloadFile(zip_url, zip_path, TimeSpan.FromMinutes(5), httpCancellationToken.Token);

                    using (ZipArchive archive = await Task.Run(() => ZipFile.OpenRead(zip_path)))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            //if (entry.Name == "Contra_Launcher.exe") continue;
                            await Task.Run(() => entry.ExtractToFile(entry.Name, true));
                        }
                    }

                    File.Delete(zip_path);
                    //restartLauncher = true;

                    // Show a message when the patch download has completed
                    var updateDoneText = new Dictionary<Tuple<string, string>, bool>
                        {
                            { Tuple.Create($"{modVersionText} was installed successfully! The launcher will now restart!", "Update Complete"), Globals.GB_Checked},
                            { Tuple.Create($"{modVersionText} установлен успешно! Лаунчер перезапустится!", "Обновление завершено"), Globals.RU_Checked},
                        }.Single(l => l.Value).Key;
                    MessageBox.Show(new Form { TopMost = true }, updateDoneText.Item1, updateDoneText.Item2, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //this.Close();
                    Application.Restart();
                    Environment.Exit(0);
                }
                else
                { }
            }
            catch (OperationCanceledException)
            {
                //restartLauncher = false;
                File.Delete(zip_path); // Clean-up partial download
                PatchDLPanel.Hide();
            }
            catch (Exception ex)
            {
                //restartLauncher = false;
                File.Delete(zip_path); // Clean-up partial download
                PatchDLPanel.Hide();
                MessageBox.Show(ex.ToString());
            }
        }

        public static readonly HttpClient httpclient = new HttpClient();

        public static Tuple<double, string> ByteToSizeType(long value)
        {
            if (value == 0L) return Tuple.Create(0D, "Bytes"); // zero is plural
            IReadOnlyDictionary<long, string> thresholds = new Dictionary<long, string>()
                {
                    { 1, "Byte" },
                    { 2, "Bytes" },
                    { 1024, "KiB" },
                    { 1048576, "MiB" },
                    { 1073741824, "GiB" },
                    { 1099511627776, "TiB" },
                    { 1125899906842620, "PiB" },
                    { 1152921504606850000, "EiB" },
                };
            for (int t = thresholds.Count - 1; t > 0; t--)
            {
                if (value >= thresholds.ElementAt(t).Key) return Tuple.Create(Math.Round((double)value / thresholds.ElementAt(t).Key, 2), thresholds.ElementAt(t).Value);
            }
            // handle negative values if given
            var reValue = ByteToSizeType(-value);
            return Tuple.Create(-reValue.Item1, reValue.Item2);
        }

        public async Task DownloadFile(string url, string outPath, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            httpclient.Timeout = timeout;
            var response = httpclient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            response.EnsureSuccessStatusCode();

            var contentLength = response.Content.Headers.ContentLength.GetValueOrDefault();
            var totalToDownload = ByteToSizeType(contentLength);
            var downloadSize = totalToDownload.Item1;
            var downloadUnit = totalToDownload.Item2;
            PatchDLPanel.Show();

            using (Stream contentStream = await response.Content.ReadAsStreamAsync(), fileStream = new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                var data = new byte[8192];
                long totalBytesRead = 0L, readCount = 0L;
                bool bytesRemaining = true;

                do
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var bytesRead = await contentStream.ReadAsync(data, 0, data.Length, cancellationToken);

                    if (bytesRead == 0) bytesRemaining = false;
                    else
                    {
                        await fileStream.WriteAsync(data, 0, bytesRead);
                        totalBytesRead += bytesRead;
                        readCount += 1;
                        if (readCount % 100 == 0)
                        {
                            PatchDLProgressBar.Value = Convert.ToInt32((double)totalBytesRead / contentLength * 100);
                            DLPercentLabel.Text = $"{(double)totalBytesRead / contentLength * 100:F2}%";
                            ModDLCurrentFileLabel.Text = currentFileLabel + outPath;
                            ModDLFileSizeLabel.Text = $"{ByteToSizeType(totalBytesRead).Item1} / {downloadSize} {downloadUnit}";
                            //ModDLFileSizeLabel.Text = $"{BytesToSize(e.BytesReceived, SizeUnits.MiB)} MiB / {BytesToSize(e.TotalBytesToReceive, SizeUnits.MiB)} MiB";
                        }
                    }
                }
                while (bytesRemaining);
            }
            PatchDLPanel.Hide();
        }

        public static async Task DownloadFileSimple(string url, string outPath, TimeSpan timeout)
        {
            httpclient.Timeout = timeout;
            var response = httpclient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            response.EnsureSuccessStatusCode();

            using (var contentStream = new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                await response.Content.CopyToAsync(contentStream);
            }
        }

        //Old WebClient based implementation -- can't cancel download in progress
        //readonly WebClient wcDL = new WebClient();
        //public async Task DownloadFile(string url, string output, TimeSpan timeout)
        //{
        //    PatchDLPanel.Show();
        //    DateTime? lastReceived = null;
        //    wcDL.DownloadProgressChanged += (o, e) =>
        //    {
        //        lastReceived = DateTime.Now;
        //        PatchDLProgressBar.Value = e.ProgressPercentage;
        //        DLPercentLabel.Text = e.ProgressPercentage.ToString() + "%";
        //        ModDLCurrentFileLabel.Text = currentFileLabel + output;
        //        ModDLFileSizeLabel.Text = $"{BytesToSize(e.BytesReceived, SizeUnits.MiB)} MiB / {BytesToSize(e.TotalBytesToReceive, SizeUnits.MiB)} MiB";
        //    };

        //    var download = wcDL.DownloadFileTaskAsync(url, output);

        //    // await until download is completed or timeout expires
        //    while (lastReceived == null || DateTime.Now - lastReceived < timeout)
        //    {
        //        await Task.WhenAny(Task.Delay(1000), download); // 1 second wait vs download task
        //        if (download.IsCompleted) break;
        //    }

        //    PatchDLPanel.Hide();
        //    if (download.IsCanceled) throw new TaskCanceledException("File cancelled by user.");

        //    var exception = download.Exception;
        //    bool timed_out = !download.IsCompleted && exception == null;

        //    // download did not complete, nor did it fail, most likely user's connection dropped, let's cancel it
        //    if (timed_out) wcDL.CancelAsync();

        //    if (timed_out || exception != null)
        //    {
        //        // delete partially downloaded file if any (CancelAsync() is not immediate so multiple tries might be needed)
        //        int fails = 0;
        //        while (true)
        //        {
        //            try
        //            {
        //                File.Delete(output);
        //                break;
        //            }
        //            catch
        //            {
        //                fails++;
        //                if (fails >= 10) break;

        //                await Task.Delay(1000);
        //            }
        //        }
        //    }

        //    if (exception != null) throw new Exception("Failed to download file", exception);
        //    if (timed_out) throw new Exception($"Failed to download file (timeout reached: {timeout})");
        //}

        // There should be only one instance of HttpClient for more efficient socket usage, no need to wrap it in a 'using' or dispose it.

        //public void DownloadModUpdate(string patch_url)
        //{
        //    void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //    {
        //        // In case you don't have a progressBar Log the value instead 
        //        // Console.WriteLine(e.ProgressPercentage);
        //        PatchDLProgressBar.Value = e.ProgressPercentage;
        //        DLPercentLabel.Text = e.ProgressPercentage.ToString() + "%";
        //        ModDLCurrentFileLabel.Text = currentFileLabel + currentFile;
        //        FileInfo f = new FileInfo(currentFile);
        //        long s1 = f.Length;
        //        ModDLFileSizeLabel.Text = BytesToString(s1) + " / " + BytesToString(bytes_total);
        //    }

        //    void wc_DownloadPatchCompleted(object sender, AsyncCompletedEventArgs e)
        //    {
        //        PatchDLPanel.Hide();
        //        //TO-DO: update version string? Currently handled by forced launcher restart

        //        if (e.Cancelled)
        //        {
        //            // delete the partially-downloaded file
        //            File.Delete(patchFileName);
        //            return;
        //        }

        //        // display completion status.
        //        if (e.Error != null)
        //        {
        //            MessageBox.Show(e.Error.Message);
        //            return;
        //        }

        //        // Extract patch zip
        //        string zipPath = launcherExecutingPath + @"\" + patchFileName;
        //        //if (patchFileName == "Contra009FinalPatch2.zip") //If the current patch installed is patch 2
        //        //{
        //        //    try
        //        //    {
        //        //        Directory.Delete("contra"); //Delete old contra folder containing tinc vpn scripts
        //        //    }
        //        //    catch { }
        //        //}
        //        try //To prevent crash
        //        {
        //            ZipFile.ExtractToDirectory(zipPath, launcherExecutingPath);
        //        }
        //        catch { }
        //        File.Delete(patchFileName);

        //        // Show a message when the patch download has completed
        //        var updateLangText = new Dictionary<Tuple<string, string>, bool>
        //    {
        //        { Tuple.Create("A new patch has been downloaded!\n\nThe application will now restart!", "Update Complete"), Globals.GB_Checked},
        //        { Tuple.Create("Новый патч был загружен!\n\nПриложение будет перезагружено!", "Обновление завершено"), Globals.RU_Checked},
        //        { Tuple.Create("Новий виправлення завантажено!\n\nПрограма буде перезавантажена!", "Оновлення завершено"), Globals.UA_Checked},
        //        { Tuple.Create("Нов пач беше изтеглен!\n\nСега ще се рестартира!", "Обновяването е завършено"), Globals.BG_Checked},
        //        { Tuple.Create("Ein neuer Patch wurde heruntergeladen!\n\nDas Programm wird sich jetzt neu starten!", "Aktualisierung abgeschlossen"), Globals.DE_Checked},
        //    }.Single(l => l.Value).Key;
        //        MessageBox.Show(new Form { TopMost = true }, updateLangText.Item1, updateLangText.Item2, MessageBoxButtons.OK, MessageBoxIcon.Information);

        //        System.Diagnostics.Process.Start(launcherExecutingPath + "\\Contra007Classic_Launcher.exe");
        //        this.Close();
        //    }
        //    try
        //    {
        //        wcMod.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadPatchCompleted);
        //        wcMod.DownloadProgressChanged += wc_DownloadProgressChanged;

        //        // Download one patch at a time
        //        if (modVersionLocalInt != 2) //If user doesn't have the latest patch
        //        {
        //            if (patch1Found == false)
        //            {
        //                patchFileName = "Contra009FinalPatch1.zip";
        //            }
        //            else if (patch2Found == false)
        //            {
        //                patchFileName = "Contra009FinalPatch2.zip";
        //            }
        //            CheckIfFileIsAvailable(patch_url + patchFileName);
        //            currentFile = patchFileName;
        //            wcMod.OpenRead(patch_url + patchFileName);
        //            bytes_total = Convert.ToInt64(wcMod.ResponseHeaders["Content-Length"]);

        //            wcMod.DownloadFileAsync(new Uri(patch_url + patchFileName), Path.Combine(launcherExecutingPath, patchFileName));
        //        }
        //        PatchDLPanel.Show();

        //    }
        //    catch {}
        //}

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

        public static bool wait = true;

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

        //**********MINIMIZE FORM CODE START**********
        const int WS_MINIMIZEBOX = 0x20000;
        const int CS_DBLCLKS = 0x8;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        }
        //**********MINIMIZE FORM CODE END**********

        private void CheckInstallDir()
        {
            if (Globals.GB_Checked == true)
            {
                MessageBox.Show("You have installed Contra in the wrong folder. Install it in the Zero Hour folder which contains the \"generals.exe\". It's very often the parent folder.", "Error");
            }
            else if (Globals.RU_Checked == true)
            {
                MessageBox.Show("Вы установили Contra в неправильную папку. Установите его в папку Zero Hour, которая содержит файл \"generals.exe\". Это очень часто предыдущая папка.", "Ошибка");
            }
        }

        private void DelTmpChunk()
        {
            if (File.Exists(UserDataLeafName() + "_tmpChunk.dat"))
            {
                File.Delete(UserDataLeafName() + "_tmpChunk.dat");
            }
            else if (File.Exists(myDocPath + "_tmpChunk.dat"))
            {
                File.Delete(myDocPath + "_tmpChunk.dat");
            }
        }

        private void DeleteDuplicateFiles()
        {
            string exclamationMark = "";
            if (patchNumberInt > 0)
            {
                for (int i = 1; i < patchNumberInt + 1; i++)
                {
                    exclamationMark += "!";
                    if (File.Exists(exclamationMark + "!Contra_Classic_Patch" + i + ".ctr") && File.Exists(exclamationMark + "!Contra_Classic_Patch" + i + ".big"))
                    {
                        File.Delete(exclamationMark + "!Contra_Classic_Patch" + i + ".big");
                    }
                }
            }
            if (File.Exists("!Contra_Classic_GameData.ctr") && File.Exists("!Contra_Classic_GameData.big"))
            {
                File.Delete("!Contra_Classic_GameData.big");
            }
            if (File.Exists("!Contra_Classic.ctr") && File.Exists("!Contra_Classic.big"))
            {
                File.Delete("!Contra_Classic.big");
            }
            if (File.Exists("!Contra_Classic_RUVO.ctr") && File.Exists("!Contra_Classic_RUVO.big"))
            {
                File.Delete("!Contra_Classic_RUVO.big");
            }
            if (File.Exists("!Contra_Classic_ENVO.ctr") && File.Exists("!Contra_Classic_ENVO.big"))
            {
                File.Delete("!Contra_Classic_ENVO.big");
            }
            if (File.Exists("!!Contra_Classic_FunnyGenPics.ctr") && File.Exists("!!Contra_Classic_FunnyGenPics.big"))
            {
                File.Delete("!!Contra_Classic_FunnyGenPics.big");
            }
            if (File.Exists("!Contra_Classic_NewMusic.ctr") && File.Exists("!Contra_Classic_NewMusic.big"))
            {
                File.Delete("!Contra_Classic_NewMusic.big");
            }
            if (File.Exists("!Contra_Classic_EN.ctr") && File.Exists("!Contra_Classic_EN.big"))
            {
                File.Delete("!Contra_Classic_EN.big");
            }
            if (File.Exists("!Contra_Classic_RU.ctr") && File.Exists("!Contra_Classic_RU.big"))
            {
                File.Delete("!Contra_Classic_RU.big");
            }
            if (File.Exists("langdata.dat") && File.Exists("langdata1.dat"))
            {
                File.Delete("langdata1.dat");
            }
            //if (File.Exists("dbghelp.dll") && File.Exists("dbghelp.backup"))
            //{
            //    File.Delete("dbghelp.backup");
            //}
        }

        private void RenameBigToCtr()
        {
            try
            {
                string exclamationMark = "";
                if (patchNumberInt > 0)
                {
                    for (int i = 1; i < patchNumberInt + 1; i++)
                    {
                        exclamationMark += "!";
                        File.Move(exclamationMark + "!Contra_Classic_Patch" + i + ".big", exclamationMark + "!Contra_Classic_Patch" + i + ".ctr");
                    }
                }
                List<string> bigs = new List<string>
                {
                    "!Contra_Classic_GameData.big",
                    "!Contra_Classic.big",
                    "!Contra_Classic_RUVO.big",
                    "!Contra_Classic_ENVO.big",
                    "!!Contra_Classic_FunnyGenPics.big",
                    "!Contra_Classic_NewMusic.big",
                    "!Contra_Classic_EN.big",
                    "!Contra_Classic_RU.big",
                };
                foreach (string big in bigs)
                {
                    string ctr = big.Replace(".big", ".ctr");
                    if (File.Exists(big))
                    {
                        File.Move(big, ctr);
                    }
                }
                if (File.Exists("langdata1.dat"))
                {
                    File.Move("langdata1.dat", "langdata.dat");
                }
                if (Directory.Exists(@"Data\Scripts1"))
                {
                    Directory.Move(@"Data\Scripts1", @"Data\Scripts");
                }

                //File.Move("dbghelp.backup", "dbghelp.dll");

                if (File.Exists("Install_Final_ZH.bmp"))
                {
                    try
                    {
                        File.SetAttributes("Install_Final.bmp", FileAttributes.Normal);
                        File.SetAttributes("Install_Final_ZH.bmp", FileAttributes.Normal);
                        File.SetAttributes("Install_Final_Contra.bmp", FileAttributes.Normal);
                        File.Copy("Install_Final_ZH.bmp", "Install_Final.bmp", true);
                    }
                    catch
                    { }
                }

                if (File.Exists("generals_zh.exe"))
                {
                    try
                    {
                        File.SetAttributes("generals.exe", FileAttributes.Normal);
                        File.SetAttributes("generals_zh.exe", FileAttributes.Normal);
                        File.SetAttributes("generals.ctr", FileAttributes.Normal);
                        File.Copy("generals_zh.exe", "generals.exe", true);
                    }
                    catch
                    { }
                }

                // Save CTR Options; Make ZH Options.ini active
                try
                {
                    if (File.Exists(UserDataLeafName() + "Options_ZH.ini"))
                    {
                        File.SetAttributes(UserDataLeafName() + "Options.ini", FileAttributes.Normal);
                        File.SetAttributes(UserDataLeafName() + "Options_CTR.ini", FileAttributes.Normal);
                        File.SetAttributes(UserDataLeafName() + "Options_ZH.ini", FileAttributes.Normal);
                        File.Copy(UserDataLeafName() + "Options.ini", UserDataLeafName() + "Options_CTR.ini", true);
                        File.Copy(UserDataLeafName() + "Options_ZH.ini", UserDataLeafName() + "Options.ini", true);
                    }
                    else if (File.Exists(myDocPath + "Options_ZH.ini"))
                    {
                        File.SetAttributes(myDocPath + "Options.ini", FileAttributes.Normal);
                        File.SetAttributes(myDocPath + "Options_CTR.ini", FileAttributes.Normal);
                        File.SetAttributes(myDocPath + "Options_ZH.ini", FileAttributes.Normal);
                        File.Copy(myDocPath + "Options.ini", myDocPath + "Options_CTR.ini", true);
                        File.Copy(myDocPath + "Options_ZH.ini", myDocPath + "Options.ini", true);
                    }
                }
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
                catch
                { }
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            catch
            { }
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            if (RadioFlag_GB.Checked == true)
            {
                buttonLaunch.BackgroundImage = Properties.Resources.play_button_yellow;
            }
            else
            {
                buttonLaunch.BackgroundImage = Properties.Resources.play_button_yellow_rus;
            }
            buttonLaunch.ForeColor = SystemColors.ButtonHighlight;
            buttonLaunch.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            if (RadioFlag_GB.Checked == true)
            {
                buttonLaunch.BackgroundImage = Properties.Resources.play_button_red;
            }
            else
            {
                buttonLaunch.BackgroundImage = Properties.Resources.play_button_red_rus;
            }
            buttonLaunch.ForeColor = SystemColors.ButtonHighlight;
            buttonLaunch.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void button1_Click(object sender, EventArgs e) // LaunchButton
        {
            DeleteDuplicateFiles();
            RenameBigToCtr();
            try
            {
                try
                {
                    string exclamationMark = "";
                    if (patchNumberInt > 0)
                    {
                        for (int i = 1; i < patchNumberInt + 1; i++)
                        {
                            exclamationMark += "!";
                            File.Move(exclamationMark + "!Contra_Classic_Patch" + i + ".ctr", exclamationMark + "!Contra_Classic_Patch" + i + ".big");
                        }
                    }
                    File.Move("!Contra_Classic.ctr", "!Contra_Classic.big");
                    File.Move("!Contra_Classic_GameData.ctr", "!Contra_Classic_GameData.big");

                    // Remove dbghelp to fix DirectX error on game startup.
                    File.Delete("dbghelp.dll");
                    File.Delete("dbghelp.ctr");
                    File.Delete("dbghelp.backup");
                }
                catch { }
                if ((RadioRUQuotes.Checked) && (File.Exists("!Contra_Classic_RUVO.ctr")))
                {
                    File.Move("!Contra_Classic_RUVO.ctr", "!Contra_Classic_RUVO.big");
                }
                if ((RadioENQuotes.Checked) && (File.Exists("!Contra_Classic_ENVO.ctr")))
                {
                    File.Move("!Contra_Classic_ENVO.ctr", "!Contra_Classic_ENVO.big");
                }
                if ((RadioEN.Checked) && (File.Exists("!Contra_Classic_EN.ctr")))
                {
                    File.Move("!Contra_Classic_EN.ctr", "!Contra_Classic_EN.big");
                }
                if ((RadioRU.Checked) && (File.Exists("!Contra_Classic_RU.ctr")))
                {
                    File.Move("!Contra_Classic_RU.ctr", "!Contra_Classic_RU.big");
                }
                if ((MNew.Checked) && (File.Exists("!Contra_Classic_NewMusic.ctr")))
                {
                    File.Move("!Contra_Classic_NewMusic.ctr", "!Contra_Classic_NewMusic.big");
                }
                if ((GoofyPics.Checked) && (File.Exists("!!Contra_Classic_FunnyGenPics.ctr")))
                {
                    File.Move("!!Contra_Classic_FunnyGenPics.ctr", "!!Contra_Classic_FunnyGenPics.big");
                }
                else if ((!GoofyPics.Checked) && (File.Exists("!!Contra_Classic_FunnyGenPics.big")))
                {
                    File.Move("!!Contra_Classic_FunnyGenPics.big", "!!Contra_Classic_FunnyGenPics.ctr");
                }
                if ((Properties.Settings.Default.LangF == false) && (File.Exists("langdata.dat")))
                {
                    File.Move("langdata.dat", "langdata1.dat");
                }
                else if ((Properties.Settings.Default.LangF == true) && (File.Exists("langdata1.dat")))
                {
                    File.Move("langdata1.dat", "langdata.dat");
                }
                if (Directory.Exists(@"Data\Scripts"))
                {
                    int scripts = Directory.GetFiles(@"Data\Scripts").Length;
                    if (scripts == 0)
                    {
                        Directory.Delete(@"Data\Scripts");
                    }
                }
                if (Directory.Exists(@"Data\Scripts1"))
                {
                    int scripts1 = Directory.GetFiles(@"Data\Scripts1").Length;
                    if (scripts1 == 0)
                    {
                        Directory.Delete(@"Data\Scripts1");
                    }
                }
                if (Directory.Exists(@"Data\Scripts"))
                {
                    Directory.Move(@"Data\Scripts", @"Data\Scripts1");
                }
                if (File.Exists("Install_Final.bmp") && (File.Exists("Install_Final_Contra.bmp")))
                {
                    try
                    {
                        File.SetAttributes("Install_Final.bmp", FileAttributes.Normal);
                        if (File.Exists("Install_Final_ZH"))
                        {
                            File.SetAttributes("Install_Final_ZH.bmp", FileAttributes.Normal);
                        }
                        File.SetAttributes("Install_Final_Contra.bmp", FileAttributes.Normal);
                        File.Copy("Install_Final.bmp", "Install_Final_ZH.bmp", true);
                        File.Copy("Install_Final_Contra.bmp", "Install_Final.bmp", true);
                    }
                    catch
                    { }
                }

                // Make CTR Options.ini active
                try
                {
                    if (File.Exists(UserDataLeafName() + "Options_CTR.ini"))
                    {
                        File.SetAttributes(UserDataLeafName() + "Options.ini", FileAttributes.Normal);
                        File.SetAttributes(UserDataLeafName() + "Options_CTR.ini", FileAttributes.Normal);
                        File.SetAttributes(UserDataLeafName() + "Options_ZH.ini", FileAttributes.Normal);
                        File.Copy(UserDataLeafName() + "Options.ini", UserDataLeafName() + "Options_ZH.ini", true);
                        File.Copy(UserDataLeafName() + "Options_CTR.ini", UserDataLeafName() + "Options.ini", true);
                    }
                    else if (File.Exists(myDocPath + "Options_CTR.ini"))
                    {
                        File.SetAttributes(myDocPath + "Options.ini", FileAttributes.Normal);
                        File.SetAttributes(myDocPath + "Options_CTR.ini", FileAttributes.Normal);
                        File.SetAttributes(myDocPath + "Options_ZH.ini", FileAttributes.Normal);
                        File.Copy(myDocPath + "Options.ini", myDocPath + "Options_ZH.ini", true);
                        File.Copy(myDocPath + "Options_CTR.ini", myDocPath + "Options.ini", true);
                    }
                }
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
                catch
                { }

                // Disable cyrillic letters, enable German umlauts.
                if (File.Exists("GermanZH.big") && File.Exists("GenArial.ttf"))
                {
                    File.Move("GenArial.ttf", "GenArial_.ttf");
                }

                // Check for generals.ctr
                string message = null;
                string title = null;
                if (!File.Exists("generals.ctr") || CalculateMD5("generals.ctr") != "ee7d5e6c2d7fb66f5c27131f33da5fd3")
                {
                    if (Globals.GB_Checked == true)
                    {
                        message = "\"generals.ctr\" not found or checksum mismatch! Custom camera height preferences might not work and you might not be able to see other players in the game lobby.\n\nWould you like to start the game anyway?";
                        title = "Warning";
                    }
                    else if (Globals.RU_Checked == true)
                    {
                        message = "\"generals.ctr\" не найден или несоответствие контрольной суммы! Пользовательские настройки высоты камеры могут не работать, и вы не сможете видеть других игроков в игровом лобби.\n\nХотели бы вы начать игру?";
                        title = "Предупреждение";
                    }
                    DialogResult dialogResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        StartGenerals();
                    }
                    else
                    { }
                }
                else
                {
                    StartGenerals();
                }
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error");
            //}
            catch
            { }
            return;
        }

        public void StartGenerals()
        {
            // Check for .dll files
            if (!File.Exists("binkw32.dll") || (!File.Exists("mss32.dll")))
            {
                if (Globals.GB_Checked == true)
                {
                    MessageBox.Show("The game cannot start because the \"binkw32.dll\" and/or \"mss32.dll\" file(s) were not found. You may have installed the mod in the wrong folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Globals.RU_Checked == true)
                {
                    MessageBox.Show("Игра не может запуститься, потому что файлы \"binkw32.dll\" и/или \"mss32.dll\" не найдены. Возможно, вы установили мод не в ту папку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            // Rename generals.exes
            if (File.Exists("generals.exe") && (File.Exists("generals.ctr")))
            {
                try
                {
                    File.SetAttributes("generals.exe", FileAttributes.Normal);
                    if (File.Exists("generals_zh.exe"))
                    {
                        File.SetAttributes("generals_zh.exe", FileAttributes.Normal);
                    }
                    File.SetAttributes("generals.ctr", FileAttributes.Normal);
                    File.Copy("generals.exe", "generals_zh.exe", true);
                    File.Copy("generals.ctr", "generals.exe", true);
                }
                catch
                { }
            }

            if (File.Exists("generals.exe"))
            {
                Process generals = new Process();
                generals.StartInfo.FileName = "generals.exe";

                if (Properties.Settings.Default.Windowed == false && Properties.Settings.Default.Quickstart == false)
                {
                    //no start arguments
                }
                else if (Properties.Settings.Default.Quickstart == true && Properties.Settings.Default.Windowed == false)
                {
                    generals.StartInfo.Arguments = "-quickstart -nologo";
                }
                else if (Properties.Settings.Default.Windowed == true && Properties.Settings.Default.Quickstart == true)
                {
                    generals.StartInfo.Arguments = "-win -quickstart -nologo";
                }
                else //if (Properties.Settings.Default.Windowed == true && Properties.Settings.Default.Quickstart == false)
                {
                    generals.StartInfo.Arguments = "-win";
                }

                generals.EnableRaisingEvents = true;
                generals.Exited += (sender1, e1) =>
                {
                    WindowState = FormWindowState.Normal;
                };
                generals.StartInfo.WorkingDirectory = Path.GetDirectoryName("generals.exe");
                WindowState = FormWindowState.Minimized;
                generals.Start();
            }
            else
            {
                CheckInstallDir();
            }
        }

        internal static bool Url_open(string url)
        {
            try
            {
                Process.Start(url);
                return true;
            }
            catch
            {
                try
                {
                    Process.Start("IExplore.exe", url);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not use your default browser to open URL:\n" + url + "\n\n" + ex.Message, "Opening link failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
        }
        private void websitebutton_Click(object sender, EventArgs e)
        {
            Url_open("https://contra.cncguild.net/oldsite/Eng/index.php");
        }

        private void button5_Click(object sender, EventArgs e) //ModDBButton
        {
            Url_open("https://www.moddb.com/mods/contra");
        }

        private void button3_Click(object sender, EventArgs e) //ReadMeButton
        {
            try
            {
                Process.Start("Readme_Contra.txt");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }
            RadioEN.Checked = Properties.Settings.Default.LangEN;
            RadioRU.Checked = Properties.Settings.Default.LangRU;
            MNew.Checked = Properties.Settings.Default.MusicNew;
            MStandard.Checked = Properties.Settings.Default.MusicStandard;
            RadioRUQuotes.Checked = Properties.Settings.Default.VoNew;
            RadioENQuotes.Checked = Properties.Settings.Default.VoStandard;
            RadioRUQuotes.Checked = Properties.Settings.Default.VoRU;
            DefaultPics.Checked = Properties.Settings.Default.GenPicDef;
            GoofyPics.Checked = Properties.Settings.Default.GenPicGoo;
            RadioFlag_GB.Checked = Properties.Settings.Default.Flag_GB;
            RadioFlag_RU.Checked = Properties.Settings.Default.Flag_RU;
            AutoScaleMode = AutoScaleMode.Dpi;
        }

        private void OnApplicationExit(object sender, EventArgs e) //AppExit
        {
            DeleteDuplicateFiles();
            RenameBigToCtr();
            Properties.Settings.Default.LangEN = RadioEN.Checked;
            Properties.Settings.Default.LangRU = RadioRU.Checked;
            Properties.Settings.Default.MusicNew = MNew.Checked;
            Properties.Settings.Default.MusicStandard = MStandard.Checked;
            Properties.Settings.Default.VoNew = RadioRUQuotes.Checked;
            Properties.Settings.Default.VoNew = RadioRUQuotes.Checked;
            Properties.Settings.Default.VoStandard = RadioENQuotes.Checked;
            Properties.Settings.Default.GenPicDef = DefaultPics.Checked;
            Properties.Settings.Default.GenPicGoo = GoofyPics.Checked;
            Properties.Settings.Default.Flag_GB = RadioFlag_GB.Checked;
            Properties.Settings.Default.Flag_RU = RadioFlag_RU.Checked;
            Properties.Settings.Default.Save();

            DelTmpChunk();
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e) //MinimizeIcon
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button18_Click(object sender, EventArgs e) //ExitIcon
        {
            this.Close(); //Application.Exit(); //OnApplicationExit(sender, e);
        }

        private void buttonChat_MouseEnter(object sender, EventArgs e)
        {
            buttonDiscord009.BackgroundImage = Properties.Resources.discord_009_yellow;
            buttonDiscord009.ForeColor = SystemColors.ButtonHighlight;
            buttonDiscord009.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }
        private void buttonChat_MouseLeave(object sender, EventArgs e)
        {
            buttonDiscord009.BackgroundImage = Properties.Resources.discord_009_red;
            buttonDiscord009.ForeColor = SystemColors.ButtonHighlight;
            buttonDiscord009.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void buttonChat_Click(object sender, EventArgs e)
        {
            Url_open("https://discordapp.com/invite/015E6KXXHmdWFXCtt");
        }


        private void buttonDiscordClassic_MouseEnter(object sender, EventArgs e)
        {
            buttonDiscordClassic.BackgroundImage = Properties.Resources.discord_classic_yellow;
            buttonDiscordClassic.ForeColor = SystemColors.ButtonHighlight;
            buttonDiscordClassic.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void buttonDiscordClassic_MouseLeave(object sender, EventArgs e)
        {
            buttonDiscordClassic.BackgroundImage = Properties.Resources.discord_classic_red;
            buttonDiscordClassic.ForeColor = SystemColors.ButtonHighlight;
            buttonDiscordClassic.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void buttonDiscordClassic_Click(object sender, EventArgs e)
        {
            Url_open("https://discordapp.com/invite/666aeVvFXq");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void WinCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void voicespanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GoofyPics_CheckedChanged(object sender, EventArgs e)
        {

        }

        public static void SetFirewallExcemption(string exePath)
        {
            // Full path in rule name is ugly, let's only show filename instead
            string ExeWithoutPath = exePath;
            int idx = exePath.LastIndexOf(@"\");
            if (idx != -1) ExeWithoutPath = exePath.Substring(idx + 1);

            // Check if rule with same name exists
            var netsh = new Process();
            netsh.StartInfo.FileName = "netsh.exe";
            netsh.StartInfo.CreateNoWindow = true;
            netsh.StartInfo.UseShellExecute = false;
            netsh.StartInfo.Arguments = $"advfirewall firewall show rule name=\"Contra - \"{ExeWithoutPath}\"";
            netsh.Start();
            netsh.WaitForExit();

            // Add new firewall excemption rule if missing
            if (netsh.ExitCode != 0)
            {
                netsh.StartInfo.Arguments = $"advfirewall firewall add rule name=\"Contra - {ExeWithoutPath}\" dir=in action=allow program=\"{Environment.CurrentDirectory}\\{exePath}\" protocol=tcp profile=private,public edge=yes enable=yes";
                netsh.Start();

                Process netsh2 = new Process();
                netsh2.StartInfo = netsh.StartInfo;
                netsh2.StartInfo.Arguments = $"advfirewall firewall add rule name=\"Contra - {exePath}\" dir=in action=allow program=\"{Environment.CurrentDirectory}\\{exePath}\" protocol=udp profile=private,public edge=yes enable=yes";
                netsh2.Start();

                netsh.WaitForExit();
                netsh2.WaitForExit();
            }
        }

        public static void CheckFirewallExceptions()
        {
            // All executables which need listening ports open
            ReadOnlyCollection<string> exes = Array.AsReadOnly(new[] {
                "game.dat",
                "generals.exe",
            });

            // Check if all files exist first before attempting to add any rules
            bool allFilesExist = exes.All(file => File.Exists(Environment.CurrentDirectory + @"\" + file));
            if (allFilesExist) foreach (string exe in exes) SetFirewallExcemption(exe);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public string GetCurrentCulture()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string cultureStr = culture.ToString();
            return cultureStr;
        }

        public static class ThreadHelperClass
        {
            delegate void SetTextCallback(Form f, Control ctrl, string text);
            /// <summary>
            /// Set text property of various controls
            /// </summary>
            /// <param name="form">The calling form</param>
            /// <param name="ctrl"></param>
            /// <param name="text"></param>
            public static void SetText(Form form, Control ctrl, string text)
            {
                // InvokeRequired required compares the thread ID of the 
                // calling thread to the thread ID of the creating thread. 
                // If these threads are different, it returns true. 
                if (ctrl.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    form.Invoke(d, new object[] { form, ctrl, text });
                }
                else
                {
                    ctrl.Text = text;
                }
            }
        }

        bool downloadTextFile = false;
        bool seekForUpdate = true;

        // This method is executed on the worker thread and makes 
        // a thread-safe call on the TextBox control. 
        private void ThreadProcSafeMOTD(string versionsTXT)
        {
            try
            {
                {
                    if (downloadTextFile == false)
                    {
                        //Check for launcher update once per launch.
                        if (seekForUpdate == true)
                        {
                            seekForUpdate = false;
                            //GetLauncherUpdate(versionsTXT, launcher_url);
                            //GetModUpdate(versionsTXT, patch_url);
                        }
                        downloadTextFile = true;
                    }
                    void SetMOTD(string prefix)
                    {
                        string MOTDText = versionsTXT.Substring(versionsTXT.LastIndexOf(prefix) + 9);
                        string MOTDText2 = MOTDText.Substring(0, MOTDText.IndexOf("$"));
                        ThreadHelperClass.SetText(this, MOTD, MOTDText2);
                    }

                    var versionsTXT_lang = new Dictionary<string, bool>
                    {
                        {"MOTD-EN: ", Globals.GB_Checked},
                        {"MOTD-RU: ", Globals.RU_Checked},
                    };
                    SetMOTD(versionsTXT_lang.Single(l => l.Value).Key);
                }
            }
            catch { }
        }

        void gtwc_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Extract zip
            string zipPath = launcherExecutingPath + @"\" + genToolFileName;

            try //To prevent crash
            {
                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries.Where(a => a.FullName.Contains("d3d8.dll")))
                    {
                        entry.ExtractToFile(Path.Combine(launcherExecutingPath, entry.FullName), true);
                    }
                }
            }
            catch { }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            try
            {
                File.Delete(genToolFileName);
            }
            catch { }

            //Show a message when the patch download has completed
            var gtLangText = new Dictionary<Tuple<string, string>, bool>
                {
                    { Tuple.Create("A new version of Gentool has been downloaded!", "Gentool update Complete"), Globals.GB_Checked},
                    { Tuple.Create("Новая версия GenTool был загружен!", "Gentool обновление завершено"), Globals.RU_Checked},
                }.Single(l => l.Value).Key;
            MessageBox.Show(new Form { TopMost = true }, gtLangText.Item1, gtLangText.Item2, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DownloadGentool(string url)
        {
            try
            {
                WebClient gtwc = new WebClient();
                gtwc.DownloadFileCompleted += new AsyncCompletedEventHandler(gtwc_DownloadCompleted);

                //CheckIfFileIsAvailable(url);
                //gtwc.OpenRead(url + genToolFileName);
                //bytes_total = Convert.ToInt64(gtwc.ResponseHeaders["Content-Length"]);

                gtwc.DownloadFileAsync(new Uri(url), launcherExecutingPath + @"\" + genToolFileName);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        public static Tuple<int, int> getScreenResolution() => Tuple.Create(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        readonly int ScreenResolutionX = getScreenResolution().Item1;
        readonly int ScreenResolutionY = getScreenResolution().Item2;

        public void CreateOptionsINI()
        {
            try
            {
                using (FileStream fs = File.Create(UserDataLeafName() + @"\Options.ini"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(
                        "IdealStaticGameLOD = High" +
                        Environment.NewLine +
                        "Resolution = " + ScreenResolutionX + " " + ScreenResolutionY +
                        Environment.NewLine +
                        "BuildingOcclusion = Yes" +
                        Environment.NewLine +
                        "DynamicLOD = Yes" +
                        Environment.NewLine +
                        "ExtraAnimations = Yes" +
                        Environment.NewLine +
                        "HeatEffects = No" +
                        Environment.NewLine +
                        "ShowSoftWaterEdge = Yes" +
                        Environment.NewLine +
                        "ShowTrees = Yes" +
                        Environment.NewLine +
                        "StaticGameLOD = Custom" +
                        Environment.NewLine +
                        "UseCloudMap = Yes" +
                        Environment.NewLine +
                        "UseLightMap = Yes" +
                        Environment.NewLine +
                        "UseShadowDecals = Yes" +
                        Environment.NewLine +
                        "UseShadowVolumes = No");
                    fs.Write(info, 0, info.Length);
                }
                using (FileStream fs = File.Create(myDocPath + @"\Options.ini"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(
                        "IdealStaticGameLOD = High" +
                        Environment.NewLine +
                        "Resolution = " + ScreenResolutionX + " " + ScreenResolutionY +
                        Environment.NewLine +
                        "BuildingOcclusion = Yes" +
                        Environment.NewLine +
                        "DynamicLOD = Yes" +
                        Environment.NewLine +
                        "ExtraAnimations = Yes" +
                        Environment.NewLine +
                        "HeatEffects = No" +
                        Environment.NewLine +
                        "ShowSoftWaterEdge = Yes" +
                        Environment.NewLine +
                        "ShowTrees = Yes" +
                        Environment.NewLine +
                        "StaticGameLOD = Custom" +
                        Environment.NewLine +
                        "UseCloudMap = Yes" +
                        Environment.NewLine +
                        "UseLightMap = Yes" +
                        Environment.NewLine +
                        "UseShadowDecals = Yes" +
                        Environment.NewLine +
                        "UseShadowVolumes = No");
                    fs.Write(info, 0, info.Length);
                }
            }
            catch { }
        }

        static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public string AspectRatio(int x, int y)
        {
            double value = (double)x / y;
            if (value > 1.7)
                return "16:9";
            else
                return "4:3";
        }

        private void ChangeCamHeight()
        {
            if (File.Exists("!Contra_Classic.big") || File.Exists("!Contra_Classic.ctr"))
            {
                if (File.Exists("!Contra_Classic_GameData.big"))
                {
                    Encoding encoding = Encoding.GetEncoding("windows-1252");
                    var regex = Regex.Replace(File.ReadAllText("!Contra_Classic_GameData.big", encoding), "  MaxCameraHeight = .*\r?\n", "  MaxCameraHeight = 240.0 ;350.0\r\n");
                    File.WriteAllText("!Contra_Classic_GameData.big", regex, encoding);
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

        private void Form1_Shown(object sender, EventArgs e)
        {
            string gtHash = null;
            try
            {
                gtHash = CalculateMD5("d3d8.dll");
            }
            catch { }

            if (isGentoolInstalled("d3d8.dll") && isGentoolOutdated("d3d8.dll", 79) || (gtHash == "70c28745f6e9a9a59cfa1be00df6836a" || gtHash == "13a13584d97922de92443631931d46c3"))
            {
                //try
                //{
                //    {
                //        System.Threading.Thread demoThread =
                //           new System.Threading.Thread(new System.Threading.ThreadStart(ThreadProcSafeGentool));
                //        demoThread.Start();
                //    }
                //}
                //catch (Exception ex) { MessageBox.Show(ex.ToString()); }

                try
                {
                    using (WebClient client = new WebClient())
                    {
                        genToolFileName = client.DownloadString("http://www.gentool.net/download/patch");
                        genToolFileName = genToolFileName.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[1];

                        //MessageBox.Show(genToolFileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }

                string gtURL = "http://www.gentool.net/download/" + genToolFileName;
                DownloadGentool(gtURL);
            }

            // Cleanup old Launcher file after update
            if (File.Exists(launcherExecutingPath + @"\Contra007Classic_Launcher_ToDelete.exe"))
            {
                File.SetAttributes("Contra007Classic_Launcher_ToDelete.exe", FileAttributes.Normal);
                File.Delete(launcherExecutingPath + @"\Contra007Classic_Launcher_ToDelete.exe");
            }

            // Generate Options.ini if missing.
            if (!File.Exists(UserDataLeafName() + "Options.ini") && (!File.Exists(myDocPath + "Options.ini")))
            {
                //if (Globals.GB_Checked == true)
                //{
                //    MessageBox.Show("Options.ini not found, therefore the game will not start! You have to run Zero Hour once for it to generate the file.\nIf that fails, you will have to create the file manually. Click the \"Help\" button in launcher to be brought to a page with instructions.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                //else if (Globals.RU_Checked == true)
                //{
                //    MessageBox.Show("Файл \"Options.ini\" не найден, поэтому игра не запустится! Вам нужно запустить Zero Hour один раз, чтобы он сгенерировал файл.\nЕсли не получится, вам придется создать файл вручную. Нажмите кнопку «Help» в панели лаунчера, чтобы перейти на страницу с инструкциями.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                //else if (Globals.UA_Checked == true)
                //{
                //    MessageBox.Show("Файл Options.ini не знайдений, отже гра не розпочнеться! Вам потрібно запустити Zero Hour один раз, щоб створити файл.\nЯкщо це не вдасться, вам доведеться створити файл вручну. Натисніть кнопку \"Help\" в панелі запуску, щоб перейти на сторінку з інструкціями.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                //else if (Globals.BG_Checked == true)
                //{
                //    MessageBox.Show("Options.ini не беше намерен, следователно играта няма да се стартира! Трябва да стартирате Zero Hour веднъж, за да генерира файла.\nАко това се провали, трябва да създадете файла ръчно. Щракнете на \"Help\" бутона в launcher-а, за да отидете на страница с инструкции.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                //else if (Globals.DE_Checked == true)
                //{
                //    MessageBox.Show("Options.ini nicht gefunden, daher startet das Spiel nicht! Sie müssen Zero Hour einmal ausführen, damit die Datei generiert wird.\nWenn dies fehlschlägt, müssen Sie die Datei manuell erstellen. Klicken Sie im Launcher auf die Schaltfläche \"Help\", um zu einer Seite mit Anweisungen zu gelangen.", "Warnung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                CreateOptionsINI();
            }

            // Make a 2 copies of Options.ini, name them Options_ZH.ini and Options_CTR.ini
            if (!File.Exists(UserDataLeafName() + "Options_ZH.ini") || (!File.Exists(myDocPath + "Options_ZH.ini")) && !File.Exists(UserDataLeafName() + "Options_CTR.ini") || (!File.Exists(myDocPath + "Options_CTR.ini")))
            {
                if (File.Exists(UserDataLeafName() + "Options.ini"))
                {
                    File.SetAttributes(UserDataLeafName() + "Options.ini", FileAttributes.Normal);
                    File.Copy(UserDataLeafName() + "Options.ini", UserDataLeafName() + "Options_ZH.ini", true);
                    File.Copy(UserDataLeafName() + "Options.ini", UserDataLeafName() + "Options_CTR.ini", true);
                }
                else if (File.Exists(myDocPath + "Options.ini"))
                {
                    File.SetAttributes(myDocPath + "Options.ini", FileAttributes.Normal);
                    File.Copy(myDocPath + "Options.ini", myDocPath + "Options_ZH.ini", true);
                    File.Copy(myDocPath + "Options.ini", myDocPath + "Options_CTR.ini", true);
                }
            }

            // Make CTR Options.ini active
            try
            {
                if (File.Exists(UserDataLeafName() + "Options_CTR.ini"))
                {
                    File.SetAttributes(UserDataLeafName() + "Options.ini", FileAttributes.Normal);
                    File.SetAttributes(UserDataLeafName() + "Options_CTR.ini", FileAttributes.Normal);
                    File.SetAttributes(UserDataLeafName() + "Options_ZH.ini", FileAttributes.Normal);
                    File.Copy(UserDataLeafName() + "Options.ini", UserDataLeafName() + "Options_ZH.ini", true);
                    File.Copy(UserDataLeafName() + "Options_CTR.ini", UserDataLeafName() + "Options.ini", true);
                }
                else if (File.Exists(myDocPath + "Options_CTR.ini"))
                {
                    File.SetAttributes(myDocPath + "Options.ini", FileAttributes.Normal);
                    File.SetAttributes(myDocPath + "Options_CTR.ini", FileAttributes.Normal);
                    File.SetAttributes(myDocPath + "Options_ZH.ini", FileAttributes.Normal);
                    File.Copy(myDocPath + "Options.ini", myDocPath + "Options_ZH.ini", true);
                    File.Copy(myDocPath + "Options_CTR.ini", myDocPath + "Options.ini", true);
                }
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            catch
            { }

            if (Properties.Settings.Default.FirstRun)
            {
                try
                {
                    // Remove dbghelp to fix DirectX error on game startup.
                    File.Delete("dbghelp.dll");
                    File.Delete("dbghelp.ctr");
                    File.Delete("dbghelp.backup");
                }
                catch
                { }

                // Enable GameData
                if (File.Exists("!Contra_Classic_GameData.ctr"))
                {
                    File.Move("!Contra_Classic_GameData.ctr", "!Contra_Classic_GameData.big");
                }
                // Set default cam height
                //try
                //{
                //    if (AspectRatio(ScreenResolutionX, ScreenResolutionY) == "16:9" && isGentoolInstalled("d3d8.dll"))
                //    {
                //        ChangeCamHeight();
                //    }
                //}
                //catch { }

                // If there are older Contra config folders, this means Contra Launcher has been
                // ran before on this PC, so in this case, we skip first run welcome message.
                int directoryCount = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Contra").Length;

                // Zero Hour has a 'DeleteFile("Data\INI\INIZH.big");' line in GameEngine::init with no condition whatsoever (will always try to delete it if exists)
                // an identical copy of this file exists in root ZH folder so we can safely delete it before ZH runs to prevent unwanted crashes
                try
                {
                    File.Delete(@"Data\INI\INIZH.big");
                }
                catch { }

                // Get CPU specs to determine default graphical settings
                ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_Processor");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
                ManagementObjectCollection results = searcher.Get();
                foreach (ManagementObject result in results)
                {
                    Globals.cpuSpeed = Convert.ToInt32(result["MaxClockSpeed"]);

                    //Properties.Settings.Default.HeatEffects = false; // Disabled by default to prevent black screen issue of some users.
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

                    if (Globals.cpuSpeed < 3500) // We consider base clock less than 3500MHz to be insufficient for stable FPS.
                    {
                        //    Properties.Settings.Default.Shadows3D = false;
                        //    Properties.Settings.Default.DisableDynamicLOD = false;

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

                    //Console.WriteLine("NumberOfCores : {0}", result["NumberOfCores"]);
                    //Console.WriteLine("NumberOfLogicalProcessors : {0}", result["NumberOfLogicalProcessors"]);
                    //Console.WriteLine("CurrentClockSpeed : {0}MHz", result["CurrentClockSpeed"]);
                    //Console.WriteLine("MaxClockSpeed : {0}MHz", result["MaxClockSpeed"]);
                    //Console.WriteLine("ExtClock : {0}MHz", result["ExtClock"]);
                }

                // Show message on first run.
                if (GetCurrentCulture() == "en-US")
                {
                    RadioFlag_GB.Checked = true;
                    if (directoryCount <= 2)
                    {
                        MessageBox.Show("Welcome to Contra 007 Classic! We highly recommend you to join our Discord communities!");
                    }
                }
                else if (GetCurrentCulture() == "ru-RU")
                {
                    RadioFlag_RU.Checked = true;
                    if (directoryCount <= 2)
                    {
                        MessageBox.Show("Добро пожаловать в Contra 007 Classic! Мы настоятельно рекомендуем Вам присоедениться к нашим Discord сообществам!");
                        RadioRU.Checked = true;
                    }
                }
                else
                {
                    RadioFlag_GB.Checked = true;
                    if (directoryCount <= 1)
                    {
                        MessageBox.Show("Welcome to Contra 007 Classic! We highly recommend you to join our Discord communities!");
                    }
                }

                // Delete old Contra config folders
                DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Contra");

                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    if (dir.Name.Contains("vpnconfig") == true || dir.Name.Contains("Contra_Launcher.exe_Url") == true) // do not delete these folders
                    {
                        continue;
                    }
                    dir.Delete(true);
                }
                try
                {
                    // Enable Tournament Mode (limit super weapons and super units) on first run.
                    if (Directory.Exists(UserDataLeafName()))
                    {
                        string text = File.ReadAllText(UserDataLeafName() + "Skirmish.ini");
                        {
                            if (text.Contains("SuperweaponRestrict = No"))
                            {
                                File.WriteAllText(UserDataLeafName() + "Skirmish.ini", File.ReadAllText(UserDataLeafName() + "Skirmish.ini").Replace("SuperweaponRestrict = No", "SuperweaponRestrict = Yes"));
                            }
                            else if (text.Contains("SuperweaponRestrict = no"))
                            {
                                File.WriteAllText(UserDataLeafName() + "Skirmish.ini", File.ReadAllText(UserDataLeafName() + "Skirmish.ini").Replace("SuperweaponRestrict = no", "SuperweaponRestrict = Yes"));
                            }
                        }
                    }
                    else if (Directory.Exists(myDocPath))
                    {
                        string text = File.ReadAllText(myDocPath + "Skirmish.ini");
                        {
                            if (text.Contains("SuperweaponRestrict = No"))
                            {
                                File.WriteAllText(myDocPath + "Skirmish.ini", File.ReadAllText(myDocPath + "Skirmish.ini").Replace("SuperweaponRestrict = No", "SuperweaponRestrict = Yes"));
                            }
                            else if (text.Contains("SuperweaponRestrict = no"))
                            {
                                File.WriteAllText(myDocPath + "Skirmish.ini", File.ReadAllText(myDocPath + "Skirmish.ini").Replace("SuperweaponRestrict = no", "SuperweaponRestrict = Yes"));
                            }
                        }
                    }
                }
                catch //(Exception ex)
                {
                    //
                }

                // Add Firewall exceptions.
                CheckFirewallExceptions();

                Properties.Settings.Default.FirstRun = false;
                Properties.Settings.Default.Save();
            }

            // Show warning if the base mod isn't found.
            if (!File.Exists("!Contra_Classic.ctr") && !File.Exists("!Contra_Classic.big") && Application.StartupPath.Contains(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
            {
                if (Globals.GB_Checked == true)
                {
                    MessageBox.Show("Contra Launcher could not find Contra installed on your desktop. Locate the game folder where you installed Contra and make a shortcut (not a copy) of Contra Launcher instead.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (Globals.RU_Checked == true)
                {
                    MessageBox.Show("Contra Launcher не может найти установленная Contra на вашем рабочем столе. Найдите папку с игрой, в которую вы установили Contra, и вместо этого сделайте ярлык (не копию) Contra Launcher.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (!File.Exists("!Contra_Classic.ctr") && !File.Exists("!Contra_Classic.big"))
            {
                if (Globals.GB_Checked == true)
                {
                    MessageBox.Show("File \"!Contra_Classic.ctr\" is missing!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (Globals.RU_Checked == true)
                {
                    MessageBox.Show("Файл \"!Contra_Classic.ctr\" отсутствует!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Show warning if there are .ini files in Data\INI or its subfolders.
            try
            {
                if (Directory.GetFiles(Environment.CurrentDirectory + @"\Data\INI", "*.ini", SearchOption.AllDirectories).Length == 0)
                {
                    // no .ini files
                }
                else
                {
                    if (Globals.GB_Checked == true)
                    {
                        MessageBox.Show("Found .ini files in the Data\\INI directory. They may corrupt the mod or cause mismatch online.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (Globals.RU_Checked == true)
                    {
                        MessageBox.Show("Найдены файлы .ini в каталоге Data\\INI. Они могут повредить мод или вызвать несоответствие в сети.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch { }

            // Show warning if there are .wnd files in Window or its subfolders.
            try
            {
                if (Directory.GetFiles(Environment.CurrentDirectory + @"\Window", "*.wnd", SearchOption.AllDirectories).Length == 0)
                {
                    // no .wnd files
                }
                else
                {
                    if (Globals.GB_Checked == true)
                    {
                        MessageBox.Show("Found .wnd files in the Window directory. They may corrupt the mod.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (Globals.RU_Checked == true)
                    {
                        MessageBox.Show("Найдены файлы .wnd в каталоге Window. Они могут повредить мод.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch { }
        }

        private void applyResources(ComponentResourceManager resources, Control.ControlCollection ctls)
        {
            foreach (Control ctl in ctls)
            {
                resources.ApplyResources(ctl, ctl.Name);
                applyResources(resources, ctl.Controls);
            }
        }

        private void Resolution_Click(object sender, EventArgs e) //Opens More Options form
        {
            //Delete duplicate GameData if such exists
            if (File.Exists("!Contra_Classic_GameData.ctr") && File.Exists("!Contra_Classic_GameData.big"))
            {
                File.Delete("!Contra_Classic_GameData.big");
            }
            //Enable GameData so that we can show current camera height in Options
            if (File.Exists("!Contra_Classic_GameData.ctr"))
            {
                File.Move("!Contra_Classic_GameData.ctr", "!Contra_Classic_GameData.big");
            }

            if (File.Exists(UserDataLeafName() + "Options.ini") || (File.Exists(myDocPath + "Options.ini")))
            {
                foreach (Form moreOptionsForm in Application.OpenForms)
                {
                    if (moreOptionsForm is moreOptionsForm)
                    {
                        moreOptionsForm.Close();
                        new moreOptionsForm().Show();
                        return;
                    }
                }
                new moreOptionsForm().Show();
            }
            else
            {
                if (Globals.GB_Checked == true)
                {
                    MessageBox.Show("Options.ini not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Globals.RU_Checked == true)
                {
                    MessageBox.Show("Файл \"Options.ini\" не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Resolution_MouseEnter(object sender, EventArgs e)
        {
            moreOptions.ForeColor = Color.FromArgb(255, 210, 100);
        }

        private void Resolution_MouseDown(object sender, MouseEventArgs e)
        {
            moreOptions.ForeColor = Color.FromArgb(255, 230, 160);
        }

        private void Resolution_MouseLeave(object sender, EventArgs e)
        {
            moreOptions.ForeColor = Color.FromArgb(255, 255, 255);
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

        private void RadioFlag_GB_MouseEnter(object sender, EventArgs e)
        {
            RadioFlag_GB.BackgroundImage = Properties.Resources.flag_gb_tr;
        }
        private void RadioFlag_GB_MouseLeave(object sender, EventArgs e)
        {
            RadioFlag_GB.BackgroundImage = Properties.Resources.flag_gb;
        }

        private void RadioFlag_RU_MouseEnter(object sender, EventArgs e)
        {
            RadioFlag_RU.BackgroundImage = Properties.Resources.flag_ru_tr;
        }
        private void RadioFlag_RU_MouseLeave(object sender, EventArgs e)
        {
            RadioFlag_RU.BackgroundImage = Properties.Resources.flag_ru;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //If updating has failed, clear the 0KB file
            if (File.Exists($"{launcherExecutingPath}\\Contra007Classic_Launcher_{newVersion}.exe") && (applyNewLauncher == false))
            {
                File.Delete($"{launcherExecutingPath}\\Contra007Classic_Launcher_{newVersion}.exe");
            }
            //This renames the original file so any shortcut works and names it accordingly after the update
            if (File.Exists($"{launcherExecutingPath}\\Contra007Classic_Launcher_{newVersion}.exe") && (applyNewLauncher == true))
            {
                File.Move($"{launcherExecutingPath}\\Contra007Classic_Launcher.exe", $"{launcherExecutingPath}\\Contra007Classic_Launcher_ToDelete.exe");
                File.Move($"{launcherExecutingPath}\\Contra007Classic_Launcher_{newVersion}.exe", $"{launcherExecutingPath}\\Contra007Classic_Launcher.exe");
                //Process.Start(Path.Combine(launcherExecutingPath, "Contra007Classic_Launcher.exe"));
            }
        }

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

        public static bool isGentoolOutdated(string gentoolPath, int minVersion)
        {
            try
            {
                var size = GetFileVersionInfoSize(gentoolPath, out _);
                if (size == 0) { throw new Win32Exception(); };
                var bytes = new byte[size];
                bool success = GetFileVersionInfo(gentoolPath, 0, size, bytes);
                if (!success) { throw new Win32Exception(); }

                // 040904E4 US English + CP_USASCII
                VerQueryValue(bytes, @"\StringFileInfo\040904E4\ProductVersion", out IntPtr ptr, out _);
                return int.Parse(Marshal.PtrToStringUni(ptr)) < minVersion;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void moreOptions_MouseEnter(object sender, EventArgs e)
        {
            if (RadioFlag_GB.Checked == true)
            {
                moreOptions.BackgroundImage = Properties.Resources.settings_button_yellow;
            }
            else
            {
                moreOptions.BackgroundImage = Properties.Resources.settings_button_yellow_rus;
            }
            moreOptions.ForeColor = SystemColors.ButtonHighlight;
            moreOptions.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void moreOptions_MouseLeave(object sender, EventArgs e)
        {
            if (RadioFlag_GB.Checked == true)
            {
                moreOptions.BackgroundImage = Properties.Resources.settings_button_red;
            }
            else
            {
                moreOptions.BackgroundImage = Properties.Resources.settings_button_red_rus;
            }
            moreOptions.ForeColor = SystemColors.ButtonHighlight;
            moreOptions.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            //Delete duplicate GameData if such exists
            if (File.Exists("!Contra_Classic_GameData.ctr") && File.Exists("!Contra_Classic_GameData.big"))
            {
                File.Delete("!Contra_Classic_GameData.big");
            }
            //Enable GameData so that we can show current camera height in Options
            if (File.Exists("!Contra_Classic_GameData.ctr"))
            {
                File.Move("!Contra_Classic_GameData.ctr", "!Contra_Classic_GameData.big");
            }

            if (File.Exists(UserDataLeafName() + "Options.ini") || (File.Exists(myDocPath + "Options.ini")))
            {
                foreach (Form moreOptionsForm in Application.OpenForms)
                {
                    if (moreOptionsForm is moreOptionsForm)
                    {
                        moreOptionsForm.Close();
                        new moreOptionsForm().Show();
                        return;
                    }
                }
                new moreOptionsForm().Show();
            }
            else
            {
                if (Globals.GB_Checked == true)
                {
                    MessageBox.Show("Options.ini not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Globals.RU_Checked == true)
                {
                    MessageBox.Show("Файл \"Options.ini\" не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CancelModDLBtn_Click(object sender, EventArgs e)
        {
            httpCancellationToken.Cancel();
            //PatchDLPanel.Hide();
            //wcMod.CancelAsync();
        }
    }
}
