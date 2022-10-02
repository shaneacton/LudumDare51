using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnTile : MonoBehaviour
{

    public VisualEffect EnemySpawnVFX;
    public Light Light;
    private void Start() { EnemySpawnVFX.Stop(); }

    public void StartEffects()
    {
        EnemySpawnVFX.Play();
        Light.enabled = true;
    }
    public void StopEffects()
    {
        EnemySpawnVFX.Stop();
        Light.enabled = false;
    }
}
