using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepItem : TaskItem
{

    public bool isActive;
    public float timeToComplete;
    public float timeRemaining;

    private SpriteRenderer renderer;

    private void Start()
    {
        type = TaskItemInteractionType.SWEEP;
        isActive = false;
        PlayerInteraction.Instance.onHoldDownInteractStart += OnHoldDownStart;
        PlayerInteraction.Instance.onHoldDownInteractEnd += OnHoldDownEnd;
        timeRemaining = timeToComplete;
        renderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {

        if(timeRemaining <= 0.2f)
        {
            RoomTasksManager.Instance.OnTaskWasCompleted();
            EffectsManager.Instance.SpawnEffectAtPosition(EffectType.SPARKLE, transform.position);
            Destroy(gameObject);
        }

        if(isActive)
        {
            timeRemaining -= Time.deltaTime;
            Color tmp = renderer.color;
            tmp.a = GetCompletePercent();
            renderer.color = tmp;
        }
    }

    private void OnDestroy()
    {
        PlayerInteraction.Instance.onHoldDownInteractStart -= OnHoldDownStart;
        PlayerInteraction.Instance.onHoldDownInteractEnd -= OnHoldDownEnd;
    }

    public void OnHoldDownStart(TaskItem i)
    {
        if(i == this)
            isActive = true;
    }

    public void OnHoldDownEnd()
    {
        isActive = false;
    }

    public float GetCompletePercent()
    {
        return Mathf.Max(timeRemaining / timeToComplete, 0.2f);
    }
}
