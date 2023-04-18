using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class QuestionData
{
    public string id;
    public Type type;
    public Dimension dimension;
    [TextArea(15, 20)]
    public string question;
    [TextArea(5, 15)]
    public List<string> choices = new List<string>();
    public int answer;
    public int weight;

    public void RefineChoiceText()
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

    public void RefineQuestion()
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