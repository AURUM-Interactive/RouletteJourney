using UnityEngine;
using System.Collections;

public class MeleeEnemy : EnemyLogic
{
    [Header("Melee Settings")]
    public int damage = 1;
    public float attackCooldown = 1f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(MeleeAttackRoutine());
    }

    protected override void EngageBehavior()
    {
        ApproachPlayer();
    }

    private IEnumerator MeleeAttackRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => distanceToPlayer <= 1f && hasLineOfSight);
            DamagePlayer(damage);
            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
