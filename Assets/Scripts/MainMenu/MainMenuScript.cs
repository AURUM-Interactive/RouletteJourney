using System.Collections;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public string GameScene;

    [SerializeField]
    GameObject SettingsScreen;

    [SerializeField]
    GameObject MainMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickPlay()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void ClickSettings()
    {
        SettingsScreen.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void ClickExit()
    {
        Application.Quit();
    }
}
