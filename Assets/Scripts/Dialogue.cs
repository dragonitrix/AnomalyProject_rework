using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string id;
    [TextArea(10,20)]
    public string text;

}

[Serializable]
public class Dialogue_dicision : Dialogue
{
    public List<string> choices = new List<string>();
}