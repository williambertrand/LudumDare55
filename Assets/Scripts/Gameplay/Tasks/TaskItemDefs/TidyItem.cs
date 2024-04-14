using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TidyItem : TaskItem
{
    private bool complete;

    private void Start()
    {
        complete = false;
    }
    public override void HandlePlayerInteract()
    {
        if (complete) return;
        FindObjectOfType<PlayerInteraction>().TryPickUp(this);
    }

    public override void OnCollect(CollectorItem collector)
    {
        complete = true;
        Transform spot = collector.getOpenSpot();
        transform.DOMove(spot.position, 0.5f);
        RoomTasksManager.Instance.OnTaskWasCompleted();
    }
}
