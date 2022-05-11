using System;

[Serializable]
public class Score
{
    public int id;
    public string name;
    public float score;

    public Score(int id, string name, float score)
    {
        this.id = id;
        this.name = name;
        this.score = score;
    }
}
