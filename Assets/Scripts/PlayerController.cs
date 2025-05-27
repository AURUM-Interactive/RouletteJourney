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

    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    public bool ControlsEnabled { get; set; }

    [SerializeField]
    GameObject pauseMenu;

    private void Start()
    {
        ControlsEnabled = true;
        playerSpriteAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        GetInput();
        UpdatePlayerSpriteAnimator();
    }

    public void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        if (ControlsEnabled)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (moveInput.magnitude != 0)
            {
                lastMoveDirection = moveInput;
            }

            moveInput = new Vector2(horizontalInput, verticalInput).normalized;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    private void MovePlayer()
    {
        rb.linearVelocity = moveInput * movementSpeed;
    }

    private void UpdatePlayerSpriteAnimator()
    {
        playerSpriteAnimator.SetFloat("MoveX", moveInput.x);
        playerSpriteAnimator.SetFloat("MoveY", moveInput.y);
        playerSpriteAnimator.SetFloat("MoveMagnitude", moveInput.magnitude);
        playerSpriteAnimator.SetFloat("LastMoveX", lastMoveDirection.x);
        playerSpriteAnimator.SetFloat("LastMoveY", lastMoveDirection.y);

        //Debug.Log($"x: {lastMoveDirection}");
    }
}

