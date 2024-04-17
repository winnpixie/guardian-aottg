using System;

namespace Guardian
{
    internal class Constants
    {
        public static readonly string AppVersion = "1.0.6";
        public static readonly string InstallDir = Environment.CurrentDirectory;
        public static readonly int OSArch = Environment.Is64BitOperatingSystem ? 64 : 32;

        public static readonly string BaseURL = "https://aottgfan.site/clients/guardian";
        public static readonly string VersionsURL = BaseURL + "/version.txt";
        public static readonly string BinaryName = "Guardian_Win" + OSArch + ".zip";
        public static readonly string GameDataURL = BaseURL + "/" + BinaryName;
    }
}
