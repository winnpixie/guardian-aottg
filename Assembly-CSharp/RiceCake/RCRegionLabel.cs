using UnityEngine;

public class RCRegionLabel : MonoBehaviour
{
    public GameObject myLabel;

    private void Update()
    {
        if (myLabel != null && myLabel.GetComponent<UILabel>().isVisible)
        {
            myLabel.transform.LookAt(2f * myLabel.transform.position - Camera.main.transform.position);
        }
    }
}
