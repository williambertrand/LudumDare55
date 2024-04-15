using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectorType
{
    TRUNK,
    SHELF,
    TABLE,
}

public class CollectorItem : TaskItem
{

    // Only accept item w/ this id
    public string collectIdFilter;

    // TODO: Add some trashcans that get full after x num of trashbags get put in it
    public bool hasLimit;
    public int limit;

    public Transform[] spots;
    private int spotsUsed;

    [SerializeField] private SpriteRenderer onLimitReachedSprite;
    [SerializeField] private CollectorType collectorItemType;

    private void Start()
    {
        type = TaskItemInteractionType.COLLECTOR;
        spotsUsed = 0;
    }

    public override void HandlePlayerInteract()
    {
      
    }

    public override void HandleItemInteract(TaskItem item)
    {
        if (hasLimit && spotsUsed == limit) return;
        if(!(item is TrashItem) && !(item is TidyItem))
        {
            Debug.Log("Trash can does not interact with this item");
            return;
        }
        if(collectIdFilter != null)
        {
            if (item.id.Equals(collectIdFilter))
            {
                // Collect this item
                item.OnCollect(this);
                spotsUsed += 1;
            }
        }
        else
        {
            // Collect!
            item.OnCollect(this);
        }

        if(hasLimit && spotsUsed == limit)
        {
            if(collectorItemType == CollectorType.TRUNK)
            {
                AudioManager.Instance.PlayOneShot(AudioEvent.TRUNK_CLOSE);
            }
            
            if (onLimitReachedSprite != null)
                onLimitReachedSprite.enabled = true;
        }
    }

    public Transform getOpenSpot()
    {
        // Just re-use spots if we've run out
        return spots[Mathf.Min(spotsUsed, spots.Length - 1)];
    }


}
