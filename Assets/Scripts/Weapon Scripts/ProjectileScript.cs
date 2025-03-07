using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private float baseDamage = 10;
    public float BaseDamage => baseDamage;
    
    // In World Units / Second
    [SerializeField]
    private float baseSpeed = 2;
    public float BaseSpeed => baseSpeed;

    [SerializeField]
    private float baseRange = 5;
    public float BaseRange => baseRange;

    Rigidbody2D rb;
    Vector3 origin;
    //Vector3 aimDirection;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        origin = transform.position;
    }

    public void Start()
    {
        // 1 mass * 50 acceleration = 1 unit / second
        rb.AddForce(50 * transform.right * baseSpeed);
    }

    public void FixedUpdate()
    {
        //rb.MovePosition(transform.position + aimDirection * baseSpeed * Time.deltaTime);

        float distanceTravelled = Vector3.Distance(origin, rb.position);
        if(distanceTravelled > baseRange)
        {
            Destroy(this.gameObject);
        }
    }

    public void Setup(Vector3 mousePosition)
    {
        //aimDirection = (mousePosition - transform.position).normalized;
        //float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //rb.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
