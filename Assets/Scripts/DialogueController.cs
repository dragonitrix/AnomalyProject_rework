using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public DialogueSet dialogueSet;
    protected int dialogue_index;

    public delegate void OnDialogueFinishedDelegate();
    public OnDialogueFinishedDelegate onDialogueFinished = () => { };

    public CanvasGroup canvasGroup;

    public bool isClickable = false;

    public string chatSFXOverride = "";

    public virtual void InitDialogue()
    {
        //Debug.Log("InitDialogue");
        SetInteractable(false);
    }

    public virtual void InitDialogue(DialogueSet dialogueSet)
    {
        this.dialogueSet = dialogueSet;
        InitDialogue();
    }

    public virtual void StartDialogue()
    {
        Debug.Log("StartDialogue");
        dialogue_index = 0;
        SetCurrentDialogue();
        SetInteractable(true);
    }

    public virtual void NextDialogue()
    {
        if (dialogue_index >= 0 && dialogue_index < dialogueSet.dialogues.Count - 1)
        {
            dialogue_index++;
            SetCurrentDialogue();
        }
        else
        {
            OnFinished();
        }
    }

    public virtual void SetCurrentDialogue()
    {
        AudioManager.instance.PlaySound(chatSFXOverride == "" ? "chat" : chatSFXOverride);
    }

    public virtual void OnFinished()
    {
        Debug.Log("Dialogue END");
        onDialogueFinished();
        //SetInteractable(false);
        isClickable = false; // only allow scroll
    }

    public virtual void OnClickAreaClicked()
    {
        if (!isClickable) return;
        NextDialogue();
    }

    protected Dialogue GetCurrentDialogue()
    {
        if (dialogue_index >= 0 && dialogue_index < dialogueSet.dialogues.Count)
        {
            return dialogueSet.dialogues[dialogue_index];
        }
        else
        {
            return null;
        }
    }

    public void SetInteractable(bool val)
    {
        canvasGroup.interactable = val;
        canvasGroup.blocksRaycasts = val;
        isClickable = val;
    }

}
