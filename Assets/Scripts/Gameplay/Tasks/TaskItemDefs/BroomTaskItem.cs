using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomTaskItem : TaskItem
{

    private Animator anim;

    [SerializeField] private SpriteRenderer displaySprite;
    [SerializeField] private SpriteRenderer activeInteractionSprite;

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
        activeInteractionSprite.enabled = true;
        displaySprite.enabled = false;
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

    public override void OnDrop()
    {
        base.OnDrop();
        if(activeInteractionSprite != null)
            activeInteractionSprite.enabled = false;
        if (displaySprite != null)
            displaySprite.enabled = true;
    }

}
