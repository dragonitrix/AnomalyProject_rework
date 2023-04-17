using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    public static PlayerInfoManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        account = new PlayerAccount();
        info = new PlayerInfo();
        score = new PlayerScore();


    }

    public PlayerAccount account;
    public PlayerInfo info;
    public PlayerScore score;

    public string CurrentPlayerId { get { return account.id; } }


}

[Serializable]
public class PlayerAccount
{
    public string id;
    public string email;
}

[Serializable]
public class PlayerInfo
{
    public string id;
    public string nickname;
    public string fullname;
    public string faculty;
    public string uni;
}

[Serializable]
public class Answer
{
    public string id;
    public int answer;
    public Dimension dimension;
    public bool isCorrected;
}

[Serializable]
public class PlayerScore
{
    public string id;
    public List<Answer> dimensionAnswers = new List<Answer>();
    public List<Answer> evalAnswers = new List<Answer>();
}

[Serializable]
public class QuestionData
{
    public string id;
    public Type type;
    public Dimension dimension;
    [TextArea]
    public string question;
    public List<string> choices = new List<string>();
    public int answer;
    public int weight;

    public void refineChoiceText()
    {
        for (int i = 0; i < choices.Count; i++)
        {
            Regex regex = new Regex("(\\d\\s)");
            choices[i] = regex.Replace(choices[i], "");

            if (choices[i].Length > 0 && choices[i][0] == ' ')
            {
                choices[i].Remove(0);
            }
        }
    }

    public void refineQuestion()
    {
        Regex regex = new Regex("(\\$fill)");
        question = regex.Replace(question, ".......");

    }

    public enum Type
    {
        _0_truefalse,
        _1_multichoice,
        _2_filling
    }

}

[Serializable]
public class DimensionTest
{
    public int dimensionIndex = 0;
    public List<QuestionData> questionDatas = new List<QuestionData>();
}

public enum Dimension
{
    _D1 = 1,
    _D2,
    _D3,
    _D4,
    _D5,
    _D6,
}