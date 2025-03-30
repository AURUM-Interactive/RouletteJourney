using UnityEngine;

public class CurrencyLootScript : MonoBehaviour
{

    private PlayerMain PlayerMain;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerMain = GameObject.Find("Player").GetComponent<PlayerMain>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            PlayerMain.chipCount++;
            Destroy(this.gameObject);
        }
    }
}
