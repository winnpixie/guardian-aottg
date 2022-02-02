using System;
using System.Windows.Forms;

namespace Launcher
{
    static class Program
    {
        public static readonly string Build = "01FEB2022";
        public static readonly string RunDirectory = Environment.CurrentDirectory;
        public static readonly int Architecture = Environment.Is64BitOperatingSystem ? 64 : 32;

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
