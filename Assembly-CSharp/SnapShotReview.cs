using System;
using UnityEngine;

public class SnapShotReview : MonoBehaviour
{
    public GameObject labelPage;
    public GameObject texture;
    public GameObject labelDMG;
    public GameObject labelInfo;
    private float textureW = 960f;
    private float textureH = 600f;
    private UILabel page;

    private void Start()
    {
        QualitySettings.SetQualityLevel(5, applyExpensiveChanges: true);
        page = labelPage.GetComponent<UILabel>();
        if (SnapShotSaves.GetLength() > 0)
        {
            texture.GetComponent<UITexture>().mainTexture = SnapShotSaves.GetCurrentImage();
        }
        labelInfo.GetComponent<UILabel>().text = LoginFengKAI.Player.Name + " " + DateTime.Today.ToShortDateString();
        RefreshInfo();
        SetTextureDimensions();
    }

    private void SetTextureDimensions()
    {
        if (SnapShotSaves.GetLength() != 0)
        {
            float num = 1.6f;
            float num2 = (float)texture.GetComponent<UITexture>().mainTexture.width / (float)texture.GetComponent<UITexture>().mainTexture.height;
            if (num2 > num)
            {
                texture.transform.localScale = new Vector3(textureW, textureW / num2, 0f);
                labelDMG.transform.localPosition = new Vector3((int)(textureW * 0.5f - 20f), (int)(0f + textureW * 0.5f / num2 - 20f), -20f);
                labelInfo.transform.localPosition = new Vector3((int)(textureW * 0.5f - 20f), (int)(0f - textureW * 0.5f / num2 + 20f), -20f);
            }
            else
            {
                texture.transform.localScale = new Vector3(textureH * num2, textureH, 0f);
                labelDMG.transform.localPosition = new Vector3((int)(textureH * num2 * 0.5f - 20f), (int)(0f + textureH * 0.5f - 20f), -20f);
                labelInfo.transform.localPosition = new Vector3((int)(textureH * num2 * 0.5f - 20f), (int)(0f - textureH * 0.5f + 20f), -20f);
            }
        }
    }

    private void RefreshInfo()
    {
        if (SnapShotSaves.GetLength() == 0)
        {
            page.text = " 0 / 0";
        }
        else
        {
            page.text = " " + (SnapShotSaves.GetCurrentIndex() + 1) + " / " + SnapShotSaves.GetLength();
        }
        if (SnapShotSaves.GetCurrentDamage() > 0)
        {
            labelDMG.GetComponent<UILabel>().text = SnapShotSaves.GetCurrentDamage().ToString();
        }
        else
        {
            labelDMG.GetComponent<UILabel>().text = string.Empty;
        }
    }

    public void ShowNextImage()
    {
        texture.GetComponent<UITexture>().mainTexture = SnapShotSaves.GetNextImage();
        SetTextureDimensions();
        RefreshInfo();
    }

    public void ShowPreviousImage()
    {
        texture.GetComponent<UITexture>().mainTexture = SnapShotSaves.GetPreviousImage();
        SetTextureDimensions();
        RefreshInfo();
    }
}
