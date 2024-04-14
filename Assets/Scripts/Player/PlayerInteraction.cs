using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoSingleton<PlayerInteraction>
{

    public TaskItem currentCaryItem;
    public GameObject carryHolder;
    public float carryItemOriginalYPos;

    [Header("interaction distance and pos")]
    public Transform interactPos;
    public float interactRadius;
    public LayerMask interactLayer;

    public delegate void OnHoldInteractionStart();
    public OnHoldInteractionStart onHoldDownInteractStart;

    public delegate void OnHoldInteractionEnd();
    public OnHoldInteractionStart onHoldDownInteractEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryPickUp(TaskItem t)
    {
        if(currentCaryItem == null)
        {
            currentCaryItem = t;
            t.gameObject.transform.parent = transform;
            carryItemOriginalYPos = t.gameObject.transform.position.y;
            t.gameObject.transform.localPosition = carryHolder.transform.localPosition;
        } else
        {
            Debug.LogFormat("Player already holding item: {0}", currentCaryItem.gameObject.name);
        }
    }

    public void Drop()
    {
        if(currentCaryItem == null)
        {
            Debug.Log("no item to drop");
            return;
        }

        currentCaryItem.transform.parent = null;
        // currentCaryItem.transform.position = new Vector3(currentCaryItem.transform.position.x, carryItemOriginalYPos, currentCaryItem.transform.position.z);
        currentCaryItem.OnDrop();
        currentCaryItem = null;
    }

    public void Interact(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started)
        {
            
            if (currentCaryItem != null)
            {
                Drop();
                return;
            }

            Collider2D col = Physics2D.OverlapCircle(interactPos.position, interactRadius, interactLayer);
            if (col != null)
            {
                Debug.LogFormat("interact item: {0}", col.gameObject.name);
                TaskItem otherItem = col.gameObject.GetComponent<TaskItem>();
                if(otherItem == null)
                {
                    Debug.Log("other item not on col");
                    return;
                }
                else if (otherItem is SweepItem)
                {
                    onHoldDownInteractStart?.Invoke();
                }
                otherItem.HandlePlayerInteract();
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("HOLD DOWN END!!!");
            onHoldDownInteractEnd?.Invoke();
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactPos.position, interactRadius);
    }
}
