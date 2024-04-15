using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskItemInteractionType
{
    PICK_UP,
    COLLECTOR, //E.g trask can, dumpster, something that takes in other objects
    SWEEP, // takes an amount of time where the player is interacting to complete
}

public class TaskItem : MonoBehaviour
{

    public GameObject obj;
    public Transform position;
    protected TaskItemInteractionType type;
    public string id;

    // We may need ref to a task from the item so we can use it on complete?

    public virtual void OnDrop()
    {
        if(type != TaskItemInteractionType.PICK_UP)
        {
            Debug.Log("no drop action for task item");
            return;
        }

        if(gameObject.GetComponent<BoxCollider2D>() != null)
            gameObject.GetComponent<BoxCollider2D>().enabled = true;

        // Check for interaction item within bounds
        Collider2D[] otherItemCols = Physics2D.OverlapCircleAll(transform.position, GameConstants.ITEM_CHECK_RADIUS);
        if (otherItemCols != null && otherItemCols.Length > 0)
        {
            foreach(Collider2D col in otherItemCols)
            {
                TaskItem otherItem = col.gameObject.GetComponent<TaskItem>();
                if (otherItem != null && otherItem.type == TaskItemInteractionType.COLLECTOR)
                {
                    otherItem.HandleItemInteract(this);
                }
            }
        }
    }

    public virtual void OnCollect(CollectorItem collector)
    {
        Debug.LogFormat("No collect method added for item: {0}", gameObject.name);
    }

    public virtual void HandlePlayerInteract()
    {
        Debug.LogError("TASK ITEM HANDLING PLAYER INTERACT!");
        switch (type)
        {
            case TaskItemInteractionType.COLLECTOR:
                break;
            case TaskItemInteractionType.SWEEP:
                break;
            case TaskItemInteractionType.PICK_UP:
                break;
        }
    }

    public virtual void HandleItemInteract(TaskItem t)
    {
        Debug.LogError("TASK ITEM HANDLING ITEM INTERACT!");
        switch (type)
        {
            case TaskItemInteractionType.COLLECTOR:
                break;
            case TaskItemInteractionType.SWEEP:
                break;
            case TaskItemInteractionType.PICK_UP:
                break;
        }
    }
}
