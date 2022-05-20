using System;
using System.Collections.Generic;

[Serializable]
public class ScoreData
{
    public List<Score> scores { get; set; }

    public ScoreData()
    {
        scores = new List<Score>();
    }
}
