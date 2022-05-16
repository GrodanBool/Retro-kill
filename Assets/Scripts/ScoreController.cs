using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{

    public static ScoreController instance;

    public double score;
    public int enemyKilledScore;

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
        
    }

    public void OnEnemyKilled()
    {
        score += enemyKilledScore;

        // UIController.instance.score.text = "SCORE: " + score;
    }
}
