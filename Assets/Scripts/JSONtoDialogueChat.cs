#if UNITY_EDITOR
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class JSONtoDialogueChat : MonoBehaviour
{
    public TextAsset json;

    public string path;
    public string filename;

    [ContextMenu("Convert")]
    public void Convert()
    {
        // MyClass is inheritant from ScriptableObject base class
        DialogueChat dialogueChat = ScriptableObject.CreateInstance<DialogueChat>();

        var tempDialogueChat = JsonConvert.DeserializeObject<TempDialogueChat>(json.text);

        dialogueChat.dialogue_Chats = tempDialogueChat.dialogue_Chats.ToList<Dialogue_chat>();

        // path has to start at "Assets"
        string _path = $"{path}/{filename}.asset";
        AssetDatabase.CreateAsset(dialogueChat, _path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = dialogueChat;
    }
}

[Serializable]
public class TempDialogueChat
{
    public List<Dialogue_chat> dialogue_Chats;
}
#endif