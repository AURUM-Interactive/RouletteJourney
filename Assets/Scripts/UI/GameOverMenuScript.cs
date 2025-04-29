using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenuScript : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
