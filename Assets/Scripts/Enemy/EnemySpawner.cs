using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public List<GameObject> SpawnPoints;
    public float TimeBetweenSpawns;
    public float TimeBetweenDecay;

    private float TimeSinceSpawned = 0f;

    private void Update()
    {
        if (TimeSinceSpawned > TimeBetweenSpawns && GameManager.instance.alive)
        {
            var spawnPtIdx = Random.Range(0, SpawnPoints.Count);
            var spawnPt = SpawnPoints[spawnPtIdx].transform;

            Instantiate(enemy, spawnPt.position, Quaternion.identity, spawnPt);

            TimeSinceSpawned = 0;
            TimeBetweenSpawns *= TimeBetweenDecay;
        }
        else
        {
            TimeSinceSpawned += Time.deltaTime;
        }
    }
}