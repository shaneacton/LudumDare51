using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{

    public float movementSpeed = 10f;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Vector2 GetInputs()
    {
        // GetAxix (non-raw) makes for very unresponsive movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        return new Vector2(horizontalInput, verticalInput);
    }

    void Move(Vector2 inputs)
    {
        Vector2 currentPos = rb.position;

        Vector2 movement = inputs.normalized * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;

        rb.MovePosition(newPos);
    }

    void FixedUpdate()
    {
        var inputs = GetInputs();
        Move(inputs);
    }
}
