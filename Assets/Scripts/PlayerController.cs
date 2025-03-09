using System;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    //Movement speed
    public float speed = 10;

    //Rigidbody of player object
    public Rigidbody2D rb;

    // Animator of player sprite
    private Animator playerSpriteAnimator;

    private void Start()
    {
        playerSpriteAnimator = GetComponent<Animator>();
    }

    //Used FixedUpdate instead of Update for the love of god
    public void FixedUpdate()
    {
        //Register inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //Turn input into movement vector
        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;

        //Apply movement
        rb.MovePosition(rb.transform.position + tempVect);

        UpdatePlayerSpriteAnimator(h, v);
    }

    private void UpdatePlayerSpriteAnimator(float h, float v)
    {
        playerSpriteAnimator.SetFloat("xVelocity", h);
        playerSpriteAnimator.SetFloat("yVelocity", v);
    }
}

