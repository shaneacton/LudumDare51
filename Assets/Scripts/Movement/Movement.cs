using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private int PLAYER_LAYER;
    private int ENEMY_LAYER;
    private int GHOST_BULLET_LAYER;
    private int ENEMY_BULLET_LAYER;


    public float movementSpeed = 10f;
    public float dodgeSpeed = 50f;
    public float dodgeTime = 0.5f;
    private Vector3 _dodgeDir;

    Rigidbody2D rb;

    private MoveState _state;

    public Animator animator;

    private bool isMoving = false;

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
        ENEMY_LAYER = LayerMask.NameToLayer("EnemyBullet");
        ENEMY_LAYER = LayerMask.NameToLayer("PlayerBullet");
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.canMove) { return; }
        var inputs = GetInputs();

        if (_state == MoveState.Normal)
        {
            Move(inputs, movementSpeed);

            // if (Input.GetKey(KeyCode.Space)) { StartCoroutine(StartDodge(inputs)); }
        }
        // else if (_state == MoveState.Dodging) { Dodge(inputs); }

        if (isMoving == false && (inputs.x != 0 || inputs.y != 0))
        {
            isMoving = true;
            animator.SetBool("isMoving", true);
        }
        else if (isMoving == true && (inputs.x == 0 && inputs.y == 0))
        {
            isMoving = false;
            animator.SetBool("isMoving", false);
        }

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
        Vector3 pos = transform.position;
        Node tilePos = MapManager.getTileLocation(pos-new Vector3(0, 0.1f, 0));
        pos.z = 1 + (tilePos.y/(float)MapManager.singleton.mapDef.numYTiles);
        transform.position = pos;
    }

    // IEnumerator StartDodge(Vector2 inputs) // BROKEN!
    // {
    //     _state = MoveState.Dodging;
    //     _dodgeDir = inputs;
    //     Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, true);
    //     Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_BULLET_LAYER, true);
    //     Physics2D.IgnoreLayerCollision(PLAYER_LAYER, GHOST_BULLET_LAYER, true);

    //     yield return new WaitForSeconds(dodgeTime);

    //     _state = MoveState.Normal;

    //     yield return new WaitForSeconds(dodgeTime);
    //     // Extra invulnerable time
    //     Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, false);
    //     Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_BULLET_LAYER, false);
    //     Physics2D.IgnoreLayerCollision(PLAYER_LAYER, GHOST_BULLET_LAYER, false);

    //     // TODO 
    //     // Extended immunity time here with another wait for seconds (coyote time)?
    //     // Maybe should disable inputs for a bit longer, have like .01 seconds of downtime after dodge
    //     // Dodge cooldown so can't keep dodging
    // }

    // void Dodge(Vector2 inputs)
    // {
    //     Move(_dodgeDir, dodgeSpeed);
    // }
}
