using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePiece : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI textMesh;
    protected string text;

    protected DialogueController controller;

    public virtual void SetText(DialogueController controller, string text)
    {
        this.controller = controller;
        this.text = text;
        textMesh.text = "";
        canvasGroup.alpha = 0f;
    }

    public virtual void ShowText()
    {
        textMesh.text = text;
        canvasGroup.alpha = 1f;
    }
}
