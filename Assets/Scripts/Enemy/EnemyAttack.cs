using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackPeriod;
    private float timeSinceLastAttack = 0;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPos;

    void Update()
    {
        if (timeSinceLastAttack > attackPeriod)
        {
            // TODO: raycast
            var bullet = Instantiate(bulletPrefab,
                                     bulletSpawnPos.transform.position,
                                     transform.rotation
                                    );
            timeSinceLastAttack = 0;
        }

        timeSinceLastAttack += Time.deltaTime;
    }
}
