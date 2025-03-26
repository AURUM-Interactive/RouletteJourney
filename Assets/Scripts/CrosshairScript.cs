using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    SpriteRenderer cursorSprite;

    BoxCollider2D boxCollider;

    Vector2 mousePosition;
    void Awake()
    {
        Cursor.visible = false;
        boxCollider = GetComponent<BoxCollider2D>();
        cursorSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            cursorSprite.color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cursorSprite.color = Color.white;
    }
}
