using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class PerkScreen : MonoBehaviour
{
    public static PerkScreen instance;

    public string goToLevelScreen;

    GameManager manager;

    private string activeMod;

    [HideInInspector] public string activeMod1;

    [HideInInspector] public bool lhNotPressed = true;
    [HideInInspector] public bool esNotPressed = true;
    [HideInInspector] public bool lohNotPressed = true;
    [HideInInspector] public bool ngNotPressed = true;

    [HideInInspector] public string lowHealth;
    [HideInInspector] public string noGuns;
    [HideInInspector] public string enemySpawn;
    [HideInInspector] public string loseHealth;

    [Header("Button Binds")]
    public GameObject buttonLowHealth;
    public GameObject buttonNoGun;
    public GameObject buttonEnemySpawnRate;
    public GameObject buttonLoseHealth;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (activeMod1)
        {
            case "lh":
                if (lhNotPressed)
                {
                    lowHealth += "\nLower Health";
                    activeMod1 = "";
                    lhNotPressed = !lhNotPressed;
                }
                else
                {
                    lowHealth = "";
                    activeMod1 = "";
                    lhNotPressed = !lhNotPressed;
                }
                break;
            case "ng":
                if (ngNotPressed)
                {
                    noGuns += "\nNo Guns";
                    activeMod1 = "";
                    ngNotPressed = !ngNotPressed;
                }
                else
                {
                    noGuns = "";
                    activeMod1 = "";
                    ngNotPressed = !ngNotPressed;
                }
                break;
            case "es":
                if (esNotPressed)
                {
                    enemySpawn += "\nEnemy Spawn Rate";
                    activeMod1 = "";
                    esNotPressed = !esNotPressed;
                }
                else
                {
                    enemySpawn = "";
                    activeMod1 = "";
                    esNotPressed = !esNotPressed;
                }
                break;
            case "loh":
                if (lohNotPressed)
                {
                    loseHealth += "\nLose Health";
                    activeMod1 = "";
                    lohNotPressed = !lohNotPressed;
                }
                else
                {
                    loseHealth = "";
                    activeMod1 = "";
                    lohNotPressed = !lohNotPressed;
                }
                break;
            default:
                break;
        }

    }

    public void Continue()
    {
        PlayerPrefs.SetString("activemod", activeMod);
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
    }

    public void LowerHealth()
    {
        activeMod1 = "lh";
        if (lhNotPressed)
        {
            Color selected = buttonLowHealth.GetComponentInChildren<Text>().color;
            selected.a = 1f;
            buttonLowHealth.GetComponentInChildren<Text>().color = selected;
        }
        else
        {
            Color selected = buttonLowHealth.GetComponentInChildren<Text>().color;
            selected.a = 0.5f;
            buttonLowHealth.GetComponentInChildren<Text>().color = selected;
        }

    }

    public void NoGuns()
    {
        activeMod1 = "ng";
        if (ngNotPressed)
        {
            Color selected = buttonNoGun.GetComponentInChildren<Text>().color;
            selected.a = 1f;
            buttonNoGun.GetComponentInChildren<Text>().color = selected;
        }
        else
        {
            Color selected = buttonNoGun.GetComponentInChildren<Text>().color;
            selected.a = 0.5f;
            buttonNoGun.GetComponentInChildren<Text>().color = selected;
        }

    }

    public void EnemySpawnRate()
    {
        activeMod1 = "es";
        if (esNotPressed)
        {
            Color selected = buttonEnemySpawnRate.GetComponentInChildren<Text>().color;
            selected.a = 1f;
            buttonEnemySpawnRate.GetComponentInChildren<Text>().color = selected;
        }
        else
        {
            Color selected = buttonEnemySpawnRate.GetComponentInChildren<Text>().color;
            selected.a = 0.5f;
            buttonEnemySpawnRate.GetComponentInChildren<Text>().color = selected;
        }

    }

    public void LoseHealth()
    {
        activeMod1 = "loh";
        if (lohNotPressed)
        {
            Color selected = buttonLoseHealth.GetComponentInChildren<Text>().color;
            selected.a = 1f;
            buttonLoseHealth.GetComponentInChildren<Text>().color = selected;
        }
        else
        {
            Color selected = buttonLoseHealth.GetComponentInChildren<Text>().color;
            selected.a = 0.5f;
            buttonLoseHealth.GetComponentInChildren<Text>().color = selected;
        }

    }

    public void StartGame()
    {
        activeMod = lowHealth + noGuns + enemySpawn + loseHealth;
        PlayerPrefs.SetString("activemod", activeMod);
        SceneManager.LoadScene(goToLevelScreen);

        PlayerPrefs.SetString("CurrentLevel", "");

    }
}
