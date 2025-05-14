using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenuScript : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void OnEnable()
    {
        audioManager.TogglePausedAudio(true);
    }

    private void OnDisable()
    {
        audioManager.TogglePausedAudio(false);
    }

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
