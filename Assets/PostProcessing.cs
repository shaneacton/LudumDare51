using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcessing : MonoBehaviour
{
    public Volume volume;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

    public float frequencyMultiplier = 1;
    public float intensityMultiplier = 1f;
    public float xDistortionMultiplier = 0.5f;
    public float chromeIntensityMultiplier = 0.5f;

    public float defaultIntensity = 0.4f;
    public float defaultX = 0.3f;

    public float lerpStart;
    public float targetLerpTime;

    public float chromiomIntensity = 0.4f;

    private void Start()
    {
    }

    private void Update()
    {
        if (SpawnManager.instance._state == SpawnManager.State.Break)
        {
            DistortionFunction();
        }

        else
        {
            var intensity = Mathf.Lerp(GetDistortionIntensit(), defaultIntensity, (Time.time - lerpStart) / targetLerpTime);
            var xmag = Mathf.Lerp(GetDistortionXMultiplier(), defaultX, (Time.time - lerpStart) / targetLerpTime);
            var chromIntensity = Mathf.Lerp(GetChromaticAberation(), chromiomIntensity, (Time.time - lerpStart) / targetLerpTime);
            LensDistortionIntensity(intensity);
            LensDistortionXMultiplier(xmag);
            ChromaticAberration(chromIntensity);
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

    public void ChromaticAberration(float value)
    {
        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            chromaticAberration.intensity.value = value;
        }
    }

    public float GetChromaticAberation()
    {
        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            return chromaticAberration.intensity.value;
        }

        return chromiomIntensity;
    }

    public void DistortionFunction()
    {
        float intensity = (Mathf.Cos(Time.time * frequencyMultiplier) + 1) * intensityMultiplier;
        float chromeIntensity = (Mathf.Cos(Time.time * frequencyMultiplier) + 1) * chromeIntensityMultiplier;
        float xShift = (Mathf.Cos(Time.time * frequencyMultiplier) + 1) * xDistortionMultiplier;

        LensDistortionIntensity(intensity);
        LensDistortionXMultiplier(xShift);
        ChromaticAberration(chromeIntensity);
    }
}
