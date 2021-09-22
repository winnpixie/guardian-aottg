using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Launcher
{
    public partial class MainWindow : Form
    {
        private string _currentDirectory;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs args)
        {
            _currentDirectory = Environment.CurrentDirectory;
        }

        private async void updateAndStart_Click(object sender, EventArgs e)
        {
            try
            {
                outputLog.Text = "Attempting to terminate active Guardian tasks...";

                Process.Start(new ProcessStartInfo("taskkill.exe", "/F /IM Guardian.exe"));
            }
            catch { }

            try
            {
                outputLog.Text += "\nBeginning download of Guardian.zip";

                FileInfo updateFileRef = new FileInfo(_currentDirectory + "\\Guardian.zip");
                if (updateFileRef.Exists)
                {
                    updateFileRef.Delete();
                }

                using (HttpClient hc = new HttpClient())
                {
                    string latestVersion = await hc.GetStringAsync("https://aottg.tk/mods/guardian/version.txt?t=" + Environment.TickCount);
                    outputLog.Text += $"\nLatest Version : {latestVersion}";

                    outputLog.Text += "\nDownloading Guardian.zip from https://alerithe.github.io/guardian/Guardian.zip";
                    using (FileStream fs = updateFileRef.OpenWrite())
                    {
                        byte[] data = await hc.GetByteArrayAsync("https://alerithe.github.io/guardian/Guardian.zip?t=" + Environment.TickCount);
                        fs.Write(data, 0, data.Length);
                    }

                    outputLog.Text += "\nDownload successful!";
                }

                using (ZipArchive archive = ZipFile.OpenRead(_currentDirectory + "\\Guardian.zip"))
                {
                    outputLog.Text += $"\nExtracting Guardian.zip to current directory ({_currentDirectory})";

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        outputLog.Text += $"\nExtracting {entry.FullName}...";

                        string realPath = _currentDirectory + "\\" + entry.FullName;
                        if (realPath.EndsWith("\\") || realPath.EndsWith("/"))
                        {
                            DirectoryInfo di = new DirectoryInfo(realPath.Substring(0, realPath.Length - 1));
                            if (!di.Exists)
                            {
                                di.Create();
                            }
                        }
                        else
                        {
                            entry.ExtractToFile(realPath, true);
                        }

                        outputLog.Text += $"OK!";
                    }

                    outputLog.Text += "\nExtraction completed!";
                }

                updateFileRef.Delete();

                startNoUpdate.PerformClick();
            }
            catch (Exception ex)
            {
                outputLog.Text += $"\n\n{ex}";

                outputLog.Text += "\n\nIf errors persist, please contact me on Discord!";
                outputLog.Text += "\n\nhttps://cb.run/FFT";
            }
        }

        private void startNoUpdate_Click(object sender, EventArgs e)
        {
            outputLog.Text += "\nLaunching Guardian.exe...";

            try
            {
                Process.Start(new ProcessStartInfo("Guardian.exe"));
                this.Close();
            }
            catch (Exception ex)
            {
                outputLog.Text += "\nCould not launch application 'Guardian.exe'!";
                outputLog.Text += $"\n\n{ex}";
            }
        }

        private async void uploadLog_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo outputLogFileRef = new FileInfo(_currentDirectory + "\\Guardian_Data\\output_log.txt");
                if (outputLogFileRef.Exists)
                {
                    using (FileStream fs = outputLogFileRef.OpenRead())
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            string content = await sr.ReadToEndAsync();

                            using (StringContent sc = new StringContent(content, Encoding.UTF8, "application/json"))
                            {
                                using (HttpClient hc = new HttpClient())
                                {
                                    HttpResponseMessage response = await hc.PostAsync("https://hastebin.com/documents", sc);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        string resData = await response.Content.ReadAsStringAsync();

                                        using (JsonDocument jd = JsonDocument.Parse(resData))
                                        {
                                            outputLog.Text += "Log contents uploaded!\n\n";
                                            outputLog.Text += "https://hastebin.com/" + jd.RootElement.GetProperty("key").GetString() + ".txt";
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Bad response from https://hastebin.com/");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("No 'output_log.txt' found in 'Guardian_Data'!");
                }
            }
            catch (Exception ex)
            {
                outputLog.Text += $"\n\n{ex}";
            }
        }
    }
}
