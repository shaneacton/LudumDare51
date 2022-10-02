using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{

    public ParticleSystem EnemySpawnVFX;

    private void Start() { EnemySpawnVFX.Stop(); }

}
