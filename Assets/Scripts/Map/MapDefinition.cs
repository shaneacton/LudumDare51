
using System.Linq;
using UnityEngine;

public abstract class MapDefinition: MonoBehaviour
{
    public GameObject floorTilePrefab;
    public GameObject obstaclePrefab;
    public GameObject spawnTilePrefab;
    
    public int numXTiles = 20; // must be even
    public int numYTiles = 10; // must be even
    private GridGraph _starGrid;
    private bool[,] _obstacles;
    public Tile[,] _tiles;
    public GridGraph starGrid {
        get {
            if (_starGrid == null) { _starGrid = getStarGrid(); }
            return _starGrid; 
        }}

    public bool[,] obstacles
    {
        get
        {
            if (_obstacles == null) { _obstacles = getObstacleMatrix(); }
            return _obstacles; 
        }
    }

    public abstract bool[,] getObstacleMatrix();
    public abstract Vector2Int[] getSpawnPoints();

    public void spawnTiles()
    {
        Vector2Int[] spawnPositions = getSpawnPoints();
        for (int i = 0; i < numXTiles; i++)
        {
            for (int j = 0; j < numYTiles; j++)
            {
                bool spawnPoint = spawnPositions.Contains(new Vector2Int(i, j));
                spawnTile(i, j, obstacles[i, j], spawnPoint);
            }
        }
    }

    private GridGraph getStarGrid()
    {
        GridGraph grid = new GridGraph(numXTiles, numYTiles);
        for (int i = 0; i < numXTiles; i++)
        {
            for (int j = 0; j < numYTiles; j++)
            {
                if (_obstacles[i, j])
                {
                    grid.Walls.Add(new Vector2(i,j));
                    // Debug.Log(i + "," + j + " is an obstacle. Adding wall: " + new Vector2(i,j));
                }
            }
        }
        return grid;
    }

    void spawnTile(int i, int j, bool obstacle, bool spawnPoint)
    {
        if (_tiles == null)
        {
            _tiles = new Tile[numXTiles, numYTiles];
        }
        int x = i - numXTiles / 2;
        int y = j - numYTiles / 2;
        Vector3 pos = Vector3.right * x + Vector3.up * y;
        pos.z = 2;
        GameObject tilePrefab;
        if (obstacle)
        {
            tilePrefab = obstaclePrefab;
        }
        else
        {
            if (spawnPoint)
            {
                tilePrefab = spawnTilePrefab;
            }
            else
            {
                tilePrefab = floorTilePrefab;
            }
        }
        GameObject tileObj = Instantiate(tilePrefab, pos, transform.rotation);

        Tile tile = tileObj.GetComponent<Tile>();
        tile.X = i;
        tile.Y = j;

        _tiles[i, j] = tile;
    }
}
