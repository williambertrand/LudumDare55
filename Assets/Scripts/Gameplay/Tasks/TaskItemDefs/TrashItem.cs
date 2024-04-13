using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashItem : TaskItem
{

    private void Start()
    {
        type = TaskItemInteractionType.PICK_UP;
    }

    public override void HandlePlayerInteract()
    {
        FindObjectOfType<PlayerInteraction>().TryPickUp(this);
    }

    public override void HandleItemInteract(TaskItem t)
    {
        // Handled in collector
    }

    public override void OnCollect(CollectorItem collector)
    {
        // Emit some kind of effect
        // EffectsManager.Instantiate(cleanUpEffect, gameObject.transform.position);
        // Emit some kind of task complete event from a task manager
        RoomTasksManager.Instance.OnTaskWasCompleted();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GameConstants.ITEM_CHECK_RADIUS);
    }
}
