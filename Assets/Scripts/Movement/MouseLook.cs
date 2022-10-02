using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MouseLook : MonoBehaviour
{

    Rigidbody2D rb;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float angle;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Look();
    }

    void Look()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);

        animator.SetFloat("direction", angle);
        // Debug.DrawLine(transform.position, mousePos, Color.white);

        transform.GetChild(0).transform.rotation = Quaternion.identity;
    }
}
