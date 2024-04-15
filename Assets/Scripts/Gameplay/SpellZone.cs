using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellZone : MonoBehaviour
{

    [SerializeField] private float speedFactor;

    [SerializeField] private SpriteRenderer activeSprite;
    [SerializeField] private SpriteRenderer baseSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement.Instance.UpdateSpeed(speedFactor);
            activeSprite.enabled = true;
            baseSprite.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement.Instance.UpdateSpeed(1);
            activeSprite.enabled = false;
            baseSprite.enabled = true;
        }
    }
}
