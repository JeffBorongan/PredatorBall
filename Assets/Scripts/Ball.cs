using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
	[HideInInspector] public Text leftSideScoreText, rightSideScoreText;
	[HideInInspector] public int leftSideScore, rightSideScore;
	public Rigidbody2D rigidBody2D;
	public float movementSpeed;

	void Awake()
    {
		leftSideScoreText = GameObject.FindGameObjectWithTag("Left Side Score Text").GetComponent<Text>();
		rightSideScoreText = GameObject.FindGameObjectWithTag("Right Side Score Text").GetComponent<Text>();

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
			rightSideScoreText.text = (++rightSideScore).ToString();
		}
		else if (other.gameObject.tag == "Right Side Goal")
		{
			print("Right Side Goal");
			leftSideScoreText.text = (++leftSideScore).ToString();
		}
	}
}