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

    private void Start()
    {
        originalWarningIndicatorScale = warningIndicator.transform.localScale;
    }

    void Update()
    {
        if (timeSinceLastAttack > attackPeriod - 1)
        {
            warningIndicator.SetActive(true);
            warningIndicator.transform.localScale += new Vector3(warningIndicatorGrowSpeed, warningIndicatorGrowSpeed, warningIndicatorGrowSpeed);
        }

        if (timeSinceLastAttack > attackPeriod)
        {
            // TODO: raycast
            var bullet = Instantiate(bulletPrefab,
                                     bulletSpawnPos.transform.position,
                                     transform.rotation
                                    );
            timeSinceLastAttack = Random.Range(-1f, 1f);

            warningIndicator.SetActive(false);
            warningIndicator.transform.localScale = originalWarningIndicatorScale;
        }

        timeSinceLastAttack += Time.deltaTime;
    }
}
