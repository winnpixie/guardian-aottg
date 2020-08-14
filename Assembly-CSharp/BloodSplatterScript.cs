using UnityEngine;

public class BloodSplatterScript : MonoBehaviour
{
    public Transform bloodPrefab;
    public int maxAmountBloodPrefabs = 20;
    private GameObject[] bloodInstances;
    public Transform bloodPosition;
    public Transform bloodRotation;
    public int bloodLocalRotationYOffset;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bloodRotation.Rotate(0f, bloodLocalRotationYOffset, 0f);
            Transform transform = Object.Instantiate(bloodPrefab, bloodPosition.position, bloodRotation.rotation) as Transform;
            bloodInstances = GameObject.FindGameObjectsWithTag("blood");
            if (bloodInstances.Length >= maxAmountBloodPrefabs)
            {
                Object.Destroy(bloodInstances[0]);
            }
        }
    }
}
