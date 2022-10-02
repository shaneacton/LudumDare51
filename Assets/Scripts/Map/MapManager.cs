
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager: MonoBehaviour
{
    public static MapManager singleton;
    public MapDefinition mapDef;

    private void Awake()
    {
        singleton = this;
        mapDef.spawnTiles();
    }

    public static Vector3 getNextTargetTilePos(Enemy enemy, Vector3 target)
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
        return singleton.mapDef._tiles[nextTile.x, nextTile.y].transform.position;
    }
    
    public List<Node> findPath(Node start, Node end)
    {
        // Debug.Log("finding path from " + start + " to " + end);
        List<Node> path = AStar.Search(mapDef.starGrid, mapDef.starGrid.Grid[start.x, start.y], mapDef.starGrid.Grid[end.x, end.y]);
        
        return path;
    }

    public static Node getTileLocation(Vector3 worldPosition)
    {
        int i = (int)Math.Round(worldPosition.x + singleton.mapDef.numXTiles / 2);
        int j = (int)Math.Round(worldPosition.y + singleton.mapDef.numYTiles / 2);
        return new Node(i, j);
    }
}
