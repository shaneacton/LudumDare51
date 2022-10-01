using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;

    public GameObject enemy;
    public GameObject player;
    public List<GameObject> SpawnPoints;
    public float TimeBetweenSpawns = 10;
    private float TimeSinceSpawned = 0f;

    private void Awake(){
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
            GameManager.instance.OnReset();
        }
        else
        {
            TimeSinceSpawned += Time.deltaTime;
        }

    }

    public Transform getNearestSpawnPoint(GameObject gameObject){

        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = gameObject.transform.position;
        foreach (GameObject spawn in SpawnPoints)
        {
            Transform spawnPos = spawn.transform.position;
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