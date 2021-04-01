using UnityEngine;

public class PanelGroupManager
{
    public GameObject[] panelGroup;

    public void ActivePanel(int index)
    {
        GameObject[] array = panelGroup;
        foreach (GameObject gameObject in array)
        {
            gameObject.SetActive(value: false);
        }
        panelGroup[index].SetActive(value: true);
    }
}
