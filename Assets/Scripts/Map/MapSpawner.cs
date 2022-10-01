using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;
    public int numTiles = 10;

    private bool[,] obstacles;
    private void Awake()
    {
        obstacles = new bool[numTiles, numTiles];
        for (int i = 0; i < numTiles; i++)
        {
            for (int j = 0; j < numTiles; j++)
            {
                bool obstacle = Random.Range(0f, 1f) > 0.2f;
                spawnTile(i, j, obstacle);
            }
        }
    }

    void spawnTile(int i, int j, bool obstacle)
    {
        i -= numTiles / 2;
        j -= numTiles / 2;
        Vector3 pos = Vector3.up * i + Vector3.right * j;
        if (obstacle)
        {
            Instantiate(tilePrefab, pos, transform.rotation);
        }
        else
        {
            Instantiate(obstaclePrefab, pos, transform.rotation);
        }
    }
}
