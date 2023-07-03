using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GroupScores
{
    public int count;
    public List<List<GroupScoreDetail>> testDatas;
    public List<List<GroupScoreDetail>> missionDatas;
    public List<List<GroupScoreDetail>> evalDatas;

    public void Log()
    {
        Debug.Log(count);
        LogList(testDatas);
        LogList(missionDatas);
        LogList(evalDatas);
    }

    public void LogList(List<List<GroupScoreDetail>> l)
    {
        if (l == null)
        {
            Debug.Log("null");
            return;
        }
        var text = "";
        text += "[\n";
        for (int i = 0; i < l.Count; i++)
        {
            text += "\t[ ";
            for (int j = 0; j < l[i].Count; j++)
            {
                text += l[i][j].GetLog();
                if (j < l[i].Count - 1)
                {
                    text += ", ";
                }
                else
                {
                    text += "], \n";
                }
            }
        }
        text += "]";
        Debug.Log(text);
    }


}

[Serializable]
public class GroupScoreDetail
{
    public float count;
    public float avg;
    public float sd;

    public string GetLog()
    {
        return $"count: {count} avg: {avg} sd: {sd}";
    }

}