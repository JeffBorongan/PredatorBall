using UnityEngine;

public class Ball : MonoBehaviour
{
	public Rigidbody2D rigidBody2D;
	public float movementSpeed;

    void Awake()
    {
		if (Random.value > 0.5f)
		{
			rigidBody2D.velocity = transform.right * Time.deltaTime * movementSpeed;
		}
		else
		{
			rigidBody2D.velocity = transform.right * Time.deltaTime * -movementSpeed;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Red Ink")
		{
			movementSpeed += 20;
			rigidBody2D.velocity = transform.right * Time.deltaTime * movementSpeed;
			print(movementSpeed);
		}
		else if (other.gameObject.tag == "Blue Ink")
		{
			movementSpeed -= 20;
			rigidBody2D.velocity = transform.right * Time.deltaTime * movementSpeed;
			print(movementSpeed);
		}
		else if (other.gameObject.tag == "Yellow Ink")
		{
			print("Yellow Ink");
		}
		else if (other.gameObject.tag == "Left Side Goal")
		{
			print("Left Side Goal");
		}
		else if (other.gameObject.tag == "Right Side Goal")
		{
			print("Right Side Goal");
		}
	}
}