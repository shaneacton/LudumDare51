using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private int PLAYER_LAYER;
    private int ENEMY_LAYER;

    public float movementSpeed = 10f;
    public float dodgeSpeed = 50f;
    public float dodgeTime = 0.5f;
    private Vector3 _dodgeDir;

    Rigidbody2D rb;

    private MoveState _state;
    private enum MoveState
    {
        Normal,
        Dodging
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _state = MoveState.Normal;

        PLAYER_LAYER = LayerMask.NameToLayer("Player");
        ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
    }

    void FixedUpdate()
    {
        if(!GameManager.instance.canMove){return;}
        var inputs = GetInputs();

        if (_state == MoveState.Normal)
        {
            Move(inputs, movementSpeed);

            if (Input.GetKey(KeyCode.Space)) { StartCoroutine(StartDodge(inputs)); }
        }
        else if (_state == MoveState.Dodging) { Dodge(inputs); }
    }

    Vector2 GetInputs()
    {
        // GetAxix (non-raw) makes for very unresponsive movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        return new Vector2(horizontalInput, verticalInput);
    }

    void Move(Vector2 inputs, float speed)
    {
        Vector2 currentPos = rb.position;

        Vector2 movement = inputs.normalized * speed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;

        rb.MovePosition(newPos);
    }

    IEnumerator StartDodge(Vector2 inputs)
    {
        _state = MoveState.Dodging;
        _dodgeDir = inputs;
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, true);

        yield return new WaitForSeconds(dodgeTime);

        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, false);
        _state = MoveState.Normal;

        // TODO 
        // Extended immunity time here with another wait for seconds (coyote time)?
        // Maybe should disable inputs for a bit longer, have like .01 seconds of downtime after dodge
        // Dodge cooldown so can't keep dodging
    }

    void Dodge(Vector2 inputs)
    {
        Move(_dodgeDir, dodgeSpeed);
    }
}
