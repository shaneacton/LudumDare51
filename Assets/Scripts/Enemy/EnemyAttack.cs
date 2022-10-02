using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttack : MonoBehaviour
{
    public float attackPeriod;
    private float timeSinceLastAttack = 0;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPos;

    public GameObject warningIndicator;
    private Vector3 originalWarningIndicatorScale;
    public float warningIndicatorGrowSpeed = 0.001f;

    private Enemy selfEnemy;
    private void Start()
    {
        originalWarningIndicatorScale = warningIndicator.transform.localScale;
        selfEnemy = GetComponent<Enemy>();
    }

    void Update()
    {
        bool waitedLongEnoughToShoot = timeSinceLastAttack > attackPeriod;
        bool aboutToBeAbleToShoot = timeSinceLastAttack > attackPeriod - 1;
        bool hasSeenPlayerRecently = Time.time - selfEnemy.lastSeenPlayerTime <= 1f;

        if (aboutToBeAbleToShoot)
        { // is about to be allowed to shoot
            if (hasSeenPlayerRecently)
            {
                warningIndicator.SetActive(true);
                float timeTillShootReady = attackPeriod - timeSinceLastAttack;
                timeTillShootReady = Math.Max(0, timeTillShootReady);
                if(timeTillShootReady > 1){throw new Exception("");}
                warningIndicator.transform.localScale = originalWarningIndicatorScale * (1-timeTillShootReady);
            }
            else
            {
                warningIndicator.SetActive(false);
            }
        }

        if (waitedLongEnoughToShoot)
        {// has waited long enough to shoot
            if (hasSeenPlayerRecently)
            { // has seen player within th last 1 second
                var bullet = Instantiate(bulletPrefab,
                    bulletSpawnPos.transform.position,
                    transform.rotation
                );
                AudioManager.Play("EnemyPistol");
                timeSinceLastAttack = Random.Range(-1f, 1f);
                warningIndicator.SetActive(false);
            }
            else
            { // player has recently droped out of view. Don't shoot
                //wait a little longer to shoot after seeing player again
                timeSinceLastAttack = attackPeriod / 2f;
            }

        }

        timeSinceLastAttack += Time.deltaTime;
    }
}
