using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class EvalData
{
    public string id;
    public int type;
    public Dimension dimension;
    [TextArea(15, 20)]
    public string question;
    [TextArea(5, 15)]
    public List<string> choices = new List<string>();
    public int answer;


    public void Log()
    {
        Debug.Log("id: " + id);
        Debug.Log("type: " + type);
        Debug.Log("dimension: " + dimension);
        Debug.Log("question: " + question);
        for (int i = 0; i < choices.Count; i++)
        {
            Debug.Log("choice " + i + " : " + choices[i]);
        }
        Debug.Log("answer: " + answer);
    }

}