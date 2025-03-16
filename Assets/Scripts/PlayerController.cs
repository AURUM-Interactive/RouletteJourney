using System;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    //Movement speed
    public float movementSpeed = 10;

    //Rigidbody of player object
    public Rigidbody2D rb;

    // Animator of player sprite
    private Animator playerSpriteAnimator;

    private Vector2 moveDirection;

    private void Start()
    {
        playerSpriteAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        GetInput();
    }

    public void FixedUpdate()
    {
        MovePlayer();
        UpdatePlayerSpriteAnimator(moveDirection.x, moveDirection.y);
    }

    private void GetInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
    }

    private void MovePlayer()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
    }

    private void UpdatePlayerSpriteAnimator(float horizontalInput, float verticalInput)
    {
        playerSpriteAnimator.SetFloat("xVelocity", horizontalInput);
        playerSpriteAnimator.SetFloat("yVelocity", verticalInput);
    }
}

