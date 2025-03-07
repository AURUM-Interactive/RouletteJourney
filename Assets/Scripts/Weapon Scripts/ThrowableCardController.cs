using UnityEngine;

public class ThrowableCardController : MonoBehaviour
{
    public GameObject CardProjectile;

    [SerializeField]
    private Camera mainCamera;

    Vector3 mousePosition;
    bool primaryFire;
    bool secondaryFire;

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

            Quaternion cardRotation = Quaternion.Euler(0, 0, GetAngleToCursorDegrees());

            var projectile = Instantiate(CardProjectile, transform.position, cardRotation);
        }
        else if (secondaryFire) 
        {
            secondaryFire = false;

            Quaternion leftCardRotation = Quaternion.Euler(0, 0, GetAngleToCursorDegrees() + 10);
            var projectileLeft = Instantiate(CardProjectile, transform.position, leftCardRotation);

            Quaternion centerCardRotation = Quaternion.Euler(0, 0, GetAngleToCursorDegrees());
            var projectileCenter = Instantiate(CardProjectile, transform.position, centerCardRotation);

            Quaternion rightCardRotation = Quaternion.Euler(0, 0, GetAngleToCursorDegrees() - 10);
            var projectileRight = Instantiate(CardProjectile, transform.position, rightCardRotation);
        }
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
