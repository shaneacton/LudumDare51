
using System;
using System.Linq;
using UnityEngine;

public class MainMap: MapDefinition
{

    [NonSerialized]
    public Vector2Int[] obstacles =
    {
        new Vector2Int(3, 1), new Vector2Int(3, 2), new Vector2Int(3,3), new Vector2Int(4,3),
        new Vector2Int(3, 6), new Vector2Int(4, 6),
        new Vector2Int(7, 3), new Vector2Int(8, 3), new Vector2Int(9, 3), new Vector2Int(10, 3),
        new Vector2Int(7, 4), new Vector2Int(10, 4), new Vector2Int(7, 5), new Vector2Int(10, 5),
        new Vector2Int(16,3), new Vector2Int(17,3), new Vector2Int(18,3),
        new Vector2Int(15, 6), new Vector2Int(15, 7), new Vector2Int(15, 8), new Vector2Int(16, 6)
    };
    
    [NonSerialized]
    public Vector2Int[] spawnPoints =
    {
        new Vector2Int(2,2), new Vector2Int(17,2), new Vector2Int(8,4), new Vector2Int(7,7),
        new Vector2Int(17,8)
    };


    public override bool[,] getObstacleMatrix()
    {
        bool[,] matrix = new bool[numXTiles, numYTiles];
        for (int i = 0; i < numXTiles; i++)
        {
            for (int j = 0; j < numYTiles; j++)
            {
                matrix[i, j] = obstacles.Contains(new Vector2Int(i, j));
                matrix[i, j] = matrix[i, j] || (i == 0 || j == 0) ||
                               (i == numXTiles - 1 || j == numYTiles - 1);
            }
        }
        return matrix;
    }

    public override Vector2Int[] getSpawnPoints()
    {
        return spawnPoints;
    }
}
