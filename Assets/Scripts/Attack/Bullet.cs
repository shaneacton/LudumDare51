using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] protected float _speed = 500;

    private void Start()
    {   
        GameManager.instance._bullets.Add(this);
        var rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * _speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy")) 
        {
            GameManager.instance.OnKillEnemy(other.gameObject);
        }
        else if (other.transform.CompareTag("Player"))
        {
            GameManager.instance.OnPlayerDead();
            // Destroy(other.gameObject);
        }

        GameManager.instance._bullets.Remove(this);
        Destroy(gameObject);
    }
}