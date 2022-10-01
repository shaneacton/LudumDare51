using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Lazer : MonoBehaviour
{
    [SerializeField] private float _speed = 500;

    private float x_scale;
    public float scaleFactor = 5;
    public bool freeze = false;
    public float timeout = 5;
    private void Start()
    {
        //var rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(transform.right*(scaleFactor/2));

        x_scale = transform.localScale.x;
    }

    private void Update()
    {
        if (!freeze)
        {

            x_scale += scaleFactor/100;
            transform.localScale = new Vector3(x_scale,
                                                transform.localScale.y,
                                                transform.localScale.z);
        }
        else
        {
            var i = 0;
            while(i < timeout)
            {
                i++;
            }
            Destroy(gameObject);
        }

    }
}
