using UnityEngine;

public class BodyPushBox : MonoBehaviour
{
    public GameObject parent;

    private void OnTriggerStay(Collider other)
    {
        if (!(other.gameObject.tag == "bodyCollider"))
        {
            return;
        }

        BodyPushBox component = other.gameObject.GetComponent<BodyPushBox>();
        if ((bool)component && (bool)component.parent)
        {
            Vector3 vector = component.parent.transform.position - parent.transform.position;
            vector.y = 0f;
            if (vector.magnitude > 0f)
            {
                vector.Normalize();
            }
            else
            {
                vector.x = 1f;
            }
        }
    }
}
