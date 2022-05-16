using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerkScreen : MonoBehaviour
{
    public static PerkScreen instance;

    public string goToLevelScreen;
    public string returnTo;

    GameManager manager;

    private string activeMod;

    public string activeMod1;

    public bool lhNotPressed = true;
    public bool esNotPressed = true;
    public bool lohNotPressed = true;
    public bool ngNotPressed = true;

    public string lowHealth;
    public string noGuns;
    public string enemySpawn;
    public string loseHealth;

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
    }

    public void NoGuns()
    {
        activeMod1 = "ng";

    }

    public void EnemySpawnRate()
    {
        activeMod1 = "es";

    }

    public void LoseHealth()
    {
        activeMod1 = "loh";

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(returnTo);

        PlayerPrefs.SetString("CurrentLevel", "");
    }

    public void StartGame()
    {
        activeMod = lowHealth + noGuns + enemySpawn + loseHealth;
        PlayerPrefs.SetString("activemod", activeMod);
        SceneManager.LoadScene(goToLevelScreen);

        PlayerPrefs.SetString("CurrentLevel", "");

    }
}
