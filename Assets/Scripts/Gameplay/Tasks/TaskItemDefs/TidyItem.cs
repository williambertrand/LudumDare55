using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TidyItem : TaskItem
{
    private bool complete;
    [SerializeField] private SpriteRenderer activeInteractionSprite;

    private void Start()
    {
        complete = false;
        activeInteractionSprite.enabled = false;
    }
    public override void HandlePlayerInteract()
    {
        if (complete) return;
        FindObjectOfType<PlayerInteraction>().TryPickUp(this);
        activeInteractionSprite.enabled = true;
    }

    public override void OnCollect(CollectorItem collector)
    {
        complete = true;
        Transform spot = collector.getOpenSpot();
        transform.DOMove(spot.position, 0.5f);
        RoomTasksManager.Instance.OnTaskWasCompleted();
    }
}
