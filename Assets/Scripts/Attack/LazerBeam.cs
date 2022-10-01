using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBeam : MonoBehaviour
{

    public Lazer lazer;
    private bool disable = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (disable) return;

        lazer.IncreaseBeamSize();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Environment"))
        {
            Debug.Log("collision");
            disable = true;
            lazer.scaleFactor = 0;
            lazer.DisableBeam();
        }

    }
}
