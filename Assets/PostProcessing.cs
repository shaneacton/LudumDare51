using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcessing : MonoBehaviour
{
    public Volume volume;
    private LensDistortion lensDistortion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
