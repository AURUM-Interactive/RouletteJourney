using UnityEngine;

public class ThrowableCardController : MonoBehaviour
{
    public GameObject CardProjectile;

    [SerializeField]
    private int baseManaUse = 2;

    private Camera mainCamera;
    private PlayerMain playerMainScript;

    Vector3 mousePosition;
    bool primaryFire;
    bool secondaryFire;

    private void Start()
    {
        mainCamera = Camera.main;
        playerMainScript = this.transform.parent.gameObject.GetComponent<PlayerMain>();
    }

    void Update()
    {
        UpdateMousePostion();

        if (Input.GetMouseButtonDown(0))
        {
            primaryFire = true;
        }

        else if (Input.GetMouseButtonDown(1))
        {
            secondaryFire = true;
        }
    }

    private void FixedUpdate()
    {
        if (primaryFire)
        {
            primaryFire = false;

            PrimaryFire();

        }
        else if (secondaryFire) 
        {
            secondaryFire = false;

            SecondaryFire();
        }
    }

    void PrimaryFire()
    {
        if (playerMainScript.mana < baseManaUse) return;

        playerMainScript.mana -= baseManaUse;

        Quaternion cardRotation = Quaternion.Euler(0, 0, GetAngleToCursorDegrees());

        GameObject projectile = Instantiate(CardProjectile, transform.position, cardRotation);
    }

    void SecondaryFire()
    {
        if (playerMainScript.mana < 3 * baseManaUse) return;

        playerMainScript.mana -= baseManaUse * 3;

        float cursorAngle = GetAngleToCursorDegrees();

        float cardGapInDegrees = 10;

        float damageModifier = 0.5f;

        Quaternion leftCardRotation = Quaternion.Euler(0, 0, cursorAngle + cardGapInDegrees);
        var projectileLeft = Instantiate(CardProjectile, transform.position, leftCardRotation);
        projectileLeft.GetComponent<ProjectileScript>().ModifyDamageMultiplicative(damageModifier);

        Quaternion centerCardRotation = Quaternion.Euler(0, 0, cursorAngle);
        var projectileCenter = Instantiate(CardProjectile, transform.position, centerCardRotation);
        projectileCenter.GetComponent<ProjectileScript>().ModifyDamageMultiplicative(damageModifier);

        Quaternion rightCardRotation = Quaternion.Euler(0, 0, cursorAngle - cardGapInDegrees);
        var projectileRight = Instantiate(CardProjectile, transform.position, rightCardRotation);
        projectileRight.GetComponent<ProjectileScript>().ModifyDamageMultiplicative(damageModifier);
    }

    float GetAngleToCursorDegrees()
    {
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        return angle;
    }

    void UpdateMousePostion()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
    }
}
