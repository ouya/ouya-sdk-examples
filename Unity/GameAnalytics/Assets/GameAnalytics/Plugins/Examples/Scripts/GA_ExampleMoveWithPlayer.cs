using UnityEngine;
using System.Collections;

public class GA_ExampleMoveWithPlayer : MonoBehaviour
{
	public GameObject Player;
	
	void Update ()
	{
		if (Player != null)
		{
			transform.position = Player.transform.position + new Vector3(0, 0.5f, 0);
		}
	}
}
