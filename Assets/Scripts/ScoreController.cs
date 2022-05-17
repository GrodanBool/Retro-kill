using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreController : MonoBehaviour
{

    public static ScoreController instance;

    public double score;
    public int enemyKilledScore;
    public int totalMultiplier = 1;

    [HideInInspector] public bool loseHealthMultiplier = false;
    [HideInInspector] public bool noGunMultiplier = false;
    [HideInInspector] public bool enemyRespawnRateMultiplier = false;
    [HideInInspector] public bool lowerStartingHealthMultiplier = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ScoreMultiplier();
        StartCoroutine("AddPoints");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnemyKilled()
    {
        score += enemyKilledScore * totalMultiplier;
    }

    public void ScoreMultiplier()
    {
        string multiplierString = PlayerPrefs.GetString("activemod");
        string[] multiplierArray = multiplierString.Split("\n");

        if (multiplierArray.Contains("Lower Health"))
        {
            totalMultiplier += 1;
        }

        if (multiplierArray.Contains("No Guns"))
        {
            totalMultiplier += 2;
        }

        if (multiplierArray.Contains("Enemy Spawn Rate"))
        {
            totalMultiplier += 3;
        }

        if (multiplierArray.Contains("Lose Health"))
        {
            totalMultiplier += 4;
        }
    }

    IEnumerator AddPoints()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            totalMultiplier += 2;
        }
    }
}
