using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScreen : MonoBehaviour
{
    public string lvl1,lvl2, returnTo;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Level1()
    {
        AudioManager.instance.StopBGM();
        SceneManager.LoadScene(lvl1);
        PlayerPrefs.SetString("CurrentLevel", "");
    }
    public void Level2()
    {
        AudioManager.instance.StopBGM();
        SceneManager.LoadScene(lvl2);
        PlayerPrefs.SetString("CurrentLevel", "");
    }
    public void ReturnTo()
    {
        SceneManager.LoadScene(returnTo);
        PlayerPrefs.SetString("CurrentLevel", "");
    }
}
