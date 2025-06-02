using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {
    private int start_time = 0;
    private int textTime = 90;
    private int actualTime = 90;
    public TMP_Text timer; 
    private bool on_scene = true;


    void Start() {
        UpdateTimer();
    }

    void Update() {
        if (on_scene == true) {
            textTime = start_time + actualTime - (int) Time.time;
            UpdateTimer();

            if (textTime <= 0)
            {
                timerEnded();
            }
        }
    }
    void UpdateTimer() {
        string seconds;
        if (textTime % 60 < 10)
            seconds = "0" + textTime % 60;
        else
            seconds = textTime % 60 + "";
        timer.SetText("Time Left: " + textTime / 60 + ":" + seconds);
    }

    void timerEnded()
    {
        on_scene = false;
    }


}