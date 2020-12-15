using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSlider_Update : MonoBehaviour
{
    public string beforeText;
    public string afterText;

    public Text thisText;


    void Start()
    {
        thisText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //thisText.text = "My text has now changed.";
    }

    public void ChangeText(float value)
    {
        thisText.text = beforeText + value.ToString("F2") + " " + afterText;
    }
}
