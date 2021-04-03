using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ball : MonoBehaviour
{
	[HideInInspector] public Text leftSideScoreText, rightSideScoreText;
	[HideInInspector] public int leftSideScore, rightSideScore;
	public Rigidbody2D rigidBody2D;
	private float movementSpeed = 150;

    private void Start()
    {
        rigidBody2D.velocity = transform.right * Time.fixedDeltaTime * movementSpeed;
        print(movementSpeed);
    }

    #region
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Line>().LineTag == "GreenInk")
        {
            if(movementSpeed < 500)
            {
                movementSpeed += 50;
                rigidBody2D.velocity = transform.right * Time.fixedDeltaTime * movementSpeed;
                print("Green" + movementSpeed);
            }
            
        }
        else if (other.gameObject.GetComponent<Line>().LineTag == "YellowInk")
        {
            print("Yellow" + movementSpeed);
        }
        else if (other.gameObject.GetComponent<Line>().LineTag == "RedInk")
        {
            if(movementSpeed > 150)
            {
                movementSpeed -= 50;
                rigidBody2D.velocity = transform.right * Time.fixedDeltaTime * movementSpeed;
                print("Red" + movementSpeed);
            }
            
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
    #endregion
}