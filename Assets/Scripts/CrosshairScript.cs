using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    SpriteRenderer cursorSprite;
    Vector2 mousePosition;

    void Awake()
    {
        Cursor.visible = false;
        cursorSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        RaycastHit2D mouseRaycast = Physics2D.Raycast(this.gameObject.transform.position, new Vector2());
        
        if (mouseRaycast.collider == null)
        {
            ChangeSpriteColor(Color.white);
        }
        else if (mouseRaycast.collider.CompareTag("Enemy"))
        {
            ChangeSpriteColor(Color.red);
        }
    }

    private void ChangeSpriteColor(Color color)
    {
        if (cursorSprite.color != color)
        {
            cursorSprite.color = color;
        }
    }
}
