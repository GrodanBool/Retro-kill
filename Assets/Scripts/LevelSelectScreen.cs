using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScreen : MonoBehaviour
{
    public string lvl1, lvl2, lvl3, lvl4, lvl5, returnTo;

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
        SceneManager.LoadScene(lvl1);
        PlayerPrefs.SetString("CurrentLevel", "");
        Debug.Log("click");
    }

    public void ReturnTo()
    {
        SceneManager.LoadScene(returnTo);
        PlayerPrefs.SetString("CurrentLevel", "");
    }
}