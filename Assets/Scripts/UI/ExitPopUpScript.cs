using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPopUpScript : MonoBehaviour
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
    public void StayButton()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    public void ExitButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
