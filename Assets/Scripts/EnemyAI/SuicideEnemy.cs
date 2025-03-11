using System.Collections;
using UnityEngine;

public class SuicideEnemy : MonoBehaviour
{
    public float distance;
    public Transform Player;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(this.transform.position, Player.position);
    }

    void FixedUpdate()
    {
        if (distance < 10)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            PlayerInRange(direction);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    void PlayerInRange(Vector2 direction)
    {
        rb.linearVelocity = direction * distance;
    }
}
