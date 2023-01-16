using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class MainWindow : Form
    {
        private readonly HttpClient httpClient = new HttpClient();

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

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            httpClient.Dispose();
        }

        private void SetLogText(string message)
        {
            LogOutputArea.Text = message;
        }

        private void PrependToLog(string message)
        {
            LogOutputArea.Text = message + LogOutputArea.Text;
        }

        private void AppendToLog(string message)
        {
            LogOutputArea.Text += message;
        }

        private void StartGame(bool clearLog)
        {
            try
            {
                if (clearLog)
                {
                    SetLogText("Starting Guardian.exe...");
                }
                else
                {
                    AppendToLog("Starting Guardian.exe...");
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "Guardian.exe",
                    WorkingDirectory = Program.Cwd
                };

                Process.Start(psi);
                AppendToLog("\n");
            }
            catch (Exception ex)
            {
                AppendToLog($"FAILED\n\n{ex}");
            }
        }

        private async void UpdateAndPlayBtn_Start(object sender, EventArgs e)
        {
            SetLogText(string.Empty);

            // Terminate any active processes related to our software
            try
            {
                AppendToLog("Attempting to kill active Guardian processes...");
                Process.Start(new ProcessStartInfo("taskkill.exe", "/F /IM Guardian.exe"));
            }
            catch { }

            // Get and print the latest build information
            try
            {
                AppendToLog("\nObtaining latest build information...");
                string latestVersion = await GetVersionData();
                AppendToLog($"\n{latestVersion}");
            }
            catch (Exception ex)
            {
                AppendToLog($"ERROR, SKIPPING\n\n{ex}\n");
            }

            byte[] binZipData;

            // Get the binary data
            try
            {
                AppendToLog($"\nDownloading binaries for {Program.Arch}-bit Windows...");
                binZipData = await GetGameData();
            }
            catch (Exception ex)
            {
                AppendToLog($"FAILED\n\n{ex}");
                return;
            }

            FileInfo gameZip = new FileInfo(Program.Cwd + $"\\{Program.BinaryName}");

            // Delete previous ZIP to try and minimize unintended behaviour
            if (gameZip.Exists)
            {
                try
                {
                    AppendToLog($"\nDeleting previous ZIP...");
                    gameZip.Delete();
                }
                catch (Exception ex)
                {
                    AppendToLog($"FAILED\nPlease delete it yourself and retry.\n\n{ex}\n");
                    return;
                }
            }

            // Write ZIP data to local file
            try
            {
                AppendToLog($"\nWriting ZIP data to {gameZip.FullName}...");
                await WriteGameData(gameZip, binZipData);
            }
            catch (Exception ex)
            {
                AppendToLog($"FAILED\n\n{ex}\n");
                return;
            }

            // Extract ZIP contents to current working directory
            try
            {
                AppendToLog($"\nExtracting {gameZip.FullName} to current directory ({Program.Cwd})...");
                ExtractToDirectory(gameZip);
            }
            catch (Exception ex)
            {
                AppendToLog($"FAILED\n\n{ex}\n");
                return;
            }

            // Delete ZIP file since we're done with it
            try
            {
                AppendToLog("\nCleaning up...");
                gameZip.Delete();
                AppendToLog("\n");
            }
            catch (Exception ex)
            {
                AppendToLog($"FAILED (This is probably still okay!!)\n\n{ex}\n");
            }

            StartGame(false);
        }

        private async Task<string> GetVersionData()
        {
            return await httpClient.GetStringAsync($"{Program.VersionsURL}?t=" + Environment.TickCount);
        }

        private async Task<byte[]> GetGameData()
        {
            return await httpClient.GetByteArrayAsync($"{Program.GameDataURL}?t=" + Environment.TickCount);
        }

        private async Task<bool> WriteGameData(FileInfo file, byte[] binData)
        {
            using (FileStream fs = file.OpenWrite())
            {
                await fs.WriteAsync(binData, 0, binData.Length);
                return true;
            }
        }

        private void ExtractToDirectory(FileInfo file)
        {
            using (ZipArchive archive = ZipFile.OpenRead(file.FullName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    AppendToLog($"\nExtracting {entry.FullName}...");

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

        private void StartGameBtn_Click(object sender, EventArgs e)
        {
            StartGame(true);
        }

        private async void UploadLogBtn_Click(object sender, EventArgs e)
        {
            // Ensure user has ran the game at least once
            FileInfo outputLogFile = new FileInfo(Program.Cwd + "\\Guardian_Data\\output_log.txt");
            if (!outputLogFile.Exists)
            {
                SetLogText("No 'output_log.txt' in Guardian_Data");
                return;
            }

            try
            {
                SetLogText("Uploading Guardian_Data\\output_log.txt to hastebin.com...");
                AppendToLog("\n\n" + await UploadLog(outputLogFile));
            }
            catch (Exception ex)
            {
                AppendToLog($"FAILED\n\n{ex}\n");
            }
        }

        private async Task<string> UploadLog(FileInfo outputLog)
        {
            // Read contents of output_log.txt
            using (StreamReader sr = new StreamReader(outputLog.OpenRead()))
            {
                string outputText = await sr.ReadToEndAsync();

                // Upload file contents to hastebin.com
                using (StringContent payload = new StringContent(outputText, Encoding.UTF8, "text/plain"))
                {
                    HttpResponseMessage response = await httpClient.PostAsync("https://hastebin.com/documents", payload);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Bad response from https://hastebin.com/");
                    }

                    // Read JSON response and return URL
                    string responseBody = await response.Content.ReadAsStringAsync();
                    using (JsonDocument jd = JsonDocument.Parse(responseBody))
                    {
                        return "https://hastebin.com/" + jd.RootElement.GetProperty("key").GetString() + ".txt";
                    }
                }
            }
        }
    }
}
