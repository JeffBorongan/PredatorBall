using UnityEngine;

public class Ball : MonoBehaviour
{
    public float movementSpeed = 200;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * Time.deltaTime * movementSpeed;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Red Ink")
		{
			movementSpeed += 20;
			rb.velocity = transform.right * Time.deltaTime * movementSpeed;
			print(movementSpeed);
		}
		else if (other.gameObject.tag == "Blue Ink")
		{
			movementSpeed -= 20;
			rb.velocity = transform.right * Time.deltaTime * movementSpeed;
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