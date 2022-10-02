using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcessing : MonoBehaviour
{
    public Volume volume;
    private LensDistortion lensDistortion;

    //public float lifespanSeconds = 5;
    //public float flashingSeconds = 2.5f;
    //public float flashFrequency = 3;

    public float frequencyMultiplier = 1;
    public float intensityMultiplier = 0.5f;
    public float xDistortionMultiplier = 0.2f;


    private void Start()
    {
    }

    private void Update()
    {
        if (SpawnManager.instance._state == SpawnManager.State.Break)
        {
            DistortionFunction(SpawnManager.instance.breakTime);
        }
        else
        {
            LensDistortionIntensity(intensityMultiplier);
            LensDistortionXMultiplier(xDistortionMultiplier);
        }
    }

    public void LensDistortionIntensity(float value)
    {
        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.intensity.value = value;
        }
    }

    public void LensDistortionXMultiplier(float value)
    {
        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.xMultiplier.value = value;
        }
    }

    public void DistortionFunction(long period)
    {

        float frequency = (1 / period)*frequencyMultiplier;

        float timeSinceSpawn = Time.time - SpawnManager.instance.breakStartTime;
        //float deltT = timeSinceSpawn - lifespanSeconds + flashingSeconds;
        //float flashTimeLeft = (lifespanSeconds - timeSinceSpawn) / flashingSeconds;
        //float x = deltT * flashFrequency / flashTimeLeft;

        Debug.Log(timeSinceSpawn);

        float intensity = (Mathf.Cos(frequency* timeSinceSpawn)) * intensityMultiplier;
        float x = (Mathf.Cos(frequency* timeSinceSpawn)) * xDistortionMultiplier;

        LensDistortionIntensity(intensity);
        LensDistortionXMultiplier(x);
        Debug.Log($"{intensity} {x}");
    }
}
