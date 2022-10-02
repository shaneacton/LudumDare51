using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnTile : MonoBehaviour
{

    public VisualEffect EnemySpawnVFX;
    private void Start() { EnemySpawnVFX.Stop(); }

}
