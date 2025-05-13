using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
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

    public void ContinueButton()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
