using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float lifespanSeconds=5;
    private float startTime;
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

    private void pickUp()
    {
        Debug.Log("got coin");
        GameManager.incrementScore();
    }
}
