using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/HangmanPool")]
public class HangmanPool : ScriptableObject
{
    public List<HangmanText> hangmanTexts = new List<HangmanText>();
}
