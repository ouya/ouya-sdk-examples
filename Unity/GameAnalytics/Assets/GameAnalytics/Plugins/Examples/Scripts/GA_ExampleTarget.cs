using UnityEngine;
using System.Collections;

public class GA_ExampleTarget : MonoBehaviour
{
	public float SpinSpeed = 0.5f;
	
	private bool _hit = false;
	
	void Start ()
	{
		renderer.material.color = Color.yellow;
	}
	
	void Update ()
	{
		if (!_hit)
		{
			rigidbody.AddTorque(Vector3.forward * SpinSpeed);
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name.Equals("Ball") && !_hit)
		{
			GA_ExampleHighScore.AddScore(10, gameObject.name, collision.transform.position);
			
			StartCoroutine(FlashColor(0.5f));
		}
	}
	
	IEnumerator FlashColor(float duration)
	{
		_hit = true;
		renderer.material.color = Color.green;
		
		yield return new WaitForSeconds(duration);
		
		_hit = false;
		renderer.material.color = Color.yellow;
	}
}
