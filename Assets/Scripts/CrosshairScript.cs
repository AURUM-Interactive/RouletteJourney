using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    SpriteRenderer cursorSprite;
    Vector2 mousePosition;

    PlayerMain PlayerMain;
    void Awake()
    {
        Cursor.visible = false;
        cursorSprite = GetComponent<SpriteRenderer>();
        PlayerMain = GameObject.Find("Player").GetComponent<PlayerMain>();
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

        if(PlayerMain.health <= 0)
        {
            Cursor.visible = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
