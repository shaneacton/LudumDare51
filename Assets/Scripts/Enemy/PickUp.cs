
using UnityEngine;

public abstract class PickUp: MonoBehaviour
{
    public float lifespanSeconds=5;
    private float startTime;

    public abstract void pickUp();

    private void Start()
    {
        startTime = Time.time;
    }
    
    private void Update()
    {
        if (Time.time - startTime > lifespanSeconds)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            pickUp();
            Destroy(gameObject);
        }
    }
}
