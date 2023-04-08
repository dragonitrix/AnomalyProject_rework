using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string id;
    [TextArea]
    public string text;

}

[Serializable]
public class Dialogue_dicision : Dialogue
{
    public List<string> choices = new List<string>();
}