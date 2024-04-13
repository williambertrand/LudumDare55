using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float seconds;
    private float currentTime;
    private bool finished;

    delegate void OnTimerFinished();
    OnTimerFinished onTimerFinished;

    void Start()
    {
        currentTime = seconds;
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (finished) return;

        if (currentTime <= 0)
        {
            finished = true;
            onTimerFinished?.Invoke();
            return;
        }
           
        currentTime -= Time.deltaTime;
    }
}
