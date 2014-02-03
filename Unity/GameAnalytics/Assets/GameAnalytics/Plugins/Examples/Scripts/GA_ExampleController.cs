using UnityEngine;
using System.Collections;

public class GA_ExampleController : MonoBehaviour
{
	public float Speed = 1.0f;
	public float MaxVelocityChange = 10;
	
	void Update ()
	{
		Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		targetVelocity *= Speed;
		
		// Apply a force that attempts to reach our target velocity
		Vector3 velocity = rigidbody.velocity;
		Vector3 velocityChange = (targetVelocity - velocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -MaxVelocityChange, MaxVelocityChange);
		velocityChange.y = 0;
		velocityChange.z = 0;
		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
	}
}
