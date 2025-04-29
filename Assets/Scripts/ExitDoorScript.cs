using UnityEngine;

public class ExitDoorScript : MonoBehaviour
{
    private GameObject exitPopup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        exitPopup = GameObject.Find("ExitPopup");
        exitPopup.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            exitPopup.SetActive(true);
        }
    }
}
