using UnityEngine;
using System.Collections;

/// <summary>
/// Ranged enemy that attacks the player with a charging laser beam.
/// Maintains distance from the player and fires after a charge-up period.
/// </summary>
public class RangedEnemy : EnemyLogic
{
    #region Attack Settings
    [Header("Ranged Attack Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 4f;

    [Header("Laser Beam Settings")]
    [SerializeField] private LineRenderer laserBeam;
    [SerializeField] private Color startColor = new Color(1, 0, 0, 0); // Transparent red
    [SerializeField] private Color endColor = new Color(1, 0, 0, 1);   // Opaque red
    [SerializeField] private float laserChargeTime = 1f;
    [SerializeField] private float beamStartOffset = 0.5f; // Distance from enemy center to beam start
    #endregion

    #region Private Fields
    private Coroutine attackCoroutine;
    private bool isAttacking = false;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Initialize the ranged enemy and start the attack routine.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // Start the continuous attack routine
        attackCoroutine = StartCoroutine(RangedAttackRoutine());
    }

    /// <summary>
    /// Clean up coroutines when the enemy is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
    #endregion

    #region Enemy Behavior Override Methods
    /// <summary>
    /// Defines how the enemy behaves when engaging the player.
    /// Maintains optimal distance and cancels attacks when moving.
    /// </summary>
    protected override void EngageBehavior()
    {
        // Cancel laser attack if the enemy starts moving or retreating
        if (ShouldCancelAttack())
        {
            CancelCurrentAttack();
        }

        // Determine movement based on distance to player
        if (distanceToPlayer < minDistanceToPlayer - bufferZone)
        {
            // Too close - retreat to maintain distance
            TryRetreat();
        }
        else if (distanceToPlayer > minDistanceToPlayer + bufferZone)
        {
            // Too far - move closer to get in range
            MoveTowardPlayer();
        }
        else
        {
            // In optimal range - stop and prepare to attack
            StopMovement();
            timer = 0.5f; // Prevent immediate idle wandering
        }
    }

    /// <summary>
    /// Controls animation states based on movement and player position.
    /// </summary>
    protected override void AnimationMovement()
    {
        Vector2 directionToPlayer = Player.transform.position - transform.position;

        if (IsMoving())
        {
            // Set movement animations
            SetMovementAnimation();
        }
        else
        {
            // Set idle animation and face the player
            SetIdleAnimation(directionToPlayer);
        }
    }
    #endregion

    #region Attack System
    /// <summary>
    /// Main attack coroutine that handles the charge-up and firing sequence.
    /// </summary>
    private IEnumerator RangedAttackRoutine()
    {
        while (true)
        {
            // Wait for optimal attack conditions
            yield return new WaitUntil(CanStartAttack);

            // Begin attack sequence
            yield return StartCoroutine(ExecuteAttackSequence());
        }
    }

    /// <summary>
    /// Executes the complete attack sequence: charge up, fire, and cleanup.
    /// </summary>
    private IEnumerator ExecuteAttackSequence()
    {
        isAttacking = true;
        float chargeTimer = 0f;

        // Charging phase
        while (chargeTimer < laserChargeTime)
        {
            // Check if attack should be cancelled mid-charge
            if (ShouldCancelAttack())
            {
                CancelCurrentAttack();
                yield break;
            }

            // Update laser beam visual during charge
            UpdateLaserBeamVisual(chargeTimer / laserChargeTime);

            chargeTimer += Time.deltaTime;
            yield return null;
        }

        // Fire the laser (deal damage)
        if (isAttacking)
        {
            FireLaser();

            // Wait for cooldown before next attack
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    /// <summary>
    /// Updates the visual appearance of the laser beam during charging.
    /// </summary>
    private void UpdateLaserBeamVisual(float chargeProgress)
    {
        if (laserBeam == null) return;

        // Enable and position the laser beam
        laserBeam.enabled = true;

        // Calculate beam start position (slightly in front of enemy)
        Vector2 directionToPlayer = (Player.transform.position - transform.position).normalized;
        Vector2 beamStart = (Vector2)transform.position + directionToPlayer * beamStartOffset;

        // Set beam positions
        laserBeam.SetPosition(0, beamStart);
        laserBeam.SetPosition(1, Player.transform.position);

        // Interpolate color based on charge progress
        Color currentColor = Color.Lerp(startColor, endColor, chargeProgress);
        laserBeam.startColor = currentColor;
        laserBeam.endColor = currentColor;
    }

    /// <summary>
    /// Fires the laser and deals damage to the player.
    /// </summary>
    private void FireLaser()
    {
        DamagePlayer(damage);
        CancelCurrentAttack();
    }

    /// <summary>
    /// Cancels the current attack and resets visual elements.
    /// </summary>
    private void CancelCurrentAttack()
    {
        isAttacking = false;

        if (laserBeam != null)
        {
            laserBeam.enabled = false;
        }
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Checks if the enemy can start a new attack.
    /// </summary>
    private bool CanStartAttack()
    {
        return distanceToPlayer >= minDistanceToPlayer &&
               hasLineOfSight &&
               !IsMoving() &&
               !isAttacking &&
               !isRetreating;
    }

    /// <summary>
    /// Determines if the current attack should be cancelled.
    /// </summary>
    private bool ShouldCancelAttack()
    {
        return (IsMoving() || !hasLineOfSight || isRetreating) &&
               (laserBeam != null && laserBeam.enabled);
    }

    /// <summary>
    /// Checks if the enemy is currently moving.
    /// </summary>
    private bool IsMoving()
    {
        return moveDirection.magnitude > 0.01f;
    }

    /// <summary>
    /// Sets movement animation based on current direction.
    /// </summary>
    private void SetMovementAnimation()
    {
        Vector2 normalizedDirection = moveDirection.normalized;
        animator.SetBool("Idle", false);

        // Use left animation for vertical movement, otherwise use horizontal direction
        if (Mathf.Abs(normalizedDirection.x) < 0.001f)
        {
            animator.SetFloat("MoveX", -1);
        }
        else
        {
            animator.SetFloat("MoveX", normalizedDirection.x);
        }
    }

    /// <summary>
    /// Sets idle animation and faces the player.
    /// </summary>
    private void SetIdleAnimation(Vector2 directionToPlayer)
    {
        animator.SetBool("Idle", true);
        animator.SetFloat("MoveX", directionToPlayer.x < 0 ? -1 : 1);
    }
    #endregion
}