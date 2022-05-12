using System.Linq;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    public RowUi rowUi;
    public ScoreManager scoreManager;

    void Start()
    {
        //add fake data
        scoreManager.AddScore(new Score(id: 1, name: "Viktor", score: 69420));
        scoreManager.AddScore(new Score(id: 2, name: "Erik", score: 420));
        scoreManager.AddScore(new Score(id: 3, name: "Nils", score: 69));
        scoreManager.AddScore(new Score(id: 4, name: "Vem?", score: 12));
        scoreManager.AddScore(new Score(id: 5, name: "Vad?", score: 54));
        scoreManager.AddScore(new Score(id: 6, name: "Hur?", score: 190000));
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
}

