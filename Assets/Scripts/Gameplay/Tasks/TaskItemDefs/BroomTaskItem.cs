using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomTaskItem : TaskItem
{

    private void Start()
    {
        type = TaskItemInteractionType.PICK_UP;
    }
    public override void HandlePlayerInteract()
    {
        FindObjectOfType<PlayerInteraction>().TryPickUp(this);
    }

}
