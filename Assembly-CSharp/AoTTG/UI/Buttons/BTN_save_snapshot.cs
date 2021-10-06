using System;
using System.Collections;
using UnityEngine;
using System.IO;

public class BTN_save_snapshot : MonoBehaviour
{
    public GameObject targetTexture;
    public GameObject info;
    public GameObject[] thingsNeedToHide;
    private string SaveDir = Guardian.Mod.RootDir + "\\Screenshots";

    private void OnPress()
    {
        try
        {
            foreach (GameObject go in thingsNeedToHide)
            {
                go.transform.position -= Vector3.up * 10000f;
            }

            base.transform.position -= Vector3.up * 10000f;
        } catch
        {
            info.GetComponent<UILabel>().text = "Error preparing Snapshot.";
        }

        info.GetComponent<UILabel>().text = "Attempting to save snapshot..";
        StartCoroutine(CoEncodeScreenshot());
    }

    private IEnumerator CoEncodeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        try
        {
            float scale = Screen.height / 600f;
            Vector3 texScale = targetTexture.transform.localScale;
            Texture2D texture = new Texture2D((int)(scale * texScale.x), (int)(scale * texScale.y), TextureFormat.RGB24, mipmap: false);
            texture.ReadPixels(new Rect((Screen.width / 2f) - (texture.width / 2f), (Screen.height / 2f) - (texture.height / 2f), texture.width, texture.height), 0, 0);
            texture.Apply();

            DateTime now = DateTime.Now;
            string imageName = "SnapShot-" + now.Day + "_" + now.Month + "_" + now.Year + "-" + now.Hour + "_" + now.Minute + "_" + now.Second + ".jpg";
            byte[] imageData = texture.EncodeToJPG(100);

            Guardian.Utilities.GameHelper.TryCreateFile(SaveDir, true);
            File.WriteAllBytes($"{SaveDir}\\{imageName}", imageData);

            // ExternalCall is legacy code, used to execute JavaScript on http://fenglee.com/game/aog/
            // Application.ExternalCall("SaveImg", imageName, texture.width, texture.height, Convert.ToBase64String(imageData));
            UnityEngine.Object.DestroyObject(texture);

            info.GetComponent<UILabel>().text = "Snapshot saved.";
        }
        catch
        {
            info.GetComponent<UILabel>().text = "Error saving Snapshot.";
        }

        foreach (GameObject go in thingsNeedToHide)
        {
            go.transform.position += Vector3.up * 10000f;
        }

        base.transform.position += Vector3.up * 10000f;
    }
}
