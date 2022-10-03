using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class SpawnTile : MonoBehaviour
{

    public VisualEffect EnemySpawnVFX;
    public Light2D Light;

    public float lightFlashFreq = 10;
    public float maxLightIntensity = 25;
    private void Start() { EnemySpawnVFX.Stop(); }

    private void Update()
    {
        FlashLight();
    }

    public void StartEffects()
    {
        StartCoroutine(PlayVFXLate());
        Light.enabled = true;
    }
    IEnumerator PlayVFXLate()
    {
        yield return new WaitForSeconds(1);
        EnemySpawnVFX.Play();
    }
    public void StopEffects()
    {
        EnemySpawnVFX.Stop();
        Light.enabled = false;
    }

    public void FlashLight()
    {
        Light.intensity = (Mathf.Sin(Time.time * lightFlashFreq) + 1) * maxLightIntensity;
    }
}
