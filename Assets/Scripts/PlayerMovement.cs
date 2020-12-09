using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * Time.deltaTime * movementSpeed;
    }
}