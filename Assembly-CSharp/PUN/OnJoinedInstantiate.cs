using UnityEngine;

public class OnJoinedInstantiate : MonoBehaviour
{
	public Transform SpawnPosition;

	public float PositionOffset = 2f;

	public GameObject[] PrefabsToInstantiate;

	public void OnJoinedRoom()
	{
		if (PrefabsToInstantiate == null)
		{
			return;
		}
		GameObject[] prefabsToInstantiate = PrefabsToInstantiate;
		foreach (GameObject gameObject in prefabsToInstantiate)
		{
			Debug.Log("Instantiating: " + gameObject.name);
			Vector3 a = Vector3.up;
			if (SpawnPosition != null)
			{
				a = SpawnPosition.position;
			}
			Vector3 a2 = Random.insideUnitSphere;
			a2.y = 0f;
			a2 = a2.normalized;
			Vector3 position = a + PositionOffset * a2;
			PhotonNetwork.Instantiate(gameObject.name, position, Quaternion.identity, 0);
		}
	}
}
