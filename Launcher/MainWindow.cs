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
            base.Text += $" ({Program.Build})";

            outputLogArea.AutoWordSelection = true;
        }

        private async void updateAndPlayBtn_Start(object sender, EventArgs e)
        {
            try
            {
                outputLogArea.Text = "Attempting to terminate active Guardian tasks...";
                Process.Start(new ProcessStartInfo("taskkill.exe", "/F /IM Guardian.exe"));
            }
            catch { }

            string fileName = $"Guardian{Program.Architecture}.zip";

            byte[] updateZipData = new byte[0];
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    outputLogArea.Text += "\nObtaining latest build information...";
                    string latestVersion = await client.GetStringAsync("http://www.aottg.tk/mods/guardian/version.txt?t=" + Environment.TickCount);
                    outputLogArea.Text += $"{latestVersion}\n";
                }
                catch (Exception ex)
                {
                    outputLogArea.Text = $"???\n\n{ex}\n";
                }

                try
                {
                    outputLogArea.Text += $"\nDownloading {fileName}...";
                    updateZipData = await client.GetByteArrayAsync($"https://tivuhh.github.io/guardian/{fileName}?t=" + Environment.TickCount);
                    outputLogArea.Text += "OK\n";
                }
                catch (Exception ex)
                {
                    outputLogArea.Text += $"FAILED\n\n{ex}";
                    return;
                }
            }

            FileInfo updateZipFile = new FileInfo(Program.RunDirectory + $"\\{fileName}");
            if (updateZipFile.Exists)
            {
                try
                {
                    outputLogArea.Text += "Deleting pre-existing update ZIP file...";
                    updateZipFile.Delete();
                    outputLogArea.Text += "OK\n";
                }
                catch (Exception ex)
                {
                    outputLogArea.Text += $"FAILED\nPlease delete it yourself and retry.\n\n{ex}";
                    return;
                }
            }

            try
            {
                outputLogArea.Text += "Writing downloaded data to local ZIP file...";
                using (FileStream fs = updateZipFile.OpenWrite())
                {
                    await fs.WriteAsync(updateZipData, 0, updateZipData.Length);
                }
                outputLogArea.Text += "OK\n";
            }
            catch (Exception ex)
            {
                outputLogArea.Text += $"FAILED!\n\n{ex}";
                return;
            }

            try
            {
                outputLogArea.Text += $"\nExtracting {updateZipFile.Name} to current directory ({Program.RunDirectory})...\n";

                using (ZipArchive archive = ZipFile.OpenRead(updateZipFile.FullName))
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        outputLogArea.Text += $"Extracting {entry.FullName}...";
                        string path = Program.RunDirectory + "\\" + entry.FullName;
                        if (path.EndsWith("\\") || path.EndsWith("/"))
                        {
                            DirectoryInfo di = new DirectoryInfo(path.Substring(0, path.Length - 1));
                            di.Create();
                        }
                        else
                        {
                            entry.ExtractToFile(path, true);
                        }
                        outputLogArea.Text += "OK\n";
                    }
            }
            catch (Exception ex)
            {
                outputLogArea.Text += $"\n\n{ex}";
            }

            startGameBtn.PerformClick();
        }

        private void startGameBtn_Click(object sender, EventArgs e)
        {
            try
            {
                outputLogArea.Text += "Starting Guardian.exe...";
                Process.Start("Guardian.exe");
                outputLogArea.Text += "OK";
            }
            catch (Exception ex)
            {
                outputLogArea.Text += $"FAILED\n\n{ex}";
            }
        }

        private async void uploadLogBtn_Click(object sender, EventArgs e)
        {
            FileInfo outputLogFile = new FileInfo(Program.RunDirectory + "\\Guardian_Data\\output_log.txt");
            if (outputLogFile.Exists)
            {
                try
                {
                    using (FileStream fs = outputLogFile.OpenRead())
                    using (StreamReader sr = new StreamReader(fs))
                    using (HttpClient client = new HttpClient())
                    {
                        string content = await sr.ReadToEndAsync();
                        using (StringContent payload = new StringContent(content, Encoding.UTF8, "text/plain"))
                        {
                            outputLogArea.Text = "Uploading Guardian_Data\\output_log.txt to hastebin.com...";
                            HttpResponseMessage response = await client.PostAsync("https://www.toptal.com/developers/hastebin/documents", payload);
                            if (response.IsSuccessStatusCode)
                            {
                                string data = await response.Content.ReadAsStringAsync();
                                using (JsonDocument jd = JsonDocument.Parse(data))
                                {
                                    outputLogArea.Text += "\n\nhttps://www.toptal.com/developers/hastebin/" + jd.RootElement.GetProperty("key").GetString() + ".txt";
                                }
                            }
                            else
                            {
                                throw new Exception("Bad response from https://www.toptal.com/developers/hastebin/");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    outputLogArea.Text = $"FAILED!\n\n{ex}";
                }
            }
            else
            {
                outputLogArea.Text += "No 'output_log.txt' in Guardian_Data";
            }
        }
    }
}
