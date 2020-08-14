using System;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using Guardian.Utilities;

public class BTN_save_snapshot : MonoBehaviour
{
    public GameObject targetTexture;
    public GameObject info;
    public GameObject[] thingsNeedToHide;
    private string saveDirectory;

    public void Start()
    {
        saveDirectory = $"{Application.dataPath}\\..\\Screenshots";
        GameHelper.TryCreateFile(saveDirectory, true);
    }

    private void OnClick()
    {
        GameObject[] array = thingsNeedToHide;
        foreach (GameObject gameObject in array)
        {
            gameObject.transform.position += Vector3.up * 10000f;
        }
        StartCoroutine(ScreenshotEncode());
        info.GetComponent<UILabel>().text = "Attempting to save snapshot..";
    }

    private IEnumerator ScreenshotEncode()
    {
        yield return new WaitForEndOfFrame();
        float r = (float)Screen.height / 600f;
        Vector3 localScale = targetTexture.transform.localScale;
        int width = (int)(r * localScale.x);
        Vector3 localScale2 = targetTexture.transform.localScale;
        Texture2D texture = new Texture2D(width, (int)(r * localScale2.y), TextureFormat.RGB24, mipmap: false);
        texture.ReadPixels(new Rect((float)Screen.width * 0.5f - (float)texture.width * 0.5f, (float)Screen.height * 0.5f - (float)texture.height * 0.5f - r * 0f, texture.width, texture.height), 0, 0);
        texture.Apply();
        yield return 0;
        GameObject[] array = thingsNeedToHide;
        foreach (GameObject go in array)
        {
            go.transform.position -= Vector3.up * 10000f;
        }
        string img_name = "aottg_ss-" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Day.ToString() + "_" + DateTime.Today.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".png";
        byte[] imgData = texture.EncodeToPNG();
        GameHelper.TryCreateFile(saveDirectory, true);
        File.WriteAllBytes($"{saveDirectory}\\{img_name}", imgData);
        Application.ExternalCall("SaveImg", img_name, texture.width, texture.height, Convert.ToBase64String(imgData));
        UnityEngine.Object.DestroyObject(texture);
    }
}
