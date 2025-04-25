using System.Collections;
using TMPro;
using UnityEngine;

public abstract class EnemyLogic : MonoBehaviour
{
    [Header("Movement Settings")]
    public float idleSpeed = 0.5f;
    public float activeSpeed = 3f;
    public float minDistanceToPlayer = 3f;
    public float reactionTime = 0.5f;
    public GameObject DamagePopUp;
    public LayerMask RaycastShouldIgnore;

    protected GameObject Player;
    protected bool hasLineOfSight = false;
    protected Vector2 moveDirection;
    protected float timer;
    protected float distanceToPlayer;
    protected float bufferZone = 0.1f;
    protected float pauseBeforeNextMove = 0.5f;
    private Coroutine retreatCoroutine;

    protected virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0.5f;
            MoveAround();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (hasLineOfSight)
        {
            EngageBehavior();
        }
        else
        {
            Roam();
        }

        UpdateLineOfSight();
    }

    protected void Roam()
    {
        transform.position += (Vector3)moveDirection * idleSpeed * Time.deltaTime;
    }

    protected void MoveAround()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        transform.position = Vector2.MoveTowards(transform.position, moveDirection, idleSpeed * Time.deltaTime);
    }

    protected void UpdateLineOfSight()
    {
        Vector2 direction = Player.transform.position - transform.position;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, 9999f, ~RaycastShouldIgnore);

        hasLineOfSight = ray.collider != null && ray.collider.CompareTag("Player");

        if (hasLineOfSight) { Debug.DrawRay(transform.position, Player.transform.position - transform.position, Color.red); }
        else { Debug.DrawRay(transform.position, Player.transform.position - transform.position, Color.white); }

    }

    protected abstract void EngageBehavior();

    protected IEnumerator RetreatAfterDelay()
    {
        yield return new WaitForSeconds(reactionTime);

        Vector2 escapeDirection = GetRandomEscapeDirection();
        float retreatTimer = 0f;

        while (retreatTimer < 0.7f)
        {
            transform.position += (Vector3)(escapeDirection * activeSpeed * Time.deltaTime);
            retreatTimer += Time.deltaTime;
            yield return null;
        }

        retreatCoroutine = null;
        yield return new WaitForSeconds(pauseBeforeNextMove);
    }
    private Vector2 GetRandomEscapeDirection()
    {
        Vector2 directionAway = (transform.position - Player.transform.position).normalized;
        int choice = Random.Range(1, 4);
        switch (choice)
        {
            case 1:
                directionAway = Quaternion.Euler(0, 0, 45) * directionAway;
                break;
            case 3:
                directionAway = Quaternion.Euler(0, 0, -45) * directionAway;
                break;
        }
        return directionAway;
    }
    protected void TryRetreat()
    {
        if (distanceToPlayer < minDistanceToPlayer - bufferZone && retreatCoroutine == null)
        {
            retreatCoroutine = StartCoroutine(RetreatAfterDelay());
        }
    }
    protected void ApproachPlayer()
    {
        if (distanceToPlayer > minDistanceToPlayer + bufferZone)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                Player.transform.position,
                activeSpeed * Time.fixedDeltaTime);
        }
        else
        {
            TryRetreat();
        }
    }


    protected void DamagePlayer(int damage)
    {
        var playerMain = Player.GetComponent<PlayerMain>();
        if (playerMain != null)
            playerMain.TakeDamage(damage);
    }
}
