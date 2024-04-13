using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidyItem : TaskItem
{
    public override void HandlePlayerInteract()
    {
        FindObjectOfType<PlayerInteraction>().TryPickUp(this);
    }

    public override void OnCollect(CollectorItem collector)
    {
        transform.position = collector.spot.position;
        RoomTasksManager.Instance.OnTaskWasCompleted();
    }
}
