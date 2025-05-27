using TMPro;
using System.Collections;
using UnityEngine;


public class SuicideEnemy : EnemyLogic
{
    [Header("Suicide Settings")]
    public int damage = 1;

    // Called when the script instance is being loaded
    protected override void Start()
    {
        base.Start(); // Call base class Start method
    }

    // Called at fixed intervals, used for physics updates
    protected override void FixedUpdate()
    {
        Roam(); // Handle roaming behavior
    }

    // Triggered when a collision occurs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DamagePlayer(damage); // Inflict damage to the player
            StartCoroutine(DestroyAfterDelay()); // Start destruction sequence
        }
        else
        {
            timer = 0; // Reset timer if collision is not with the player
        }
    }

    protected override void AnimationMovement()
    {
        // SuicideEnemy does not have any animations
    }

    // Override EngageBehavior to disable active engagement
    protected override void EngageBehavior()
    {
        // SuicideEnemy does not engage the player actively
    }

    // Coroutine to destroy the enemy after a short delay
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Wait for 0.5 seconds
        var popUp = Instantiate(DamagePopUp, transform.position, Quaternion.identity); // Create damage popup
        popUp.GetComponent<TextMeshPro>().text = "Poof"; // Set popup text
        Destroy(gameObject); // Destroy the enemy object
    }
}
