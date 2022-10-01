using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyManager: MonoBehaviour
{
    public static EnemyManager singleton;
    private static RectangleF mapBounds;
    public HashSet<Enemy> enemies;
    private void Awake()
    {
        singleton = this;
        enemies = new HashSet<Enemy>();
    }

    public static List<Enemy> getNearbyEnemies(Enemy center, float minimumDistance=1f)
    {
        List<Enemy> nearby = new List<Enemy>();

        foreach (Enemy candidate in singleton.enemies)
        {
            if (center == candidate)
            {
                continue;
            }
            float dist = Vector3.Distance(center.transform.position, candidate.transform.position);
            if (dist < minimumDistance)
            {
                nearby.Add(candidate);
            }
        }

        return nearby;
    }

    public static void registerEnemy(Enemy newEnemy)
    {
        singleton.enemies.Add(newEnemy);
    }

    public static void removeEnemy(Enemy enemy)
    {
        singleton.enemies.Remove(enemy);
    }
}
