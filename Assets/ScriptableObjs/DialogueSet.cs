using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/DialogueSet")]
public class DialogueSet : ScriptableObject
{
    public string id;
    public List<Dialogue> dialogues = new List<Dialogue>();
}
