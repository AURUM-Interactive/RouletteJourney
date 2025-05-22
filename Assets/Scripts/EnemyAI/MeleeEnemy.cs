using UnityEngine;
using System.Collections;

public class MeleeEnemy : EnemyLogic
{
    [Header("Melee Settings")]
    public int damage = 1; // Damage dealt to the player
    public float attackCooldown = 1f; // Cooldown time between attacks

    // Called when the script instance is being loaded
    protected override void Start()
    {
        base.Start(); // Call base class Start method
        StartCoroutine(MeleeAttackRoutine()); // Start the melee attack coroutine
    }

    // Defines behavior when engaging the player
    protected override void EngageBehavior()
    {
        if (distanceToPlayer > minDistanceToPlayer + bufferZone)
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
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", normalizedDir.y > 0 ? 1 : -1);
            }
        }
    }

    // Coroutine to handle melee attack logic
    private IEnumerator MeleeAttackRoutine()
    {
        while (true) // Infinite loop for continuous attack checks
        {
            // Wait until the player is within attack range and visible
            yield return new WaitUntil(() => distanceToPlayer <= 1.5f && hasLineOfSight);
            DamagePlayer(damage); // Deal damage to the player
            yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown duration
        }
    }
}
