using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    public RowUi rowUi;
    public ScoreManager scoreManager;
    void Start()
    {
        GetList();

        //gets highscores
        var scores = scoreManager.GetHighScores().ToArray();

        //shows 5 highest scores
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
            row.rank.text = (i + 1).ToString();
            row.playerName.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }

    private async void GetList()
    {
        List<Score> scoreList = await scoreManager.GetScore();
        foreach (Score score in scoreList)
        {
            scoreManager.AddScore(score);
        }
    }
}

