using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd = new ScoreData();
    MongoClient client = new MongoClient("mongodb+srv://Retro-kill:IAmRetroKill@retro-kill-db.23cj7.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    void Start()
    {
        database = client.GetDatabase("score");
        collection = database.GetCollection<BsonDocument>("playerscore");
    }

    private void GetList()
    {
        GetScore();
    }

    public IEnumerable<Score> GetHighScores()
    {
        GetList();
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

    public void GetScore()
    {
        var allScoresTask = collection.Find(new BsonDocument());
        var scoresAwaited = allScoresTask; 
        
        List<Score> scoresList = new List<Score>();
        foreach (var score in scoresAwaited.ToList())
        {
            AddScore(Deserialize(score.ToString()));
        }
    }

    private Score Deserialize(string rawJson)
    {
        var stringNoObjectId = rawJson.Substring(rawJson.IndexOf("),") + 2);
        string deserializableString = "{ " + stringNoObjectId;

        var scoreString = JsonConvert.DeserializeObject(deserializableString).ToString();
        var score = JsonUtility.FromJson<Score>(scoreString);
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
}























