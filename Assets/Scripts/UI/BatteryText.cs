using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText(string text)
    {
        this.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void ChangeColour(Color color)
    {
        this.GetComponent<TextMeshProUGUI>().color = color;
        //this.txtAdsStatus.
    }

}
