using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
	public Rigidbody2D rigidBody2D;
    public Vector2 spawnLimits;
    public float movementSpeed = 150;
    public string BallTag = "ball";

    private void Start()
    {
        if (Random.value > 0.5f)
        {
            rigidBody2D.velocity = transform.right * Time.fixedDeltaTime * movementSpeed;
        }
        else
        {
            rigidBody2D.velocity = transform.right * Time.fixedDeltaTime * -movementSpeed;
        }

        print(movementSpeed);

        Physics.IgnoreLayerCollision(9, 10, true);
    }

    #region
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Line>() != null)
        {
            if (other.gameObject.GetComponent<Line>().lineTag == "GreenInk")
            {
                if (movementSpeed < 500)
                {
                    movementSpeed += 50;
                    rigidBody2D.velocity = transform.right * Time.fixedDeltaTime * movementSpeed;
                    print("Green " + movementSpeed);
                }

            }

            else if (other.gameObject.GetComponent<Line>().lineTag == "YellowInk")
            {
                print("Yellow " + movementSpeed);
            }

            else if (other.gameObject.GetComponent<Line>().lineTag == "RedInk")
            {
                if (movementSpeed > 150)
                {
                    movementSpeed -= 50;
                    rigidBody2D.velocity = transform.right * Time.fixedDeltaTime * movementSpeed;
                    print("Red " + movementSpeed);
                }
            }
        }

        if (other.gameObject.tag == "Left Side Goal")
        {
            rigidBody2D.velocity = new Vector2(0, 0);
            StartCoroutine(ResetBall());
        }

        else if (other.gameObject.tag == "Right Side Goal")
        {
            rigidBody2D.velocity = new Vector2(0, 0);
            StartCoroutine(ResetBall());
        }
    }
    #endregion

    private IEnumerator ResetBall()
    {
        yield return new WaitForSecondsRealtime(1);
        Vector2 spawnPosition = new Vector2(-0.13f, Random.Range(-spawnLimits.y, spawnLimits.y));
        gameObject.transform.position = spawnPosition;

        if (Random.value > 0.5f)
        {
            rigidBody2D.velocity = transform.right * Time.fixedDeltaTime * movementSpeed;
        }
        else
        {
            rigidBody2D.velocity = transform.right * Time.fixedDeltaTime * -movementSpeed;
        }
    }
}