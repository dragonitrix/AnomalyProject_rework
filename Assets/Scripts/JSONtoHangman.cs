#if UNITY_EDITOR
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class JSONtoHangman : MonoBehaviour
{
    public TextAsset json;

    public string path;
    public string filename;

    [ContextMenu("Convert")]
    public void Convert()
    {
        // MyClass is inheritant from ScriptableObject base class
        HangmanPool hangmanPool = ScriptableObject.CreateInstance<HangmanPool>();

        var tempHangmanPool = JsonConvert.DeserializeObject<TempHangmanPool>(json.text);

        hangmanPool.hangmanTexts = tempHangmanPool.hangmanTexts.ToList<HangmanText>();

        // path has to start at "Assets"
        string _path = $"{path}/{filename}.asset";
        AssetDatabase.CreateAsset(hangmanPool, _path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = hangmanPool;
    }
}

[Serializable]
public class TempHangmanPool
{
    public List<HangmanText> hangmanTexts;
}
#endif