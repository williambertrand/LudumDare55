using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerState
{
    Normal,
    InRoll,
    Sweeping
}

public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    public static event Action OnPlayerMove;

    public PlayerState currentState;
    public bool waiting;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Movement")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float verticalFactor;
    [SerializeField] private float startingRollSpeed;
    [SerializeField] private float rollDropOffFactor;
    private float baseSpeed;

    private float rollSpeed; // represents current speed while rolling, drops off back down to match movespeed   
    private Vector3 moveDir;
    private Vector3 rollDir;

    private bool isFacingRight;

    private Collider2D _collider2D;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        //playerController = GetComponent<PlayerController>();
        currentState = PlayerState.Normal;
        isFacingRight = true;
    }

    private void Start()
    {
        PlayerInteraction.Instance.onHoldDownInteractStart += OnHoldDownStart;
        PlayerInteraction.Instance.onHoldDownInteractEnd += OnHoldDownEnd;
        baseSpeed = moveSpeed;
    }

    private void OnDestroy()
    {
        PlayerInteraction.Instance.onHoldDownInteractStart -= OnHoldDownStart;
        PlayerInteraction.Instance.onHoldDownInteractEnd -= OnHoldDownEnd;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case PlayerState.InRoll:
                _collider2D.enabled = false;
                InRoll();
                break;
            case PlayerState.Sweeping:
            case PlayerState.Normal:
                _collider2D.enabled = true;
                Normal();
                break;
        }

    }

    private void InRoll()
    {
        // ignore inputs while rolling, just handle speed
        rollSpeed -= rollSpeed * rollDropOffFactor * Time.deltaTime;
        float rollSpeedMinimum = moveSpeed;
        if (rollSpeed < rollSpeedMinimum)
        {
            currentState = PlayerState.Normal;
        }
    }

    private void Normal()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (!CanMove())
        {
            moveDir = Vector3.zero;
            return;
        }
       
        moveDir = new Vector3(moveX, moveY).normalized;
        moveDir.y *= verticalFactor;

        if (animator != null)
            animator.SetFloat("speed", Mathf.Abs(moveDir.magnitude));

        

        if (moveX > 0) isFacingRight = true;
        else if (moveX < 0) isFacingRight = false;

        Flip();
    }

    public bool CanMove()
    {
        if (waiting) return false;
        if (currentState == PlayerState.Sweeping) return false;
        return true;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Normal:
                rigidBody.velocity = moveDir * moveSpeed;
                // animator.SetFloat("speed", rigidBody.velocity.sqrMagnitude);
                if (rigidBody != null)
                    rigidBody.velocity = moveDir * moveSpeed;

                if (moveDir != Vector3.zero)
                    OnPlayerMove?.Invoke();


                break;
            case PlayerState.InRoll:
                if (rigidBody != null)
                    rigidBody.velocity = rollDir * rollSpeed;
                break;
        }
    }

    public void OnHoldDownStart(TaskItem i)
    {
        currentState = PlayerState.Sweeping;
    }

    public void OnHoldDownEnd()
    {
        currentState = PlayerState.Normal;
    }

    private void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x = isFacingRight ? 1 : -1;
        transform.localScale = theScale;
    }


    public void UpdateSpeed(float speedFactor)
    {
        moveSpeed = baseSpeed * speedFactor;
    }
}