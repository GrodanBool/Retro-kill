using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;
    MongoClient client = new MongoClient("mongodb+srv://Retro-kill:IAmRetroKill@retro-kill-db.23cj7.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    void Awake()
    {
        var json = PlayerPrefs.GetString("scores", "{}");
        sd = JsonUtility.FromJson<ScoreData>(json);
    }

    void Start()
    {
        database = client.GetDatabase("score");
        collection = database.GetCollection<BsonDocument>("playerscore");
    }

    public IEnumerable<Score> GetHighScores()
    {
        IEnumerable<Score> scores = new List<Score>();
        IEnumerable<Score> scoresTop5 = new List<Score>();
        scores = sd.scores.OrderByDescending(x => x.score);
        scoresTop5 = scores.Take(5).ToList();
        return scoresTop5;
    }

    public async void SaveScore(string name, float score)
    {
        BsonDocument document = new BsonDocument { { name, score} };
        await collection.InsertOneAsync(document);
    }

    public async Task<List<Score>> GetScore()
    {
        var allScoresTask = collection.FindAsync(new BsonDocument());
        var scoresAwaited = await allScoresTask; 
        
        List<Score> scoresList = new List<Score>();
        foreach (var score in scoresAwaited.ToList())
        {
            scoresList.Add(Deserialize(score.ToString()));
        }

        return scoresList;
    }

    private Score Deserialize(string rawJson)
    {
        var stringNoObjectId = rawJson.Substring(rawJson.IndexOf("),") + 2);
        string testString = "{ " + stringNoObjectId;

        var test = JsonConvert.DeserializeObject(testString).ToString();
        var score = JsonUtility.FromJson<Score>(test);
        return score;
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
        PlayerPrefs.SetString("scores", json);
    }
}























