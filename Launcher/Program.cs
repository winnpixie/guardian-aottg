using System;
using System.Windows.Forms;

namespace Launcher
{
    static class Program
    {
        public static readonly string Build = "12202021";
        public static string CurrentDirectory;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());

            CurrentDirectory = Environment.CurrentDirectory;
        }
    }
}
