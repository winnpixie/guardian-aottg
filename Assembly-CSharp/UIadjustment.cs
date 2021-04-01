using UnityEngine;

public class UIadjustment : MonoBehaviour
{
    private void Start()
    {
        base.transform.localScale = new Vector3(Screen.width / 960, Screen.height / 600, 0f);
    }

    private void Update()
    {
        base.transform.localScale = new Vector3(Screen.width / 960, Screen.height / 600, 0f);
    }
}
