using UnityEngine;
using UnityEngine.UI;

public class CoolDownTimer : MonoBehaviour
{
    public Slider slider;
    public float max = 10;
    public Attack attack;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       //slider.value = max - ( GameManager.getEpochTime() - attack.coolDownStartTime);
    }

    public void updateSlider(float value)
    {
        slider.value = value;
    }
}
