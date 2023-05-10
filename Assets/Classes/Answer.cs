using System;
using UnityEngine;

[Serializable]
public class Answer
{
    public string playerID;
    public AnswerType answerType;
    public string questionID;
    public Dimension dimension;
    public int answer;
    public bool isCorrected;

    public void Log()
    {
        //Debug.Log("playerID: " + playerID);
        //Debug.Log("answerType: " + answerType);
        Debug.Log("questionID: " + questionID);
        Debug.Log("dimension: " + dimension);
        Debug.Log("answer: " + answer);
        Debug.Log("isCorrected: " + isCorrected);
    }

    public Answer(string playerID, AnswerType answerType, string questionID, Dimension dimension, int answer, bool isCorrected)
    {
        this.playerID = playerID;
        this.answerType = answerType;
        this.questionID = questionID;
        this.dimension = dimension;
        this.answer = answer;
        this.isCorrected = isCorrected;
    }
}

public enum AnswerType
{
    _MISSION = 0,
    _TEST,
    _EVAL_PRE,
    _EVAL_POST,
}