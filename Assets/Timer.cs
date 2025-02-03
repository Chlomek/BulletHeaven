using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class Timer : MonoBehaviour
{
    public TMP_Text TimeAlive;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //TimeAlive.text = Time.time.ToString("F0");

        elapsedTime += Time.deltaTime; // Increment elapsed time by the time since the last frame

        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        TimeAlive.text = formattedTime;
    }
}
