using UnityEngine;

public class ParentFollow : MonoBehaviour
{
    private Transform parent;
    private Transform bTransform;
    public bool isActiveInScene;

    public void SetParent(Transform transform)
    {
        parent = transform;
        bTransform.rotation = transform.rotation;
    }

    public void RemoveParent()
    {
        parent = null;
    }

    private void Awake()
    {
        bTransform = base.transform;
        isActiveInScene = true;
    }

    private void Update()
    {
        if (isActiveInScene && parent != null)
        {
            bTransform.position = parent.position;
        }
    }
}
