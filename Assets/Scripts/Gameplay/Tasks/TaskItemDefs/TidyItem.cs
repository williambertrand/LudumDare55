using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TidyItem : TaskItem
{
    private bool complete;
    [SerializeField] private SpriteRenderer originalSprite;

    [SerializeField] private SpriteRenderer activeInteractionSprite;

    [SerializeField] private SpriteRenderer collectedInteractionSprite;

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
        if(collectedInteractionSprite != null)
        {
            originalSprite.enabled = false;
            activeInteractionSprite.enabled = false;
            collectedInteractionSprite.enabled = true;
        }
        complete = true;
        Transform spot = collector.getOpenSpot();
        transform.DOMove(spot.position, 0.5f);
        RoomTasksManager.Instance.OnTaskWasCompleted();
        EffectsManager.Instance.SpawnEffectAtPosition(EffectType.SPARKLE, spot.transform.position);
    }
}
