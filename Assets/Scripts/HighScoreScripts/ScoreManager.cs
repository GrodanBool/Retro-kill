using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [HideInInspector] public ScoreData sd = new ScoreData();
    [HideInInspector] public bool hasChanges = false;
    [HideInInspector] public bool uploadSuccess = false;
    MongoClient client = new MongoClient("mongodb+srv://Retro-kill:" + Pass.mongoPass + "@retro-kill-db.23cj7.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    void Awake()
    {
        if (instance == null) { instance = this; }
    }

    void Start()
    {
        database = client.GetDatabase("score");
        collection = database.GetCollection<BsonDocument>("playerscore");
    }

    public async Task UploadNewHighScore(string name, double score)
    {
        try
        {
            Score uploadScore = new Score(Convert.ToInt32(collection.CountDocuments(new BsonDocument())) + 1, name, score);
            var scoreString = JsonConvert.SerializeObject(uploadScore).ToString();
            var document = BsonSerializer.Deserialize<BsonDocument>(scoreString);
            await collection.InsertOneAsync(document);
            uploadSuccess = true;
        }
        catch
        {
            uploadSuccess = false;
        }
    }

    public async void GetHighScores()
    {
        var allScoresTask = collection.FindAsync(new BsonDocument());
        var scoresAwaited = await allScoresTask;

        foreach (var score in scoresAwaited.ToList())
        {
            AddScoreToLocalList(DeserializeJsonToScore(score.ToString()));
        }
    }

    private Score DeserializeJsonToScore(string rawJson)
    {
        var stringNoObjectId = rawJson.Substring(rawJson.IndexOf("),") + 2);
        string deserializableString = "{ " + stringNoObjectId;

        var scoreString = JsonConvert.DeserializeObject(deserializableString).ToString();
        var score = JsonUtility.FromJson<Score>(scoreString);
        return score;
    }

    public void AddScoreToLocalList(Score score)
    {
        if (!sd.scores.Any(s => s.id == score.id))
        {
            sd.scores.Add(score);
            hasChanges = true;
        }
        else if (sd.scores.Where(s => s.id == score.id).Any(s => s.score < score.score))
        {
            sd.scores.Where(s => s.id == score.id).Select(s => { s.score = score.score; return score; }).ToList();
            hasChanges = true;
        }
    }
}