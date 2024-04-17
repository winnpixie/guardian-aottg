using Guardian;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class MainWindow : Form
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs args)
        {
            OutputLogBox.AutoWordSelection = true;

            InformationLbl.Text = string.Format(InformationLbl.Text, Environment.OSVersion.VersionString, Constants.OSArch, Constants.AppVersion);
            InformationLbl.Refresh();

            httpClient.Timeout = TimeSpan.MaxValue;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            httpClient.Dispose();
        }

        private void SetLogText(string message)
        {
            OutputLogBox.Text = message;
        }

        private void AppendToLog(string message)
        {
            OutputLogBox.Text += message;
        }

        private void StartGame(bool clearLog)
        {
            try
            {
                if (clearLog)
                {
                    SetLogText("Starting Guardian...");
                }
                else
                {
                    AppendToLog("Starting Guardian...");
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "Guardian.exe",
                    WorkingDirectory = Constants.InstallDir
                };

                Process.Start(psi);
                AppendToLog("\n");
            }
            catch (Exception ex)
            {
                AppendToLog($"FAILED\n\n{ex}");
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            SetLogText("!! PLEASE MAKE SURE GUARDIAN MOD IS NOT OPEN !!\n");

            Task.Run(async () =>
            {
                // Get and print the latest build information
                try
                {
                    AppendToLog("\nObtaining latest build information...");
                    string versionData = await GetVersionData();
                    AppendToLog($"\n{versionData}");

                    foreach (string buildData in versionData.Split('\n'))
                    {
                        string[] buildInfo = buildData.Split(new char[] { '=' }, 2);
                        if (!buildInfo[0].Equals("LAUNCHER")) continue;

                        string latestBuild = buildInfo[1].Trim();
                        if (latestBuild.Equals(Constants.AppVersion)) break;

                        AppendToLog("\nYour copy of the Guardian Mod Launcher is OUT OF DATE, please update!");
                        AppendToLog("\n\t- https://cb.run/GuardianAoT");
                    }
                }
                catch
                {
                    AppendToLog($"!! ERROR !! (This is *probably* fine?...)\n");
                }

                byte[] gameZipData;
                // Get the binary data
                try
                {
                    AppendToLog($"\nDownloading binaries for {Constants.OSArch}-bit Windows...");
                    gameZipData = await GetGameData();
                }
                catch (Exception ex)
                {
                    SetLogText($"!! ERROR !! Could not download client files.\n\n{ex}");
                    return;
                }

                FileInfo gameZip = new FileInfo(Constants.InstallDir + $"\\{Constants.BinaryName}");
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
                        SetLogText($"!! ERROR !! Please delete any pre-existing ZIP files and retry.\n\n{ex}");
                        return;
                    }
                }

                // Write ZIP data to local file
                try
                {
                    AppendToLog($"\nWriting ZIP data to {gameZip.FullName}...");
                    await WriteGameData(gameZip, gameZipData);
                }
                catch (Exception ex)
                {
                    SetLogText($"!! ERROR !! Could not save client files...\n\n{ex}\n");
                    return;
                }

                // Extract ZIP contents to current working directory
                try
                {
                    AppendToLog($"\nExtracting {gameZip.FullName} to current directory ({Constants.InstallDir})...");
                    ExtractToDirectory(gameZip);
                }
                catch (Exception ex)
                {
                    SetLogText($"!! ERROR !! Could not extract client files...\n\n{ex}\n");
                    return;
                }

                // Delete ZIP file since we're done with it
                try
                {
                    AppendToLog("\nCleaning up...");
                    gameZip.Delete();
                    AppendToLog("\n");
                }
                catch
                {
                    AppendToLog($"!! ERROR !! (This is *probably* fine?...)\n");
                }

                StartGame(false);
            });
        }

        private async Task<string> GetVersionData()
        {
            return await httpClient.GetStringAsync($"{Constants.VersionsURL}?t={Environment.TickCount}");
        }

        private async Task<byte[]> GetGameData()
        {
            return await httpClient.GetByteArrayAsync($"{Constants.GameDataURL}?t=" + Environment.TickCount);
        }

        private async Task WriteGameData(FileInfo file, byte[] binData)
        {
            using (FileStream fs = file.OpenWrite())
            {
                await fs.WriteAsync(binData, 0, binData.Length);
            }
        }

        private void ExtractToDirectory(FileInfo file)
        {
            using (ZipArchive archive = ZipFile.OpenRead(file.FullName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string path = Constants.InstallDir + "\\" + entry.FullName;
                    if (path.EndsWith("\\") || path.EndsWith("/"))
                    {
                        DirectoryInfo di = new DirectoryInfo(path.Substring(0, path.Length - 1));
                        di.Create();
                    }
                    else
                    {
                        AppendToLog($"\nExtracting {entry.FullName}...");
                        entry.ExtractToFile(path, true);
                    }
                }
            }
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            StartGame(true);
        }
    }
}
