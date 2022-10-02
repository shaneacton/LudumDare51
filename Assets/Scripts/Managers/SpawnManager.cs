using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;

    public GameObject enemy;
    public GameObject player;
    public List<Tile> SpawnPoints;
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

    public float PreWarnEnemySpawnTime;

    public enum State
    {
        Break,
        Wave
    };

    public State _state;

    private void Awake()
    {
        instance = this;
        Vector2Int[] spawnPositions = MapManager.singleton.mapDef.getSpawnPoints();
        SpawnPoints = new List<Tile>();
        foreach (Vector2Int spawnPos in spawnPositions)
        {
            SpawnPoints.Add(MapManager.singleton.mapDef._tiles[spawnPos.x, spawnPos.y]);
        }
        StartCoroutine(StartSpawnLoop());
    }

    IEnumerator StartSpawnLoop()
    {
        _state = State.Break;
        // GameManager.instance.canMove = false;
        GameManager.instance.DisablePlayerMovement();
        var playerSpawnPt = GameManager.instance.OnStart();

        breakStartTime = GameManager.getEpochTime();

        yield return new WaitForSeconds(breakTime);

        _state = State.Wave;

        GameManager.instance.onBreakEnd();
        // GameManager.instance.canMove = true;
        GameManager.instance.EnablePlayerMovement();

        breakTimer.GetComponent<BreakTimer>().Hide();
        loopTimer.GetComponent<LoopTimer>().Show();

        spawnStartTime = GameManager.getEpochTime();

        int numWaveZero = (int)Mathf.Ceil(numEnemies / 3f);
        int numWaveOne = (int)Mathf.Ceil((numEnemies - numWaveZero) / 2f);
        int numWaveTwo = numEnemies - numWaveOne - numWaveZero;

        float waveZeroTime = UnityEngine.Random.Range(2.5f, 3.5f);
        float waveOneTime = UnityEngine.Random.Range(2.5f, 3.5f);

        SpawnPoints.Remove(playerSpawnPt);
        StartCoroutine(SpawnEnemies(numWaveZero));
        SpawnPoints.Add(playerSpawnPt);

        yield return new WaitForSeconds(waveZeroTime - PreWarnEnemySpawnTime);
        StartCoroutine(SpawnEnemies(numWaveOne));
        yield return new WaitForSeconds(waveOneTime - PreWarnEnemySpawnTime);
        StartCoroutine(SpawnEnemies(numWaveTwo));
        yield return new WaitForSeconds(10 - waveOneTime - waveZeroTime);

        loopTimer.GetComponent<LoopTimer>().Hide();
        breakTimer.GetComponent<BreakTimer>().Show();

        numEnemies += 1;

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        if (!GameManager.instance.alive) { StopCoroutine(SpawnLoop()); }

        _state = State.Break;
        // GameManager.instance.canMove = false;
        GameManager.instance.DisablePlayerMovement();

        DestroyAllEnemies();

        Tile playerSpawnPt = GameManager.instance.OnReset();

        breakStartTime = GameManager.getEpochTime();
        yield return new WaitForSeconds(breakTime);

        _state = State.Wave;

        GameManager.instance.onBreakEnd();

        GameManager.instance.EnablePlayerMovement();

        breakTimer.GetComponent<BreakTimer>().Hide();
        loopTimer.GetComponent<LoopTimer>().Show();

        spawnStartTime = GameManager.getEpochTime();

        int numWaveZero = (int)Mathf.Ceil(numEnemies / 3f);
        int numWaveOne = (int)Mathf.Ceil((numEnemies - numWaveZero) / 2f);
        int numWaveTwo = numEnemies - numWaveOne - numWaveZero;

        float waveZeroTime = UnityEngine.Random.Range(2.5f, 3.5f);
        float waveOneTime = UnityEngine.Random.Range(2.5f, 3.5f);

        SpawnPoints.Remove(playerSpawnPt);
        StartCoroutine(SpawnEnemies(numWaveZero));
        SpawnPoints.Add(playerSpawnPt);

        yield return new WaitForSeconds(waveZeroTime - PreWarnEnemySpawnTime);
        StartCoroutine(SpawnEnemies(numWaveOne));
        yield return new WaitForSeconds(waveOneTime - PreWarnEnemySpawnTime);
        StartCoroutine(SpawnEnemies(numWaveTwo));
        yield return new WaitForSeconds(10 - waveOneTime - waveZeroTime);

        loopTimer.GetComponent<LoopTimer>().Hide();
        breakTimer.GetComponent<BreakTimer>().Show();

        numEnemies += 1;

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnEnemies(int n)
    {
        List<Transform> transforms = new List<Transform>();

        for (int i = 0; i < n; i++)
        {
            var spawnPtIdx = UnityEngine.Random.Range(0, SpawnPoints.Count);
            var spawnPt = SpawnPoints[spawnPtIdx].transform;
            transforms.Add(spawnPt);

            // TODO Play spawn effect
            spawnPt.GetComponent<SpawnTile>().EnemySpawnVFX.Play();
        }

        yield return new WaitForSeconds(PreWarnEnemySpawnTime);

        foreach (var spawnPt in transforms)
        {
            spawnPt.GetComponent<SpawnTile>().EnemySpawnVFX.Stop();
            var pos = spawnPt.position;
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

    public Tile getNearestSpawnPoint(GameObject gameObject)
    {
        Tile tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = gameObject.transform.position;
        // currentPos.z = 0;
        foreach (Tile spawn in SpawnPoints)
        {
            Vector3 spawnPos = spawn.transform.position;
            // spawnPos.z = 0;
            float dist = Vector3.Distance(spawnPos, currentPos);
            if (dist < minDist)
            {
                tMin = spawn;
                minDist = dist;
            }
        }
        return tMin;
    }
}