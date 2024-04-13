using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerState
{
    Normal,
    InRoll,
    Stunned
}

public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    public static event Action OnPlayerMove;

    public PlayerState currentState;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Movement")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float verticalFactor;
    [SerializeField] private float startingRollSpeed;
    [SerializeField] private float rollDropOffFactor;

    private float rollSpeed; // represents current speed while rolling, drops off back down to match movespeed   
    private Vector3 moveDir;
    private Vector3 rollDir;

    private bool isFacingRight;

    private Collider2D _collider2D;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        //playerController = GetComponent<PlayerController>();
        currentState = PlayerState.Normal;
        isFacingRight = true;
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
            case PlayerState.Normal:
                _collider2D.enabled = true;
                Normal();
                break;
        }

    }

    private void Stunned()
    {

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

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("roll");
            rollSpeed = startingRollSpeed;
            rollDir = moveDir;
            currentState = PlayerState.InRoll;
            // pause walk wile roll/dash
            // animator.SetFloat("speed", 0.0f);
        }

        if (moveX > 0) isFacingRight = true;
        else if (moveX < 0) isFacingRight = false;

        Flip();
    }

    public bool CanMove()
    {
        //if (playerController._isStunned) return false;
        //if (playerAttack.isPlayerAttacking) return false;
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

    private void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x = isFacingRight ? 1 : -1;
        transform.localScale = theScale;
    }
}