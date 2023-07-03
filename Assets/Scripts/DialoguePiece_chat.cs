using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePiece_chat : DialoguePiece
{

    public CanvasGroup profile;

    public virtual void SetText(string text)
    {
        this.text = text;
        textMesh.text = "";
        canvasGroup.alpha = 0f;
    }

    public void SetProfile(bool val)
    {
        if (val)
        {
            profile.ShowAll();
        }
        else
        {
            profile.HideAll();
        }
    }

}
