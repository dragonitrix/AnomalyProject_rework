#if UNITY_EDITOR
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class JSONtoAchievement : MonoBehaviour
{
    public TextAsset json;

    public string path;
    public string filename;

    [ContextMenu("Convert")]
    public void Convert()
    {
        // MyClass is inheritant from ScriptableObject base class
        AchievementGroup achievementGroup = ScriptableObject.CreateInstance<AchievementGroup>();

        var tempAchievementGroup = JsonConvert.DeserializeObject<TempAchievementGroup>(json.text);

        achievementGroup.achievements = tempAchievementGroup.achievements.ToList<Achievement>();

        // path has to start at "Assets"
        string _path = $"{path}/{filename}.asset";
        AssetDatabase.CreateAsset(achievementGroup, _path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = achievementGroup;
    }
}

[Serializable]
public class TempAchievementGroup
{
    public List<Achievement> achievements;
}
#endif