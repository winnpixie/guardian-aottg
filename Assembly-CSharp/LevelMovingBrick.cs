using UnityEngine;

public class LevelMovingBrick : MonoBehaviour
{
	public GameObject pointGOA;

	public GameObject pointGOB;

	public bool towardsA = true;

	public float speed = 10f;

	private Vector3 pointA;

	private Vector3 pointB;

	private void Start()
	{
		pointA = pointGOA.transform.position;
		pointB = pointGOB.transform.position;
		Object.Destroy(pointGOA);
		Object.Destroy(pointGOB);
	}

	private void Update()
	{
		if (towardsA)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, pointA, speed * Time.deltaTime);
			if (Vector3.Distance(base.transform.position, pointA) < 2f)
			{
				towardsA = false;
			}
		}
		else
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, pointB, speed * Time.deltaTime);
			if (Vector3.Distance(base.transform.position, pointB) < 2f)
			{
				towardsA = true;
			}
		}
	}
}
