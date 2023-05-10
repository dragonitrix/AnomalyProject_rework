using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AchievementGroup")]
public class AchievementGroup : ScriptableObject
{
    public List<Achievement> achievements = new List<Achievement>();
}
