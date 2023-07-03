using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string id;
    public DialogueType type = DialogueType._0_bot;
    [TextArea(2, 10)]
    public string text;

}

[Serializable]
public class Dialogue_chat : Dialogue
{
    public string command_id;
    public List<Decision> choices = new List<Decision>();
}

[Serializable]
public class Decision
{
    public string id;
    public string text;
    public string command_id;
}

public enum ChatSide
{
    _PLAYER,
    _BOT
}

public enum DialogueType
{
    _0_bot,
    _1_playerchoice,
    _2_playerinput
}