using Guardian.UI.Toasts;
using System.Collections;
using UnityEngine;

namespace Guardian.Utilities
{
    public class UpdateChecker
    {
        public static IEnumerator CheckForUpdate()
        {
            GuardianClient.Logger.Info("Checking for update...");
            GuardianClient.Logger.Info($"Installed: {GuardianClient.Build}");

            using WWW www = new WWW("http://aottgfan.site/clients/guardian/version.txt?t=" + GameHelper.CurrentTimeMillis()); // Hopefully this avoids caching...
            yield return www;

            if (www.error != null)
            {
                GuardianClient.Logger.Error(www.error);
                GuardianClient.Toasts.Add(new Toast("UPDATE CHECK FAILED", www.error, 10));

                GuardianClient.Logger.Error($"\nIf errors persist, PLEASE contact me!");
                GuardianClient.Logger.Info("Discord:");
                GuardianClient.Logger.Info($"\t- {"https://discord.gg/JGzTdWm".AsColor("0099FF")}");

                try
                {
                    GameObject.Find("VERSION").GetComponent<UILabel>().text = "[FF0000]COULD NOT VERIFY BUILD.[-] If this persists, PLEASE contact me @ [0099FF]https://discord.gg/JGzTdWm[-]!";
                }
                catch { }
            }
            else
            {
                foreach (string buildData in www.text.Split('\n'))
                {
                    string[] buildInfo = buildData.Split(new char[] { '=' }, 2);
                    if (!buildInfo[0].Equals("MOD")) continue;

                    string latestBuild = buildInfo[1].Trim();
                    GuardianClient.Logger.Info("Latest: " + latestBuild);

                    if (latestBuild.Equals(GuardianClient.Build)) break;

                    GuardianClient.Logger.Warn($"Your copy of Guardian is {"OUT OF DATE".AsBold().AsItalic().AsColor("FF0000")}!");
                    GuardianClient.Toasts.Add(new Toast("UPDATE AVAILABLE", "Your copy of Guardian is OUT OF DATE, please update!", 15));

                    GuardianClient.Logger.Info("If you don't have the launcher, download it here:");
                    GuardianClient.Logger.Info($"\t- {"https://aottgfan.site/".AsColor("0099FF")}");

                    try
                    {
                        GameObject.Find("VERSION").GetComponent<UILabel>().text = "[FF0000]OUT OF DATE![-] Please update with the Launcher (DL @ [0099FF]https://aottgfan.site/[-])!";
                    }
                    catch { }
                }
            }
        }
    }
}
