using UnityEngine;
using UnityEngine.UI;

public class RoundTimerUI : MonoBehaviour
{
    public Slider slider;
    public float max = 10;
    public GameObject fill;

    void Start()
    {
        slider.maxValue = max;
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

        if (value > 5) GameObject.Find("Fill").GetComponent<Image>().color = new Color(0, 255, 0);

        else if (value <= 6 && value >3) GameObject.Find("Fill").GetComponent<Image>().color = new Color(255, 255, 0); // yellow
 
        else if (value < 3) GameObject.Find("Fill").GetComponent<Image>().color = new Color(255, 0, 0); // red

    }
}
