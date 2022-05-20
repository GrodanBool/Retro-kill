using System;
using UnityEngine;

[Serializable]
public class Score
{
    public int id;
    public string name;
    public double score;

    public Score(int id, string name, double score)
    {
        this.id = id;
        this.name = name;
        this.score = score;
    }
}
