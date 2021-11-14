using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Guardian.Features.Commands.Impl
{
    class CommandScreenshot : Command
    {
        private string SaveDir = Mod.RootDir + "\\Screenshots";

        public CommandScreenshot() : base("screenshot", new string[] { "ss" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            float scale = 1f;
            if (args.Length > 0 && float.TryParse(args[0], out scale))
            {
                if (scale > 4)
                {
                    scale = 4;
                }
            }

            FengGameManagerMKII.Instance.StartCoroutine(CoTakeScreenshot(scale));
        }

        private IEnumerator CoTakeScreenshot(float scale)
        {
            yield return new WaitForEndOfFrame();

            RenderTexture oldCamRt = Camera.main.targetTexture;
            RenderTexture oldRt = RenderTexture.active;

            int width = (int)(Screen.width * scale);
            int height = (int)(Screen.height * scale);
            RenderTexture rt = new RenderTexture(width, height, 24);

            Camera.main.targetTexture = rt;
            RenderTexture.active = rt;

            Camera.main.Render();

            Texture2D capture = new Texture2D(width, height);
            capture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            capture.Apply();

            RenderTexture.active = oldRt;
            Camera.main.targetTexture = oldCamRt;

            DateTime now = DateTime.Now;
            string imageName = "SnapShot-" + now.Day + "_" + now.Month + "_" + now.Year + "-" + now.Hour + "_" + now.Minute + "_" + now.Second + ".jpg";

            Utilities.GameHelper.TryCreateFile(SaveDir, true);
            File.WriteAllBytes($"{SaveDir}\\{imageName}", capture.EncodeToJPG(100));

            UnityEngine.Object.DestroyImmediate(capture);
            UnityEngine.Object.DestroyImmediate(rt);
        }
    }
}
