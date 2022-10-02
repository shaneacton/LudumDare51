
using UnityEngine;

public abstract class PickUp: MonoBehaviour
{
    public float lifespanSeconds=5;
    public float flashingSeconds=2.5f;
    public float flashFrequency=3;
    private float startTime;
    private Renderer _renderer;

    public abstract void pickUp();

    private void Start()
    {
        startTime = Time.time;
        _renderer = GetComponent<Renderer>();
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
            _renderer.material.SetColor("_Color", new Color(1,1,0,y));
            _renderer.material.SetColor("_EmissionColor", new Color(1,1,0,y));
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
