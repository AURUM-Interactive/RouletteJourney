using TMPro;
using System.Collections;
using UnityEngine;


public class SuicideEnemy : EnemyLogic
{
    [Header("Suicide Settings")]
    public int damage = 1;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        Roam();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DamagePlayer(damage);
            StartCoroutine(DestroyAfterDelay());
        }
        else
        {
            timer = 0;
        }
    }

    // Override EngageBehavior to do nothing
    protected override void EngageBehavior()
    {
        // SuicideEnemy does not engage the player actively
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        var popUp = Instantiate(DamagePopUp, transform.position, Quaternion.identity);
        popUp.GetComponent<TextMeshPro>().text = "Poof";
        Destroy(gameObject);
    }
}
