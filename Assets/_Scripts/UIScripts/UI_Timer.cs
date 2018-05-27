using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Timer : MonoBehaviour
{
    public Text TimerText;
    public Image TimerFill;

    private int maxTimer;
    private float step;
    private float timePassed;
    private bool hasChangedMaxTimer;


    public void SetTimer(int max)
    {
        ResetTimer();

        maxTimer = max;
        step = 1.0f / maxTimer;
        hasChangedMaxTimer = true;
    }

    private void ResetTimer()
    {
        timePassed = 0;
        step = 0.0f;
        hasChangedMaxTimer = false;
        TimerFill.fillAmount = 0;
    }

    public void SetTime(int time)
    {
        TimerText.text = time.ToString();
        //Debug.Log("fill amount = "+time * step+" at time = "+time+" , and step = "+step);
        TimerFill.fillAmount = time * step;

        //Mathf.InverseLerp(0, 20, 8); // Interpolant between a and b
    }
}
