using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 500;
    private void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * _speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy")) 
        {
            Destroy(other.gameObject);
            GameManager.instance.OnKillEnemy();
        }
        else if (other.transform.CompareTag("Player"))
        {
            GameManager.instance.OnPlayerDead();
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}