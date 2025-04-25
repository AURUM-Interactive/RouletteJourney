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
        ApproachPlayer(); // Move towards the player
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
