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
public class PlayerScore
{
    public string id;
    public List<Answer> dimensionAnswers = new List<Answer>();
    public List<Answer> evalAnswers = new List<Answer>();
}
