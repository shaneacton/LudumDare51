using UnityEngine;
using UnityEngine.UI;

public class RoundTimerUI : MonoBehaviour
{
    public Slider slider;
    public float max = 10;
    public GameObject fill;
    public BatteryText batteryText;

    void Start()
    {
        slider.maxValue = max;
        Instantiate(batteryText);
    }

    // Update is called once per frame
    void Update()
    {

        if (SpawnManager.instance._state == SpawnManager.State.Break)
        {
            updateSlider((GameManager.getEpochTime() - SpawnManager.instance.breakStartTime) *10/3);
        }
        else
        {
            updateSlider(max - (GameManager.getEpochTime() - SpawnManager.instance.spawnStartTime));
        }
    }

    public void updateSlider(float value)
    {
        slider.value = value;

        if (value > 5)
        {
            this.GetComponent<Image>().color = new Color(0, 255, 0);
            this.batteryText.ChangeColour(new Color(0, 255, 0));
            batteryText.ChangeText("Battery High");
        }
        else if (value <= 6 && value > 3)
        {
            this.GetComponent<Image>().color = new Color(255, 255, 0); // yellow
            batteryText.ChangeColour(new Color(255, 255, 0));
            batteryText.ChangeText("Battery Mid");
        }
        else if (value < 3)
        {
            this.GetComponent<Image>().color = new Color(255, 50, 0); // red
            batteryText.ChangeColour(new Color(255, 50, 0));
            batteryText.ChangeText("Battery Low");
        }
    }
}
