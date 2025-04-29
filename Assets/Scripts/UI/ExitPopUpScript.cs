using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPopUpScript : MonoBehaviour
{
    public void StayButton()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
