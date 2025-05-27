using UnityEngine;
using System.Collections;

public class RangedEnemy : EnemyLogic
{
    [Header("Ranged Settings")]
    public int damage = 1; // Damage dealt to the player
    public float attackCooldown = 4f; // Time between attacks
    private Coroutine attackCoroutine;

    [Header("Laser Beam Settings")]
    public LineRenderer laserBeam;
    public Color startColor = new Color(1, 0, 0, 0); // Faint red
    public Color endColor = new Color(1, 0, 0, 1);   // Bright red
    public float laserChargeTime = 1f;
    private bool isAttacking = false;


    // Called when the script instance is being loaded
    protected override void Start()
    {
        base.Start(); // Call base class Start method
        if (attackCoroutine == null)
            attackCoroutine = StartCoroutine(RangedAttackRoutine());
    }

    // Defines behavior when engaging the player
    protected override void EngageBehavior()
    {
        if (moveDirection.magnitude > 0.01f && laserBeam.enabled)
        {
            isAttacking = false;
            laserBeam.enabled = false;
        }
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
        while (true)
        {
            yield return new WaitUntil(() => distanceToPlayer >= minDistanceToPlayer && hasLineOfSight && moveDirection.magnitude < 0.01f && !isAttacking);
            isAttacking = true;

            float timer = 0f;
            while (timer < laserChargeTime)
            {
                // Stop if enemy starts moving or loses sight
                if (moveDirection.magnitude > 0.01f || !hasLineOfSight)
                {
                    isAttacking = false;
                    laserBeam.enabled = false;

                    // Let the coroutine loop start again immediately
                    yield return null;
                    continue; // Restart the outer loop from the top
                }

                // Update beam visuals
                float t = timer / laserChargeTime;
                laserBeam.enabled = true;

                Vector2 directionToPlayer = (Player.transform.position - transform.position).normalized;
                Vector2 beamStart = (Vector2)transform.position + directionToPlayer * 0.5f;
                laserBeam.SetPosition(0, beamStart);
                laserBeam.SetPosition(1, Player.transform.position);

                Color currentColor = Color.Lerp(startColor, endColor, t);
                laserBeam.startColor = currentColor;
                laserBeam.endColor = currentColor;

                timer += Time.deltaTime;
                yield return null;
            }

            if (isAttacking) // Only damage if we finished charging
            {
                DamagePlayer(damage);
                laserBeam.enabled = false;
                isAttacking = false;
                yield return new WaitForSeconds(attackCooldown);
            }

            isAttacking = false;
        }
    }

}