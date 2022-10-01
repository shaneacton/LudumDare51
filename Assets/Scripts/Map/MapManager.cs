
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager: MonoBehaviour
{
    public static MapManager singleton;
    public int numTiles = 20; // must be even

    private GridGraph starGrid;
    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        starGrid = new GridGraph(numTiles, numTiles);
        for (int i = 0; i < numTiles; i++)
        {
            for (int j = 0; j < numTiles; j++)
            {
                if (MapSpawner.singleton.obstacles[i, j])
                {
                    starGrid.Walls.Add(new Vector2(i,j));
                    // Debug.Log(i + "," + j + " is an obstacle. Adding wall: " + new Vector2(i,j));
                }
            }
        }
    }

    public static Vector3 getNextTarget(Enemy enemy, Vector3 target)
    {
        Node targetTile = getTileLocation(target);
        Node enemyTile = getTileLocation(enemy.transform.position);
        if (Equals(targetTile, enemyTile))
        {
            return target;
        }
        // Debug.Log("using A star. Enemy tile: " + enemyTile + " target tile: " + targetTile);

        List<Node> path = singleton.findPath(enemyTile, targetTile);
        if (path.Count == 0)
        {
            throw new Exception("no path from " + enemyTile + " to " + targetTile);
        }
        Node nextTile = path[0];
        // Debug.Log("next tile: " + nextTile);
        return MapSpawner.singleton.tiles[nextTile.x, nextTile.y].transform.position;
    }
    
    public List<Node> findPath(Node start, Node end)
    {
        // Debug.Log("finding path from " + start + " to " + end);
        List<Node> path = AStar.Search(starGrid, starGrid.Grid[start.x, start.y], starGrid.Grid[end.x, end.y]);
        
        return path;
    }

    public static Node getTileLocation(Vector3 worldPosition)
    {
        int i = (int)Math.Round(worldPosition.x + singleton.numTiles / 2);
        int j = (int)Math.Round(worldPosition.y + singleton.numTiles / 2);
        return new Node(i, j);
    }
}
