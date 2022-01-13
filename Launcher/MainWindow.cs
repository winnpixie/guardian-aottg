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
            // Try to kill all currently running tasks pertaining to us so we can overwrite files
            try
            {
                outputLogArea.Text = "Attempting to terminate active Guardian tasks...";
                Process.Start(new ProcessStartInfo("taskkill.exe", "/F /IM Guardian.exe"));
            }
            catch { }

            string fileName = $"Guardian{Program.Architecture}.zip";

            byte[] updateZipData;
            using (HttpClient client = new HttpClient())
            {
                // Download latest version string
                try
                {
                    outputLogArea.Text += "\nObtaining latest build information...";
                    string latestVersion = await client.GetStringAsync("https://www.sativa.tk/guardian/version.txt?t=" + Environment.TickCount);
                    outputLogArea.Text += $" {latestVersion}\n";
                }
                catch (Exception ex)
                {
                    outputLogArea.Text = $" UNKNOWN\n\n{ex}\n";
                }

                // Download update data
                try
                {
                    outputLogArea.Text += $"\nDownloading Guardian.zip from https://suhtiva.github.io/guardian/{fileName}...";
                    updateZipData = await client.GetByteArrayAsync($"https://suhtiva.github.io/guardian/{fileName}?t=" + Environment.TickCount); // Attempt to avoid cache issues
                }
                catch (Exception ex)
                {
                    outputLogArea.Text += $"FAILED!\n\n{ex}";
                    return;
                }
            }

            FileInfo updateZipFile = new FileInfo(Program.RunDirectory + $"\\{fileName}");

            // Try to delete pre-existing update archive, possibly avoiding undefined behavior
            try
            {
                if (updateZipFile.Exists)
                {
                    outputLogArea.Text += "Deleting pre-existing update ZIP file...";
                    updateZipFile.Delete();
                }
            }
            catch (Exception ex)
            {
                outputLogArea.Text += $"FAILED!\nPlease delete it yourself and retry.\n\n{ex}";
                return;
            }

            // Write downloaded to a local update archive
            try
            {
                outputLogArea.Text += "Writing downloaded data to local ZIP file...";

                using (FileStream fs = updateZipFile.OpenWrite())
                {
                    await fs.WriteAsync(updateZipData, 0, updateZipData.Length);
                }
            }
            catch (Exception ex)
            {
                outputLogArea.Text += $"FAILED!\n\n{ex}";

                // Try to remove potentially corrupted update archive
                if (updateZipFile.Exists)
                {
                    try
                    {
                        updateZipFile.Delete();
                    }
                    catch { }
                }

                return;
            }

            try
            {
                // Extract contents of update archive to their respective locations
                outputLogArea.Text += $"\nExtracting {updateZipFile.Name} to current directory ({Program.RunDirectory})...";

                using (ZipArchive archive = ZipFile.OpenRead(updateZipFile.FullName))
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        outputLogArea.Text += $"\nExtracting {entry.FullName}...";

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
                    }
            }
            catch (Exception ex)
            {
                outputLogArea.Text += $"\n\n{ex}";
            }

            // Delete update archive as we no longer need it
            if (updateZipFile.Exists)
            {
                try
                {
                    updateZipFile.Delete();
                }
                catch { }
            }

            startGameBtn.PerformClick();
        }

        private void startGameBtn_Click(object sender, EventArgs e)
        {
            try
            {
                outputLogArea.Text += "\nLaunching Guardian.exe...";
                Process.Start(new ProcessStartInfo("Guardian.exe"));
                base.Close();
            }
            catch (Exception ex)
            {
                outputLogArea.Text += $"FAILED!\n\n{ex}";
            }
        }

        private async void uploadLogBtn_Click(object sender, EventArgs e)
        {
            try
            {
                outputLogArea.Text = "Uploading Guardian_Data\\output_log.txt to hastebin.com...";

                FileInfo outputLogFile = new FileInfo(Program.RunDirectory + "\\Guardian_Data\\output_log.txt");
                if (outputLogFile.Exists)
                {
                    using (FileStream fs = outputLogFile.Open(FileMode.Open, FileAccess.Read))
                    using (StreamReader sr = new StreamReader(fs))
                    using (HttpClient client = new HttpClient())
                    {
                        string content = await sr.ReadToEndAsync();
                        using (StringContent payload = new StringContent(content, Encoding.UTF8, "text/plain"))
                        {
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
                else
                {
                    throw new Exception("No 'output_log.txt' found in 'Guardian_Data'!");
                }
            }
            catch (Exception ex)
            {
                outputLogArea.Text = $"FAILED!\n\n{ex}";
            }
        }
    }
}
