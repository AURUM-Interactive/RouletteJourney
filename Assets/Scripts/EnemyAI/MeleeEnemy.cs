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
        ApproachPlayer(); // Move towards the player
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
