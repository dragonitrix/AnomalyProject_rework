using Newtonsoft.Json;
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

    public List<AchievementProgress> achievementProgresses;

    public string currentPlayerId { get { return account.id; } }

    public void SetPlayerAccount(PlayerAccount account)
    {
        this.account.id = account.id;
        this.account.groupid = account.groupid;
        this.account.email = account.email;
    }

    public void SetPlayerInfo(PlayerInfo info)
    {
        //account.id = info.id;
        this.info.id = info.id;
        this.info.nickname = info.nickname;
        this.info.fullname = info.fullname;
        this.info.faculty = info.faculty;
        this.info.uni = info.uni;
        this.info.evalStatus = info.evalStatus;
    }

    public bool GetEvalStatus(Dimension dimension)
    {
        return info.evalStatus[(int)dimension - 1];
    }
    public void SetEvalStatus(Dimension dimension, bool val)
    {
        info.evalStatus[(int)dimension - 1] = val;
    }

    public AchievementProgress GetAchievementProgress(string id)
    {
        return achievementProgresses.Find((x) => x.achievementID == id);
    }

}

[Serializable]
public class PlayerAccount
{
    public string id;
    public string groupid;
    public string email;

    public PlayerAccount()
    {
    }
    public PlayerAccount(string id, string groupid, string email)
    {
        this.id = id;
        this.groupid = groupid;
        this.email = email;
    }

    public string JSON()
    {
        return JsonConvert.SerializeObject(this);
    }
}

[Serializable]
public class PlayerInfo
{
    public string id;
    public string nickname;
    public string fullname;
    public string faculty;
    public string uni;
    public List<bool> evalStatus;

    public string JSON()
    {
        return JsonConvert.SerializeObject(this);
    }

}


[Serializable]
public class PlayerScore
{
    public string id;
    public List<Answer> dimensionAnswers = new List<Answer>();
    public List<Answer> evalAnswers = new List<Answer>();

    public string JSON()
    {
        return JsonConvert.SerializeObject(this);
    }
}
