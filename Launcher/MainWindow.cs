using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Diagnostics;
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

        private void Form1_Load(object sender, EventArgs args)
        {
            _currentDirectory = Environment.CurrentDirectory;
        }

        private void updateAndStart_Click(object sender, EventArgs e)
        {
            outputLog.Text = "Beginning download of Guardian.zip";

            try
            {
                FileInfo updateFileRef = new FileInfo(_currentDirectory + "\\Guardian.zip");
                if (updateFileRef.Exists)
                {
                    updateFileRef.Delete();
                }

                using (HttpClient client = new HttpClient())
                {
                    outputLog.Text += $"\nLatest Version : {client.GetStringAsync("https://summie.tk/GUARDIAN_BUILD.TXT").Result}";

                    outputLog.Text += "\nDownloading Guardian.zip from https://alerithe.github.io/guardian/Guardian.zip";
                    using (FileStream fs = updateFileRef.OpenWrite())
                    {
                        byte[] data = client.GetByteArrayAsync("https://alerithe.github.io/guardian/Guardian.zip").Result;
                        fs.Write(data, 0, data.Length);
                    }

                    outputLog.Text += "\nDownload successful";

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

                        outputLog.Text += $"OK";
                    }

                    outputLog.Text += "\nExtraction completed";
                }

                updateFileRef.Delete();

                startNoUpdate.PerformClick();
            }
            catch (Exception ex)
            {
                outputLog.Text += $"\n{ex}";
            }
        }

        private void startNoUpdate_Click(object sender, EventArgs e)
        {
            outputLog.Text = "\nLaunching Guardian.exe";

            try
            {
                Process.Start(new ProcessStartInfo("Guardian.exe"));
                this.Close();
            }
            catch
            {
                outputLog.Text += "\nCould not launch application 'Guardian.exe'";
            }
        }
    }
}
