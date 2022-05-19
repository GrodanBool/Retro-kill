using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    public event EventHandler CurrentChanged;
    public RowUi rowUi;
    void Start()
    {
        ScoreManager.instance.GetHighScores().ToArray();
    }

    void Update()
    {
        if (ScoreManager.instance.hasChanges)
        {
            List<Score> scoresTop5 = new List<Score>();
            scoresTop5 = ScoreManager.instance.sd.scores.OrderByDescending(x => x.score).Take(5).ToList();

            foreach (Score score in scoresTop5)
            {
                var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
                row.rank.text = (scoresTop5.IndexOf(score) + 1).ToString();
                row.playerName.text = score.name;
                row.score.text = score.score.ToString();
            }
            ScoreManager.instance.hasChanges = false;
        }
    }
}

