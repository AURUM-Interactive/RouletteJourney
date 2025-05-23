using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;

    AudioSource musicAudioSource;

    [SerializeField]
    AudioMixerSnapshot gameRunningSnapshot, gamePausedSnapshot;

    [SerializeField]
    float PauseInTime = 0.5f;
    [SerializeField]
    float PauseOutTime = 0.25f;

    [Header("Music Tracks")]

    [SerializeField]
    AudioClip GameplayTheme;

    [SerializeField]
    AudioClip DeathTheme;

    //public static AudioManager Instance;

    private void Awake()
    {
        //if (Instance != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();
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

    public void StartGameplayTheme()
    {
        musicAudioSource.clip = GameplayTheme;
    }

    public void StartDeathTheme()
    {
        musicAudioSource.clip = DeathTheme;
        musicAudioSource.volume = 1;
        musicAudioSource.PlayDelayed(0.5f);
    }
}
