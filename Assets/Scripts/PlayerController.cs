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

    [SerializeField]
    GameObject pauseMenu;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = !pauseMenu.activeSelf ? 0 : 1;
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    private void MovePlayer()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
    }

    private void UpdatePlayerSpriteAnimator(float horizontalInput, float verticalInput)
    {
        

        playerSpriteAnimator.SetInteger("xVelocity", normalizeDirection(horizontalInput));
        playerSpriteAnimator.SetInteger("yVelocity", normalizeDirection(verticalInput));
    }

    int normalizeDirection(float direction)
    {
        int xDirection;
        if (direction != 0)
        {
            if (direction > 0)
            {
                xDirection = 1;
            }
            else
            {
                xDirection = -1;
            }
        }
        else
        {
            xDirection = 0;
        }

        return xDirection;
    }
}

