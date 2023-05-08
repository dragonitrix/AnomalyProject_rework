using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HangmanChar : MonoBehaviour
{
    public string character;
    public bool corrected = false;
    public TextMeshProUGUI textMeshPro;
    public CanvasGroup _;

    public void InitChar(string character)
    {
        this.character = character;
        textMeshPro.text = "";

        if (character == " ")
        {
            _.alpha = 0;
        }
    }

    public void ShowText()
    {
        textMeshPro.text = character;
        corrected = true;
    }

}
