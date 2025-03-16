using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    Vector2 mousePosition;
    void Awake()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }
}
