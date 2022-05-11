using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;
    void Awake()
    {
        var json = PlayerPrefs.GetString("scores", "{}");
        sd = JsonUtility.FromJson<ScoreData>(json);
    }

    public IEnumerable<Score> GetHighScores()
    {
        IEnumerable<Score> scores = new List<Score>();
        IEnumerable<Score> scoresTop5 = new List<Score>();

        scores = sd.scores.OrderByDescending(x => x.score);
        scoresTop5 = scores.Take(5).ToList();

        return scoresTop5;
    }

    public void AddScore(Score score)
    {
        if (!sd.scores.Any(s => s.id == score.id))
        {
            sd.scores.Add(score);
        }
        else if (sd.scores.Where(s => s.id == score.id).Any(s => s.score < score.score))
        {
            sd.scores.Where(s => s.id == score.id).Select(s => { s.score = score.score; return score; }).ToList();
        }
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public void SaveScore()
    {
        var json = JsonUtility.ToJson(sd);
        Debug.Log(json);
        PlayerPrefs.SetString("scores", json);
    }
}























