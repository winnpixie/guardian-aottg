using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs args)
        {
        }

        private void updateAndStart_Click(object sender, EventArgs e)
        {
            outputLog.Text = "Beginning download of Guardian.zip";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    outputLog.Text += $"\nLatest Version : {client.GetStringAsync("https://summie.tk/GUARDIAN_BUILD.TXT").Result}";

                    outputLog.Text += "\nDownloading Guardian.zip from https://alerithe.github.io/guardian/Guardian.zip";

                    File.WriteAllBytes(Environment.CurrentDirectory + "\\Guardian.zip",
                        client.GetByteArrayAsync("https://alerithe.github.io/guardian/Guardian.zip").Result);

                    outputLog.Text += "\nDownload successful";

                }

                using (ZipArchive archive = ZipFile.OpenRead(Environment.CurrentDirectory + "\\Guardian.zip"))
                {
                    outputLog.Text += $"\nExtracting Guardian.zip to current directory ({Environment.CurrentDirectory})";

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        outputLog.Text += $"\nExtracting {entry.FullName}...";

                        if (entry.FullName.EndsWith("\\") || entry.FullName.EndsWith("/"))
                        {
                            Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + entry.FullName.Substring(0, entry.FullName.Length - 1)).Create();
                        }
                        else
                        {
                            entry.ExtractToFile(Environment.CurrentDirectory + "\\" + entry.FullName, true);
                        }

                        outputLog.Text += $"OK";
                    }

                    outputLog.Text += "\nExtraction completed";
                }

                File.Delete(Environment.CurrentDirectory + "\\Guardian.zip");

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
