using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public string mainMenuScene, options;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        GameManager.instance.PauseUnpause();
    }

    public void MainMenu()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;
    }

     public void Options()
    {
        SceneManager.LoadScene(options);

        PlayerPrefs.SetString("CurrentLevel", "");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
