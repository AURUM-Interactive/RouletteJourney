using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionTime = 2f;

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PickRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            PickRandomDirection();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    void PickRandomDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        timer = changeDirectionTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PickRandomDirection();
    }
}
