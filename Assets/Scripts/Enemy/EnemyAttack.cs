using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (timeSinceLastAttack > attackPeriod - 1)
        {
            warningIndicator.SetActive(true);
            warningIndicator.transform.localScale += new Vector3(warningIndicatorGrowSpeed, warningIndicatorGrowSpeed, warningIndicatorGrowSpeed);
        }

        if (timeSinceLastAttack > attackPeriod)
        {// has waited long enough to shoot
            if (Time.time - selfEnemy.lastSeenPlayerTime <= 1f)
            { // has seen player within th last 1 second
                var bullet = Instantiate(bulletPrefab,
                    bulletSpawnPos.transform.position,
                    transform.rotation
                );
                timeSinceLastAttack = Random.Range(-1f, 1f);

                warningIndicator.SetActive(false);
                warningIndicator.transform.localScale = originalWarningIndicatorScale; 
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
