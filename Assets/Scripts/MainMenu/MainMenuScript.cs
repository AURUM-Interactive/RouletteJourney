using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    public string GameScene = "SampleScene";

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
        //TODO: open settings panel
    }

    public void ClickExit()
    {
        Application.Quit();
    }
}
