using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoSingleton<Timer>
{
    public float seconds;
    public bool paused;
    private float currentTime;
    private bool finished;

    public TMP_Text uiTimerText;

    public delegate void OnTimerFinished();
    public OnTimerFinished onTimerFinished;

    void Start()
    {
        finished = false;
        paused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (finished) return;

        if (paused)
        {
            return;
        }

        if (currentTime <= 0)
        {
            Debug.Log("TIMER EXPIRED !_!_!_!_");
            finished = true;
            onTimerFinished?.Invoke();
            return;
        }
           
        currentTime -= Time.deltaTime;
        uiTimerText.text = string.Format("{0}", currentTime.ToString("F2"));
    }

    public void Resume()
    {
        paused = false;
    }
    public void Pause()
    {
        paused = true;
    }

    public void SetTime(float val)
    {
        currentTime = val;
        finished = false;
        uiTimerText.text = string.Format("{0}", currentTime.ToString("F2"));
    }
}
