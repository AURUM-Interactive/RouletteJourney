using System.Collections;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SuicideEnemy : MonoBehaviour
{
    [SerializeField] public float idleSpeed = 0.5f;
    [SerializeField] public float activeSpeed = 3f;
    [SerializeField] public float minDistanceToPlayer = 3f;
    [SerializeField] public float reactionTime = 0.5f;

    private GameObject Player;
    private bool hasLineOfSight = false;

    private Vector2 moveDirection;
    private float timer;

    private Coroutine retreatCoroutine;
    private float distanceToPlayer;
    private float bufferZone = 0.1f;
    private float pauseBeforeNextMove = 0.5f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);


        timer -= Time.deltaTime;
        if(hasLineOfSight == false && timer <= 0)
        {
            timer = 0.5f;
            MoveAround();
        }
    }

    void FixedUpdate()
    {
        if (hasLineOfSight)
        {
            if (distanceToPlayer > minDistanceToPlayer + bufferZone)
            {
                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, activeSpeed * Time.fixedDeltaTime);
            }
            else if (distanceToPlayer < minDistanceToPlayer - bufferZone)
            {
                if (retreatCoroutine == null)
                {
                    retreatCoroutine = StartCoroutine(RetreatAfterDelay());
                }
            }
        }
        else
        {
            transform.position += (Vector3)moveDirection * idleSpeed * Time.deltaTime;
        }

        RaycastHit2D ray = Physics2D.Raycast(transform.position, Player.transform.position - transform.position);
        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
        }
    }

    private void MoveAround()
    {
        timer = 1f;
        moveDirection = Random.insideUnitCircle.normalized;
        transform.position = Vector2.MoveTowards(transform.position, moveDirection, idleSpeed * Time.fixedDeltaTime);
    }

    IEnumerator RetreatAfterDelay()
    {
        yield return new WaitForSeconds(reactionTime); // Wait before deciding to retreat

        var escapeDirection = GetRandomEscapeDirection();

        float retreatTimer = 0f;
        while (retreatTimer < 0.7f)
        {
            transform.position += (Vector3)(escapeDirection * activeSpeed * Time.deltaTime);
            retreatTimer += Time.deltaTime;
            yield return null;
        }

        retreatCoroutine = null;

        yield return new WaitForSeconds(pauseBeforeNextMove); // Pause before next move
    }
    Vector2 GetRandomEscapeDirection()
    {
        Vector2 directionAway = (transform.position - Player.transform.position).normalized;

        int choice = Random.Range(1, 4);
        switch(choice)
        {
            case 1:
                directionAway = Quaternion.Euler(0, 0, 45) * directionAway;
                break;
            case 3:
                directionAway = Quaternion.Euler(0, 0, -45) * directionAway;
                break;
            default:
                break;

        }

        return directionAway;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(DestroyAfterDelay());
        }

        else
        {
            timer = 0;
        }
    }
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
