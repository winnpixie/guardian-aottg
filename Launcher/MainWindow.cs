using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
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
            base.Text += $" (v.{Program.Build})";
            outputLog.AutoWordSelection = true;
        }

        private void updateAndStart_Click(object sender, EventArgs e)
        {
            new Thread(async () =>
            {
                try
                {
                    outputLog.Text = "Attempting to terminate active Guardian tasks...";

                    Process.Start(new ProcessStartInfo("taskkill.exe", "/F /IM Guardian.exe"));

                    outputLog.Text += "OK!\n";
                }
                catch { }

                byte[] zipData;
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        outputLog.Text += "\nObtaining latest build information...";

                        string latestVersion = await client.GetStringAsync("https://summie.tk/guardian/version.txt?t=" + Environment.TickCount);

                        outputLog.Text += "OK! (" + latestVersion + ")\n";
                    }
                    catch (Exception ex)
                    {
                        outputLog.Text += $"FAILED!\n\n{ex}\n\n";
                    }

                    try
                    {
                        outputLog.Text += "Downloading Guardian.zip from https://alerithe.github.io/guardian/Guardian.zip...";

                        zipData = await client.GetByteArrayAsync("https://alerithe.github.io/guardian/Guardian.zip?t=" + Environment.TickCount);

                        outputLog.Text += "OK!\n";
                    }
                    catch (Exception ex)
                    {
                        outputLog.Text += $"FAILED!\n\n{ex}";

                        return;
                    }
                }

                FileInfo zipFile = new FileInfo(Program.CurrentDirectory + "\\Guardian.zip");
                try
                {
                    outputLog.Text += "Writing downloaded data to local file...";

                    using (FileStream fs = zipFile.Open(FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(zipData, 0, zipData.Length);
                    }

                    outputLog.Text += "OK!\n";
                }
                catch (Exception ex)
                {
                    outputLog.Text += $"FAILED!\n\n{ex}";

                    if (zipFile.Exists)
                    {
                        try
                        {
                            zipFile.Delete();
                        }
                        catch { }
                    }
                    return;
                }

                try
                {
                    outputLog.Text += $"\nExtracting Guardian.zip to current directory ({Program.CurrentDirectory})...";

                    using (ZipArchive archive = ZipFile.Open(zipFile.FullName, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            outputLog.Text += $"\nExtracting {entry.FullName}...";

                            string path = Program.CurrentDirectory + "\\" + entry.FullName;
                            if (path.EndsWith("\\") || path.EndsWith("/"))
                            {
                                DirectoryInfo di = new DirectoryInfo(path.Substring(0, path.Length - 1));
                                if (!di.Exists)
                                {
                                    di.Create();
                                }
                            }
                            else
                            {
                                entry.ExtractToFile(path, true);
                            }

                            outputLog.Text += "OK!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    outputLog.Text += $"\n\n{ex}";
                }

                if (zipFile.Exists)
                {
                    try
                    {
                        zipFile.Delete();
                    }
                    catch { }
                }

                startNoUpdate.PerformClick();
            }).Start();
        }

        private void startNoUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                outputLog.Text += "\nLaunching Guardian.exe...";

                Process.Start(new ProcessStartInfo("Guardian.exe"));

                outputLog.Text += "OK!";
            }
            catch (Exception ex)
            {
                outputLog.Text += $"FAILED!\n\n{ex}";
            }
        }

        private void uploadLog_Click(object sender, EventArgs e)
        {
            new Thread(async () =>
            {
                try
                {
                    outputLog.Text = "Uploading Guardian_Data\\output_log.txt to hastebin.com...";

                    FileInfo outputLogFile = new FileInfo(Program.CurrentDirectory + "\\Guardian_Data\\output_log.txt");
                    if (outputLogFile.Exists)
                    {
                        using (FileStream fs = outputLogFile.Open(FileMode.Open, FileAccess.Read))
                        {
                            using (StreamReader sr = new StreamReader(fs))
                            {
                                string content = await sr.ReadToEndAsync();
                                using (HttpClient client = new HttpClient())
                                {
                                    using (StringContent payload = new StringContent(content, Encoding.UTF8, "application/json"))
                                    {
                                        HttpResponseMessage response = await client.PostAsync("https://hastebin.com/documents", payload);
                                        if (response.IsSuccessStatusCode)
                                        {
                                            string data = await response.Content.ReadAsStringAsync();
                                            using (JsonDocument jd = JsonDocument.Parse(data))
                                            {
                                                outputLog.Text += "OK!\n\nhttps://hastebin.com/" + jd.RootElement.GetProperty("key").GetString() + ".txt";
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
                    outputLog.Text = $"FAILED!\n\n{ex}";
                }
            }).Start();
        }
    }
}
