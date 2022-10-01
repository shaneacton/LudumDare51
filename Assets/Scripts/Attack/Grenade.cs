using System.Collections;
using UnityEngine;

public class Grenade : Bullet
{
    public float timeTillExplode = 1;
    public float explosionTime = 0.5f;

    private void Start()
    {
        this._speed = 0;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        var collider = GetComponent<Collider2D>();
        collider.enabled = false;

        yield return new WaitForSeconds(timeTillExplode);

        collider.enabled = true;

        yield return new WaitForSeconds(explosionTime);

        Destroy(gameObject);
    }
}