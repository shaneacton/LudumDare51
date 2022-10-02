using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcessing : MonoBehaviour
{
    public Volume volume;
    private LensDistortion lensDistortion;

    public float lifespanSeconds = 5;
    public float flashingSeconds = 2.5f;
    //public float flashFrequency = 3;

    public float frequencyMultiplier = 1;
    public float intensityMultiplier = 1f;
    public float xDistortionMultiplier = 0.5f;

    public float defaultIntensity = 0.4f;
    public float defaultX = 0.3f;

    public float lerpStart;
    public float targetLerpTime;



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
           var intensity =  Mathf.Lerp(GetDistortionIntensit(), defaultIntensity, (Time.time - lerpStart) / targetLerpTime);
            var xmag = Mathf.Lerp(GetDistortionXMultiplier(), defaultX, (Time.time - lerpStart) / targetLerpTime);
            LensDistortionIntensity(intensity);
            LensDistortionXMultiplier(xmag);
        }
        
    }

    public void LensDistortionIntensity(float value)
    {
        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.intensity.value = value;
        }
    }

    public float GetDistortionIntensit()
    {
        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            return lensDistortion.intensity.value;
        }
        return defaultIntensity;
    }

    public void LensDistortionXMultiplier(float value)
    {
        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.xMultiplier.value = value;
        }
    }

    public float GetDistortionXMultiplier()
    {
        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            return lensDistortion.xMultiplier.value;
        }
        return defaultX;
    }

    public void DistortionFunction(long period)
    {

        float frequency = (1 / period)*frequencyMultiplier;

        float timeSinceSpawn = Time.time - SpawnManager.instance.breakStartTime;
        //float deltT = timeSinceSpawn - lifespanSeconds + flashingSeconds;
        //float flashTimeLeft = (lifespanSeconds - timeSinceSpawn) / flashingSeconds;
        //float x = deltT * frequency / flashTimeLeft;

       

        float intensity = (Mathf.Cos(Time.time*frequencyMultiplier) + 1) * intensityMultiplier;
        float xShift = (Mathf.Cos(Time.time*frequencyMultiplier) + 1) * xDistortionMultiplier;

        LensDistortionIntensity(intensity);
        LensDistortionXMultiplier(xShift);
        //Debug.Log($"{intensity} {x}");
    }
}
