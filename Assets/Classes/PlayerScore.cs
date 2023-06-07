using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScore
{
    public string playerID;
    public List<int> missionScores;
    public List<int> testScores;
    public List<int> testAnswers;
    public List<int> preEvalScores;
    public List<int> postEvalScores;
}
