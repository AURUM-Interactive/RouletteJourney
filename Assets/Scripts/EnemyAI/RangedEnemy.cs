using UnityEngine;
using System.Collections;

public class RangedEnemy : EnemyLogic
{
    [Header("Ranged Settings")]
    public int damage = 1; // Damage dealt to the player
    public float attackCooldown = 4f; // Time between attacks

    // Called when the script instance is being loaded
    protected override void Start()
    {
        base.Start(); // Call base class Start method
        StartCoroutine(RangedAttackRoutine()); // Start the ranged attack coroutine
    }

    // Defines behavior when engaging the player
    protected override void EngageBehavior()
    {
        if (distanceToPlayer < minDistanceToPlayer - bufferZone)
        {
            TryRetreat();
        }
        else if (distanceToPlayer > minDistanceToPlayer + bufferZone)
        {
            MoveTowardPlayer();
        }
        else
        {
            // In optimal range - stop all movement and stay idle
            StopMovement();
            // Reset the timer to prevent MoveAround from being called
            timer = 0.5f;
        }
    }

    protected override void AnimationMovement()
    {
        Vector2 toPlayer = Player.transform.position - transform.position;

        // Only update animations if we're actually supposed to be moving
        if (moveDirection.magnitude > 0.01f)
        {
            Vector2 normalizedDir = moveDirection.normalized;
            animator.SetBool("Idle", false);

            // For straight up or down movement, use left animation
            if (Mathf.Abs(normalizedDir.x) < 0.001f) // Moving almost perfectly vertical
            {
                animator.SetFloat("MoveX", -1); // Use left animation
            }
            else
            {
                // Use the actual X direction for left/right movement
                animator.SetFloat("MoveX", normalizedDir.x);
            }
        }
        else
        {
            animator.SetBool("Idle", true);

            if (toPlayer.x < 0)
                animator.SetFloat("MoveX", -1);
            else
                animator.SetFloat("MoveX", 1);

        }
    }

    // Coroutine for ranged attack logic
    private IEnumerator RangedAttackRoutine()
    {
        while (true) // Infinite loop for continuous behavior
        {
            // Wait until the player is within range and visible
            yield return new WaitUntil(() => distanceToPlayer >= minDistanceToPlayer && hasLineOfSight);
            DamagePlayer(damage); // Deal damage to the player
            yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown duration
        }
    }
}