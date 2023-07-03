using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePiece_chat2 : DialoguePiece
{

    public ChatSide chatSide;

    public CanvasGroup profileCanvasGroup;
    public RectTransform mainRect;
    public RectTransform chatRect;

    public IntroChatPanelController introManager;

    public virtual void SetDialogue(IntroChatPanelController introManager, string text, ChatSide side)
    {
        this.introManager = introManager;
        this.text = text;
        this.chatSide = side;
        textMesh.text = text;
        StartCoroutine(_UpdateSize());
    }

    IEnumerator _UpdateSize()
    {
        LayoutRebuilder.MarkLayoutForRebuild(mainRect);

        yield return new WaitForEndOfFrame();

        Debug.Log("child size:");
        Debug.Log(chatRect.sizeDelta);

        mainRect.sizeDelta = new Vector2(introManager.dialogueRect.sizeDelta.x, chatRect.sizeDelta.y);

    }

    public void SetProfile(bool val)
    {
        if (val)
        {
            profileCanvasGroup.ShowAll();
        }
        else
        {
            profileCanvasGroup.HideAll();
        }
    }

}
