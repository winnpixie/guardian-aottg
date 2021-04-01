using UnityEngine;

[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour
{
	private void OnGUI()
	{
		if (InputToEvent.goPointedAt != null)
		{
			PhotonView photonView = InputToEvent.goPointedAt.GetPhotonView();
			if (photonView != null)
			{
				Vector3 mousePosition = Input.mousePosition;
				float left = mousePosition.x + 5f;
				float num = Screen.height;
				Vector3 mousePosition2 = Input.mousePosition;
				GUI.Label(new Rect(left, num - mousePosition2.y - 15f, 300f, 30f), string.Format("ViewID {0} InstID {1} Lvl {2} {3}", photonView.viewID, photonView.instantiationId, photonView.prefix, photonView.isSceneView ? "scene" : ((!photonView.isMine) ? ("owner: " + photonView.ownerId) : "mine")));
			}
		}
	}
}
