using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;

    public GameObject enemy;
    public GameObject player;
    public List<GameObject> SpawnPoints;
    public float TimeBetweenSpawns = 10;
    private float TimeSinceSpawned;

    [NonSerialized] public List<GameObject> enemies = new List<GameObject>();

    public int numEnemies = 2;

    private void Start()
    {
        instance = this;

        var playerSpawnPt = GameManager.instance.OnStart();
        SpawnPoints.Remove(playerSpawnPt.gameObject);
        SpawnEnemies();
        SpawnPoints.Add(playerSpawnPt.gameObject);
    }

    private void Update()
    {
        if (TimeSinceSpawned > TimeBetweenSpawns && GameManager.instance.alive)
        {
            var playerSpawnPt = GameManager.instance.OnReset();
            SpawnPoints.Remove(playerSpawnPt.gameObject);
            SpawnEnemies();
            SpawnPoints.Add(playerSpawnPt.gameObject);

            TimeSinceSpawned = 0;
        }
        else
        {
            TimeSinceSpawned += Time.deltaTime;
        }

    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            var spawnPtIdx = UnityEngine.Random.Range(0, SpawnPoints.Count);
            var spawnPt = SpawnPoints[spawnPtIdx].transform;
            var enemyGO = Instantiate(enemy, spawnPt.position, Quaternion.identity, spawnPt);

            enemies.Add(enemyGO);
        }

    }

    public Transform getNearestSpawnPoint(GameObject gameObject)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = gameObject.transform.position;
        foreach (GameObject spawn in SpawnPoints)
        {
            Vector3 spawnPos = spawn.transform.position;
            float dist = Vector3.Distance(spawnPos, currentPos);
            if (dist < minDist)
            {
                tMin = spawn.transform;
                minDist = dist;
            }
        }
        return tMin;
    }
}