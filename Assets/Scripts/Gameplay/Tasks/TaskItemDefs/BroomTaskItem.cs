using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomTaskItem : TaskItem
{

    private Animator anim;

    private void Start()
    {
        type = TaskItemInteractionType.PICK_UP;
        PlayerInteraction.Instance.onHoldDownInteractStart += OnHoldDownStart;
        PlayerInteraction.Instance.onHoldDownInteractEnd += OnHoldDownEnd;

        anim = GetComponent<Animator>();
    }
    public override void HandlePlayerInteract()
    {
        FindObjectOfType<PlayerInteraction>().TryPickUp(this);
    }

    public void OnHoldDownStart(TaskItem i)
    {
        AudioManager.Instance.PlayOneShot(AudioEvent.SWEEP);
        anim.SetBool("sweeping", true);
    }

    public void OnHoldDownEnd()
    {
        anim.SetBool("sweeping", false);
    }

}
