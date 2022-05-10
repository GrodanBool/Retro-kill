using System.Linq;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    public RowUi rowUi;
    public ScoreManager scoreManager;
    
    void Start()
    {

        scoreManager.AddScore(new Score(name: "Viktor", score: 69420));
        scoreManager.AddScore(new Score(name: "Erik", score: 420));
        scoreManager.AddScore(new Score(name: "Nils", score: 69));

        var scores = scoreManager.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
            row.rank.text = (i + 1).ToString();
            row.name.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }
}

