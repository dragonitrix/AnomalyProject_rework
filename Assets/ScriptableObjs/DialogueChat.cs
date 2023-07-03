using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/DialogueChats")]
public class DialogueChat : ScriptableObject
{
    public string id;
    public List<Dialogue_chat> dialogue_Chats;
}
