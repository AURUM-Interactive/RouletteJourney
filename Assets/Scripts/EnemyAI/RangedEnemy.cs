using UnityEngine;
using System.Collections;

public class RangedEnemy : EnemyLogic
{
    [Header("Ranged Settings")]
    public int damage = 1;
    public float attackCooldown = 4f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(RangedAttackRoutine());
    }

    protected override void EngageBehavior()
    {
        ApproachPlayer();
    }

    private IEnumerator RangedAttackRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => distanceToPlayer >= minDistanceToPlayer && hasLineOfSight);
            DamagePlayer(damage);
            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
