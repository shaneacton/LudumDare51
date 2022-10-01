using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public static MapSpawner singleton;
    
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;

    public static float obstacleChance = 0.2f; // 0.2

    public bool[,] obstacles;
    public Tile[,] tiles;
    private void Awake()
    {
        singleton = this;
        obstacles = new bool[MapManager.singleton.numTiles, MapManager.singleton.numTiles];
        tiles = new Tile[MapManager.singleton.numTiles, MapManager.singleton.numTiles];
        for (int i = 0; i < MapManager.singleton.numTiles; i++)
        {
            for (int j = 0; j < MapManager.singleton.numTiles; j++)
            {
                bool obstacle = Random.Range(0f, 1f) < obstacleChance;
                spawnTile(i, j, obstacle);
            }
        }
    }

    void spawnTile(int i, int j, bool obstacle)
    {
        int x = i - MapManager.singleton.numTiles / 2;
        int y = j - MapManager.singleton.numTiles / 2;
        Vector3 pos = Vector3.right * x + Vector3.up * y;
        GameObject tileObj;
        if (obstacle)
        {
            tileObj = Instantiate(obstaclePrefab, pos, transform.rotation);
        }
        else
        {
            tileObj = Instantiate(tilePrefab, pos, transform.rotation);
        }

        Tile tile = tileObj.GetComponent<Tile>();
        tile.X = i;
        tile.Y = j;

        tiles[i, j] = tile;
        obstacles[i, j] = obstacle;
    }
}
