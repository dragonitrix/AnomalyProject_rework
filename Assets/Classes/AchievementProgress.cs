using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementProgress
{
    public string playerID;
    public string achievementID;
    public int currentProgress;

    public AchievementProgress(string playerID, string achievementID, int currentProgress)
    {
        this.playerID = playerID;
        this.achievementID = achievementID;
        this.currentProgress = currentProgress;
    }
}
