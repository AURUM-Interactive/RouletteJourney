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

    protected Animator animator;
    private Vector2 previousPosition;

    // Called when the script instance is being loaded. Initializes the Player reference.
    protected virtual void Start()
    {
        // Find the player object by tag.
        Player = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    // Called once per frame. Updates the distance to the player and handles movement logic.
    protected virtual void Update()
    {
        // Calculate the distance to the player.
        distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);

        // Decrease the timer.
        timer -= Time.deltaTime;

        // If the timer reaches zero, reset it and move around.
        if (timer <= 0)
        {
            timer = 0.5f;
            MoveAround();
            AnimationMovement();
        }
    }

    protected void AnimationMovement()
    {
        Vector2 currentPosition = transform.position;
        Vector2 movement = currentPosition - previousPosition;

        if (movement.magnitude > 0.01f)
        {
            Vector2 normalizedDir = movement.normalized;
            float angle = Mathf.Atan2(normalizedDir.y, normalizedDir.x) * Mathf.Rad2Deg;
            angle = (angle + 360f) % 360f;

            bool isRight = angle <= 30f || angle >= 330f;
            bool isLeft = angle >= 150f && angle <= 210f;

            if (isRight)
            {
                animator.SetFloat("MoveX", 1);
                animator.SetFloat("MoveY", 0);
            }
            else if (isLeft)
            {
                animator.SetFloat("MoveX", -1);
                animator.SetFloat("MoveY", 0);
            }
            else
            {
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", normalizedDir.y > 0 ? 1 : -1);
            }
        }

        previousPosition = currentPosition;
    }

    // Called at a fixed time interval. Handles enemy behavior based on line of sight.
    protected virtual void FixedUpdate()
    {
        // If the enemy has line of sight, engage the player.
        if (hasLineOfSight)
        {
            EngageBehavior();
        }
        else
        {
            // Otherwise, roam around.
            Roam();
            AnimationMovement();
        }

        // Update the line of sight status.
        UpdateLineOfSight();
    }

    // Handles roaming behavior when the enemy has no specific target.
    protected void Roam()
    {
        // Move in the current direction at idle speed.
        transform.position += (Vector3)moveDirection * idleSpeed * Time.deltaTime;
    }

    // Updates the enemy's movement direction randomly.
    protected void MoveAround()
    {
        // Pick a random direction and move towards it.
        moveDirection = Random.insideUnitCircle.normalized;
        transform.position = Vector2.MoveTowards(transform.position, moveDirection, idleSpeed * Time.deltaTime);
    }

    // Updates whether the enemy has a line of sight to the player.
    protected void UpdateLineOfSight()
    {
        // Calculate the direction to the player.
        Vector2 direction = Player.transform.position - transform.position;

        // Perform a raycast to check for obstacles.
        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, 9999f, ~RaycastShouldIgnore);

        // Determine if the ray hit the player.
        hasLineOfSight = ray.collider != null && ray.collider.CompareTag("Player") && Player.GetComponent<PlayerMain>().health > 0;

        // Draw debug rays to visualize line of sight.
        if (hasLineOfSight)
        {
            Debug.DrawRay(transform.position, Player.transform.position - transform.position, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, Player.transform.position - transform.position, Color.white);
        }
    }

    // Abstract method to define specific behavior when engaging the player.
    protected abstract void EngageBehavior();

    // Coroutine to handle retreating behavior after a delay.
    protected IEnumerator RetreatAfterDelay()
    {
        // Wait for the reaction time before retreating.
        yield return new WaitForSeconds(reactionTime);

        // Get a random escape direction.
        Vector2 escapeDirection = GetRandomEscapeDirection();
        float retreatTimer = 0f;

        // Move in the escape direction for a short duration.
        while (retreatTimer < 0.7f)
        {
            transform.position += (Vector3)(escapeDirection * activeSpeed * Time.deltaTime);
            retreatTimer += Time.deltaTime;
            yield return null;
        }

        // Reset the retreat coroutine and wait before the next move.
        retreatCoroutine = null;
        yield return new WaitForSeconds(pauseBeforeNextMove);
    }

    // Calculates a random direction to retreat away from the player.
    private Vector2 GetRandomEscapeDirection()
    {
        // Calculate the direction away from the player.
        Vector2 directionAway = (transform.position - Player.transform.position).normalized;

        // Randomly adjust the direction slightly.
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

    // Attempts to initiate a retreat if the player is too close.
    protected void TryRetreat()
    {
        // Check if the player is within the retreat range and no retreat is ongoing.
        if (distanceToPlayer < minDistanceToPlayer - bufferZone && retreatCoroutine == null)
        {
            // Start the retreat coroutine.
            retreatCoroutine = StartCoroutine(RetreatAfterDelay());
        }
    }

    // Moves the enemy closer to the player if they are outside the minimum distance.
    protected void ApproachPlayer()
    {
        // If the player is far, move closer.
        if (distanceToPlayer > minDistanceToPlayer + bufferZone)
        {
            Vector2 targetDirection = (Player.transform.position - transform.position).normalized;
            moveDirection = targetDirection;

            transform.position = Vector2.MoveTowards(
                transform.position,
                Player.transform.position,
                activeSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Otherwise, attempt to retreat.
            TryRetreat();
        }
    }

    // Inflicts damage on the player by calling their TakeDamage method.
    protected void DamagePlayer(int damage)
    {
        // Get the player's main script and apply damage if it exists.
        var playerMain = Player.GetComponent<PlayerMain>();
        if (playerMain != null)
            playerMain.TakeDamage(damage);
    }
}
