using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BreakTimer : MonoBehaviour
{
    private long seconds = 0;
    public long maxSeconds = 3;
    private TextMeshProUGUI timeText;

    private void Start() {
        timeText = gameObject.GetComponent<TextMeshProUGUI>();
        maxSeconds = SpawnManager.instance.breakTime;
    }

    // Update is called once per frame
    void Update()
    {
        seconds = GameManager.getEpochTime() - SpawnManager.instance.breakStartTime;
        DisplaySeconds(maxSeconds - seconds);
    }

    public void DisplaySeconds(float seconds){

        if(seconds < 0) {
            seconds = 0;
        }
       
        seconds = Mathf.FloorToInt(seconds % 60);
        timeText.text = seconds.ToString();
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    public void Show(){
        gameObject.SetActive(true);
    }
}
