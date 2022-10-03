using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class SpawnTile : MonoBehaviour
{

    public VisualEffect EnemySpawnVFX;
    public ParticleSystem PlayerChargeEffect;
    public Light2D Light;

    public float lightFlashFreq = 10;
    public float maxLightIntensity = 25;
    private void Start() { EnemySpawnVFX.Stop(); }

    private void Update()
    {
        FlashLight();
    }

    public void StartSpawn()
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
