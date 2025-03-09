using System;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private float baseDamage = 10;

    private float bonusDamage = 0; // goes first
    private float damageModifier = 1; // goes second
    
    // In World Units / Second
    [SerializeField]
    private float baseSpeed = 2;

    [SerializeField]
    private float baseRange = 5;

    Rigidbody2D projectileRB;
    Vector3 origin;

    public void Awake()
    {
        projectileRB = GetComponent<Rigidbody2D>();
        origin = transform.position;
    }

    public void Start()
    {
        // 1 mass * 50 acceleration = 1 unit / second
        projectileRB.AddForce(50 * transform.right * baseSpeed);
    }

    public void FixedUpdate()
    {
        float distanceTravelled = Vector3.Distance(origin, projectileRB.position);
        if(distanceTravelled > baseRange)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Modifies projectile damage by changing the damage multiplicative modifier. Example: base damage = 10, modifier = 1, call this(1.5) to change modifier = 1 * 1.5 and final damage to 10 * 1.5 = 15
    /// </summary>
    public void ModifyDamageMultiplicative(float damageCoefficient)
    {
        damageModifier *= damageCoefficient;
    }

    /// <summary>
    /// Modifies projectile damage by changing the damage multiplicative modifier. Example: base damage = 10, modifier = 1, call this(1.5) to change modifier = 1 * 1.5 and final damage to 10 * 1.5 = 15
    /// </summary>
    public void ModifyDamageAdditive(float damageValue)
    {
        bonusDamage *= damageValue;
    }

    private float CalculateDamage()
    {
        return (baseDamage + bonusDamage) * damageModifier;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player") && !collision.CompareTag("PlayerProjectile"))
        {
            Destroy(this.gameObject);
            Debug.Log($"Dealt {CalculateDamage()} damage to {collision.gameObject}");
        }
    }
}
