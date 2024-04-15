using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrashItem : TaskItem
{

    private bool complete;
    [SerializeField] private SpriteRenderer activeInteractionSprite;

    private void Start()
    {
        type = TaskItemInteractionType.PICK_UP;
        complete = false;
        activeInteractionSprite.enabled = false;
    }

    public override void HandlePlayerInteract()
    {
        if (complete) return;
        activeInteractionSprite.enabled = true;
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
        complete = true;
        RoomTasksManager.Instance.OnTaskWasCompleted();
        transform.DOMove(collector.transform.position, 0.25f);
        EffectsManager.Instance.SpawnEffectAtPosition(EffectType.SPARKLE, collector.transform.position);
        StartCoroutine(DestorySelfAfterDelay());
        AudioManager.Instance.PlayOneShot(AudioEvent.SHOE_DROP);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GameConstants.ITEM_CHECK_RADIUS);
    }

    private IEnumerator DestorySelfAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
