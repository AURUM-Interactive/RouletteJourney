using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Slider volumeSlider;

    void Start()
    {
        // Load and apply saved resolution
        string savedRes = PlayerPrefs.GetString("resolution", "1920x1080");
        SetResolution(savedRes);

        int index = resolutionDropdown.options.FindIndex(opt => opt.text == savedRes);
        if (index >= 0)
            resolutionDropdown.value = index;

        // Load and apply saved volume
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
    }

    public void ApplySettings()
    {
        // Apply resolution
        string selectedRes = resolutionDropdown.options[resolutionDropdown.value].text;
        SetResolution(selectedRes);
        PlayerPrefs.SetString("resolution", selectedRes);

        // Apply volume
        float volume = volumeSlider.value;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);

        PlayerPrefs.Save();

        this.gameObject.SetActive(false);
    }

    private void SetResolution(string res)
    {
        string[] dims = res.Split('x');
        if (dims.Length == 2 &&
            int.TryParse(dims[0], out int width) &&
            int.TryParse(dims[1], out int height))
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }
}
