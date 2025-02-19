using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    //Movement speed
    public float speed = 10;

    //Rigidbody of player object
    public Rigidbody2D rb;

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
    }
}

