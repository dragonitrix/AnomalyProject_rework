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
    public OnDialogueFinishedDelegate onDialogueFinished;

    public virtual void InitDialogue()
    {
        dialogue_index = 0;
        SetCurrentDialogue();
    }

    public virtual void NextDialogue()
    {
        if (dialogue_index >= 0 && dialogue_index < dialogueSet.dialogues.Count-1)
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

    }

    public virtual void OnFinished()
    {
        Debug.Log("Dialogue END");
        onDialogueFinished();
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

}
