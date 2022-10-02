
using UnityEngine;

public abstract class PickUp: MonoBehaviour
{
    public float lifespanSeconds=5;
    public float flashingSeconds=2.5f;
    public float flashFrequency=3;
    private float startTime;
    public Renderer renderer;

    public abstract void pickUp();
    public abstract void playSound();

    private void Start()
    {
        startTime = Time.time;
    }
    
    private void Update()
    {
        float timeSinceSpawn = Time.time - startTime;
        if (timeSinceSpawn > lifespanSeconds)
        { // alive longer than lifetime
            Destroy(gameObject);
        }
        else if(timeSinceSpawn > lifespanSeconds-flashingSeconds)
        {
            float deltT = timeSinceSpawn - lifespanSeconds + flashingSeconds;
            float flashTimeLeft = (lifespanSeconds - timeSinceSpawn) / flashingSeconds;
            float x = deltT * flashFrequency  / flashTimeLeft ;
            float y = (Mathf.Cos(x) +1) * 0.5f;
            // Debug.Log("t: " + deltT + " x: " + x + " y: " + y);
            renderer.material.SetColor("_Color", new Color(1f,1f,1f,y));
            renderer.material.SetColor("_EmissionColor", new Color(1f,1f,1f,y));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            pickUp();
            playSound();
            Destroy(gameObject);
        }
    }
}
