using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;

    [SerializeField]
    AudioMixerSnapshot gameRunningSnapshot, gamePausedSnapshot;

    [SerializeField]
    float PauseInTime = 0.5f;
    [SerializeField]
    float PauseOutTime = 0.25f;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TogglePausedAudio(bool value)
    {
        if(value)
        {
            gamePausedSnapshot.TransitionTo(PauseInTime);  
        }
        else
        {
            gameRunningSnapshot.TransitionTo(PauseOutTime);
        }
    }
}
