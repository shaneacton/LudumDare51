using UnityEngine;

public class RandomMap: MapDefinition
{
    public float obstacleChance = 0.2f; // 0.2

    public override bool[,] getObstacleMatrix()
    {
        bool[,] matrix = new bool[numXTiles, numYTiles];
        for (int i = 0; i < numXTiles; i++)
        {
            for (int j = 0; j < numYTiles; j++)
            {
                bool obstacle = Random.Range(0f, 1f) < obstacleChance;
                matrix[i, j] = obstacle;
            }
        }
        return matrix;
    }

    public override Vector2Int[] getSpawnPoints()
    {
        throw new System.NotImplementedException();
    }
}
