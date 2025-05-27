using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    private AudioManager audioManager;
    private PlayerController playerController;

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        audioManager.TogglePausedAudio(true);
        playerController.ControlsEnabled = false;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        audioManager.TogglePausedAudio(false);
        playerController.ControlsEnabled = true;
        Time.timeScale = 1;
    }

    public void ContinueButton()
    {
        gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
