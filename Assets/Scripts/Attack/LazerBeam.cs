using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBeam : MonoBehaviour
{

    public Lazer lazer;
    private bool disable = false;

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
            disable = true;
            lazer.scaleFactor = 0;
            lazer.DisableBeam();
        }
        if (other.transform.CompareTag("Enemy")) 
        {
            GameManager.instance.OnKillEnemy(other.gameObject);
        }
        else if (other.transform.CompareTag("Player"))
        {
            GameManager.instance.OnPlayerDead();
            // Destroy(other.gameObject);
        }
    }
}
