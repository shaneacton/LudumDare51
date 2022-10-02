
using UnityEngine;

public class OutterBeam: MonoBehaviour
{
    public Lazer lazer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy")) 
        {
            GameManager.instance.OnKillEnemy(other.gameObject);
        }
    }
}
