using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TidyItemType
{
    BOOK,
    WINE,
    CANDLE,
    OTHER
}

public class TidyItem : TaskItem
{
    private bool complete;
    [SerializeField] private SpriteRenderer originalSprite;

    [SerializeField] private SpriteRenderer activeInteractionSprite;

    [SerializeField] private SpriteRenderer collectedInteractionSprite;

    [SerializeField] private TidyItemType tidyItemType;

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
        PlaySound();
    }

    private void PlaySound ()
    {
        switch (tidyItemType)
        {
            case TidyItemType.BOOK:
                AudioManager.Instance.PlayOneShot(AudioEvent.BOOK_DROP);
                break;
            case TidyItemType.WINE:
                AudioManager.Instance.PlayOneShot(AudioEvent.WINE);
                break;
            case TidyItemType.OTHER:
                AudioManager.Instance.PlayOneShot(AudioEvent.OTHER);
                break;
            default:
                break;
        }
    }
}
