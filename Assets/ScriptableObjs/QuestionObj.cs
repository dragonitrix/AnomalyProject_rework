using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/QuestionObj")]
public class QuestionObj : ScriptableObject
{
    public QuestionData question;
}
