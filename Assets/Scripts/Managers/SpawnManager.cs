using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public List<GameObject> SpawnPoints;
    public float TimeBetweenSpawns = 10;
    private float TimeSinceSpawned = 0f;


    private void Start() {
        int spawnPtIdx = Random.Range(0, SpawnPoints.Count);
        var spawnPt = SpawnPoints[spawnPtIdx].transform;
        Instantiate(player, spawnPt.position, Quaternion.identity, spawnPt);

        SpawnPoints.RemoveAt(spawnPtIdx);
    }

    private void Update()
    {
        if (TimeSinceSpawned > TimeBetweenSpawns && GameManager.instance.alive)
        {
            var spawnPtIdx = Random.Range(0, SpawnPoints.Count);
            var spawnPt = SpawnPoints[spawnPtIdx].transform;

            Instantiate(enemy, spawnPt.position, Quaternion.identity, spawnPt);

            TimeSinceSpawned = 0;
        }
        else
        {
            TimeSinceSpawned += Time.deltaTime;
        }
    }
}