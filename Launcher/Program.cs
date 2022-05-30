using System;
using System.Windows.Forms;

namespace Launcher
{
    static class Program
    {
        public static readonly string Build = "05-30-2022";
        public static readonly string Cwd = Environment.CurrentDirectory;
        public static readonly int Arch = Environment.Is64BitOperatingSystem ? 64 : 32;

        public static readonly string BaseURL = "http://www.aottg.tk/mods/guardian";
        public static readonly string VersionsURL = BaseURL + "/versions.txt";
        public static readonly string BinaryName = "Guardian" + Arch + ".zip";
        public static readonly string GameDataURL = BaseURL + "/binaries/" + BinaryName;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
