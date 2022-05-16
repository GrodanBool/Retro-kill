using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string start, online, options, highscore;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
    }

    public void StartGame()
    {
        SceneManager.LoadScene(start);

        PlayerPrefs.SetString("CurrentLevel", "");

    }

    public void StartOnlineGame()
    {
        SceneManager.LoadScene(online);

        PlayerPrefs.SetString("CurrentLevel", "");

    }

    public void Options()
    {
        SceneManager.LoadScene(options);

        PlayerPrefs.SetString("CurrentLevel", "");

    }

    public void Highscore()
    {
        SceneManager.LoadScene(highscore);

        PlayerPrefs.SetString("CurrentLevel", "");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
