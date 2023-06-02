using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GroupScores
{
    public int count;
    public List<List<float>> testDatas;
    public List<List<float>> missionDatas;
    public List<List<float>> evalCounts;
    public List<List<float>> evalTotals;
    public List<List<float>> evalAvgs;

    public void Log()
    {
        Debug.Log(count);
        LogList(testDatas);
        LogList(missionDatas);
        LogList(evalCounts);
        LogList(evalTotals);
        LogList(evalAvgs);
    }

    public void LogList(List<List<float>> l)
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
                text += l[i][j];
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
