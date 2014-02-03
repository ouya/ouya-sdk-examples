using UnityEngine;
using System.Collections;

public class GA_ExampleFloor : MonoBehaviour
{
	void Start ()
	{
		renderer.material.color = Color.red;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name.Equals("Ball"))
		{
			GA_ExampleHighScore.GameOver(collision.transform.position);
			
			Destroy(collision.gameObject);
		}
	}
}
