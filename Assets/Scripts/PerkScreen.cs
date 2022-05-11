using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerkScreen : MonoBehaviour
{
    public string goToLevelScreen;

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

    public void LowerHealth()
    {
        Debug.Log("Lower health chosen");
    }

    public void NoGuns()
    {
        Debug.Log("No guns");

    }

    public void EnemySpawnRate()
    {
        Debug.Log("Enemies spawn rate lowered");

    }

    public void LooseHealth()
    {
        Debug.Log("Loosing health continously");

    }

    public void StartGame()
    {
        SceneManager.LoadScene(goToLevelScreen);

        PlayerPrefs.SetString("CurrentLevel", "");

    }
}
