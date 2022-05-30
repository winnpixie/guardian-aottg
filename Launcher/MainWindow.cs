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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs args)
        {
            LogOutputArea.AutoWordSelection = true;

            InformationLbl.Text = string.Format(InformationLbl.Text, Environment.OSVersion.VersionString, Program.Arch, Program.Build);
            InformationLbl.Refresh();
        }

        private async void UpdateAndPlayBtn_Start(object sender, EventArgs e)
        {
            // Terminate any active processes related to our software
            try
            {
                LogOutputArea.Text = "Attempting to kill active Guardian processes...";

                Process.Start(new ProcessStartInfo("taskkill.exe", "/F /IM Guardian.exe"));
            }
            catch { }

            byte[] binZipData = new byte[0];

            using (HttpClient httpClient = new HttpClient())
            {
                // HTTP GET and print the latest build information
                try
                {
                    LogOutputArea.Text += "\nObtaining latest build information...";

                    string latestVersion = await httpClient.GetStringAsync($"{Program.VersionsURL}?t=" + Environment.TickCount);
                    LogOutputArea.Text += $"\n{latestVersion}";
                }
                catch (Exception ex)
                {
                    LogOutputArea.Text = $"ERROR, SKIPPING\n\n{ex}\n";
                }

                // HTTP GET the binaries
                try
                {
                    LogOutputArea.Text += $"\nDownloading binaries for {Program.Arch}-bit Windows...";

                    binZipData = await httpClient.GetByteArrayAsync($"{Program.GameDataURL}?t=" + Environment.TickCount);
                }
                catch (Exception ex)
                {
                    LogOutputArea.Text += $"FAILED\n\n{ex}";

                    return;
                }
            }

            FileInfo gameZip = new FileInfo(Program.Cwd + $"\\{Program.BinaryName}");

            // Delete previous ZIP to try and minimize unintended behaviour
            if (gameZip.Exists)
            {
                try
                {
                    LogOutputArea.Text += $"\nDeleting previous ZIP...";

                    gameZip.Delete();
                }
                catch (Exception ex)
                {
                    LogOutputArea.Text += $"FAILED\nPlease delete it yourself and retry.\n\n{ex}";
                    return;
                }
            }

            // Write ZIP data to local file
            try
            {
                LogOutputArea.Text += $"\nWriting ZIP data to {gameZip.FullName}...";

                using (FileStream fs = gameZip.OpenWrite())
                {
                    await fs.WriteAsync(binZipData, 0, binZipData.Length);
                }
            }
            catch (Exception ex)
            {
                LogOutputArea.Text += $"FAILED\n\n{ex}";

                return;
            }

            // Extract ZIP
            try
            {
                LogOutputArea.Text += $"\nExtracting {gameZip.FullName} to current directory ({Program.Cwd})...";

                using (ZipArchive archive = ZipFile.OpenRead(gameZip.FullName))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        LogOutputArea.Text += $"\nExtracting {entry.FullName}...";
                        string path = Program.Cwd + "\\" + entry.FullName;
                        if (path.EndsWith("\\") || path.EndsWith("/"))
                        {
                            DirectoryInfo di = new DirectoryInfo(path.Substring(0, path.Length - 1));
                            di.Create();
                        }
                        else
                        {
                            entry.ExtractToFile(path, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogOutputArea.Text += $"FAIL\n\n{ex}";
            }

            StartGameBtn.PerformClick();
        }

        private void StartGameBtn_Click(object sender, EventArgs e)
        {
            try
            {
                LogOutputArea.Text += "Starting Game...";

                Process.Start("Guardian.exe");
            }
            catch (Exception ex)
            {
                LogOutputArea.Text += $"FAILED\n\n{ex}";
            }
        }

        private async void UploadLogBtn_Click(object sender, EventArgs e)
        {
            FileInfo outputLogFile = new FileInfo(Program.Cwd + "\\Guardian_Data\\output_log.txt");
            if (!outputLogFile.Exists)
            {
                LogOutputArea.Text += "\nNo 'output_log.txt' in Guardian_Data";

                return;
            }

            // Read output_log.txt, upload it to Hastebin, and then print out the generated Hastebin URL
            try
            {
                using (StreamReader sr = new StreamReader(outputLogFile.OpenRead()))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string content = await sr.ReadToEndAsync();
                        using (StringContent payload = new StringContent(content, Encoding.UTF8, "text/plain"))
                        {
                            LogOutputArea.Text = "Uploading Guardian_Data\\output_log.txt to hastebin.com...";

                            HttpResponseMessage response = await client.PostAsync("https://www.toptal.com/developers/hastebin/documents", payload);
                            if (!response.IsSuccessStatusCode)
                            {
                                throw new Exception("Bad response from https://www.toptal.com/developers/hastebin/");
                            }

                            string data = await response.Content.ReadAsStringAsync();
                            using (JsonDocument jd = JsonDocument.Parse(data))
                            {
                                LogOutputArea.Text += "\n\nhttps://www.toptal.com/developers/hastebin/" + jd.RootElement.GetProperty("key").GetString() + ".txt";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogOutputArea.Text = $"FAILED\n\n{ex}";
            }
        }
    }
}
