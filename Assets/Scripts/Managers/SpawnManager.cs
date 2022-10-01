using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
         Vector2Int[] spawnPositions = MapManager.singleton.mapDef.getSpawnPoints();
         SpawnPoints = new List<GameObject>();
         foreach (Vector2Int spawnPos in spawnPositions)
         {
             SpawnPoints.Add(MapManager.singleton.mapDef._tiles[spawnPos.x, spawnPos.y].gameObject);
         }
        StartCoroutine(StartSpawnLoop());
    }

    IEnumerator StartSpawnLoop()
    {
        // GameManager.instance.canMove = false;
        GameManager.instance.DisablePlayerMovement();
        var playerSpawnPt = GameManager.instance.OnStart();

        breakStartTime = GameManager.getEpochTime();

        yield return new WaitForSeconds(breakTime);

        GameManager.instance.onBreakEnd();
        // GameManager.instance.canMove = true;
        GameManager.instance.EnablePlayerMovement();

        breakTimer.GetComponent<BreakTimer>().Hide();
        loopTimer.GetComponent<LoopTimer>().Show();

        spawnStartTime = GameManager.getEpochTime();

        SpawnPoints.Remove(playerSpawnPt.gameObject);
        SpawnEnemies();
        SpawnPoints.Add(playerSpawnPt.gameObject);

        yield return new WaitForSeconds(spawnTime);

        loopTimer.GetComponent<LoopTimer>().Hide();
        breakTimer.GetComponent<BreakTimer>().Show();

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        if (!GameManager.instance.alive) { StopCoroutine(SpawnLoop()); }

        // GameManager.instance.canMove = false;
        GameManager.instance.DisablePlayerMovement();

        DestroyAllEnemies();
        
        var playerSpawnPt = GameManager.instance.OnReset();

        breakStartTime = GameManager.getEpochTime();
        yield return new WaitForSeconds(breakTime);

        GameManager.instance.onBreakEnd();

        // GameManager.instance.canMove = true;
        GameManager.instance.EnablePlayerMovement();

        breakTimer.GetComponent<BreakTimer>().Hide();
        loopTimer.GetComponent<LoopTimer>().Show();

        spawnStartTime = GameManager.getEpochTime();

        SpawnPoints.Remove(playerSpawnPt.gameObject);
        SpawnEnemies();
        SpawnPoints.Add(playerSpawnPt.gameObject);

        yield return new WaitForSeconds(spawnTime);

        loopTimer.GetComponent<LoopTimer>().Hide();
        breakTimer.GetComponent<BreakTimer>().Show();

        StartCoroutine(SpawnLoop());
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            var spawnPtIdx = UnityEngine.Random.Range(0, SpawnPoints.Count);
            var spawnPt = SpawnPoints[spawnPtIdx].transform;
            Vector3 pos = spawnPt.position;
            pos.z = 0;
            var enemyGO = Instantiate(enemy, pos, Quaternion.identity, spawnPt);

            enemies.Add(enemyGO);
        }

    }

    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    public Transform getNearestSpawnPoint(GameObject gameObject)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = gameObject.transform.position;
        // currentPos.z = 0;
        foreach (GameObject spawn in SpawnPoints)
        {
            Vector3 spawnPos = spawn.transform.position;
            // spawnPos.z = 0;
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