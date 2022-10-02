using UnityEngine;
using UnityEngine.UI;

public class CoolDownTimer : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update

    public void UpdateSlider(float value)
    {
        slider.value = value;

        if (value < slider.maxValue && value >= slider.maxValue / 2)
        {
            ChangeColour(new Color(255, 255, 0)); //yellow
        }
        else if (value < slider.maxValue / 2)
        {
            ChangeColour(new Color(255, 50, 0)); //red
        }
        else if (value == slider.maxValue)
        {
            ChangeColour(new Color(0, 255, 0)); //green
        }
    }

    public void SetMax(float value)
    {
        slider.maxValue = value;
    }

    private void ChangeColour(Color color)
    {
        this.GetComponent<Image>().color = color;
    }
}
