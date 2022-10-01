using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;

    public GameObject enemy;
    public GameObject player;
    public List<GameObject> SpawnPoints;
    public long spawnTime = 10;
    [System.NonSerialized]
    public long spawnStartTime;
    public long breakTime = 3;
    [System.NonSerialized]
    public long breakStartTime;

    [NonSerialized] public List<GameObject> enemies = new List<GameObject>();

    public int numEnemies = 2;

    public GameObject loopTimer;
    public GameObject breakTimer;

    private void Awake()
    {
        instance = this;
        // var playerSpawnPt = GameManager.instance.OnStart();
        // SpawnPoints.Remove(playerSpawnPt.gameObject);
        // SpawnEnemies();
        // SpawnPoints.Add(playerSpawnPt.gameObject);

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop(){
        
        if (!GameManager.instance.alive) { StopCoroutine(SpawnLoop()); }
        
        breakStartTime = GameManager.getEpochTime();

        yield return new WaitForSeconds(breakTime);

        breakTimer.GetComponent<BreakTimer>().Hide();
        loopTimer.GetComponent<LoopTimer>().Show();

        spawnStartTime = GameManager.getEpochTime();

        SpawnEnemies();

        yield return new WaitForSeconds(spawnTime);

        loopTimer.GetComponent<LoopTimer>().Hide();

        var playerSpawnPt = GameManager.instance.OnReset();
        SpawnPoints.Remove(playerSpawnPt.gameObject);
        SpawnPoints.Add(playerSpawnPt.gameObject);

        breakTimer.GetComponent<BreakTimer>().Show();

        StartCoroutine(SpawnLoop());
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