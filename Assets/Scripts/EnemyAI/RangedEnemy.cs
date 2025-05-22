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
            StopMovement(); // Idle when in optimal range
        }
    }

    protected override void AnimationMovement()
    {
        if (moveDirection.magnitude > 0.01f)
        {
            Vector2 normalizedDir = moveDirection.normalized;
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
                // Use vertical direction to decide
                animator.SetFloat("MoveX", 1);
                animator.SetFloat("MoveY", 0);
            }
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
