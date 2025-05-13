using UnityEngine;

public class ExitDoorScript : MonoBehaviour
{


    [SerializeField]
    GameObject exitPopup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            exitPopup.SetActive(true);
        }
    }
}
