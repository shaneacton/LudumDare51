using UnityEngine;
using UnityEngine.UI;

public class RoundTimerUI : MonoBehaviour
{
    public Slider slider;
    public float max = 10;

    void Start()
    {
        slider.maxValue = max;
    }

    // Update is called once per frame
    void Update()
    {
        updateSlider(max - (GameManager.getEpochTime() - SpawnManager.instance.spawnStartTime));
    }

    private void updateSlider(float value)
    {
        slider.value = value;
    }
}
