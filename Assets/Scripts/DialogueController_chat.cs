using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController_chat : DialogueController_additive
{
    public override void StartDialogue()
    {
        //base.StartDialogue();

        //show all
        for (int i = 0; i < dialogueSet.dialogues.Count; i++)
        {
            var dialogue = dialogueSet.dialogues[i];
            var clone = Instantiate(dialoguePiece_prefab, dialogue_panel_content);
            var piece = clone.GetComponent<DialoguePiece>();
            piece.SetText(this, dialogue.text);
            piece.ShowText();
        }

        dialogue_index = dialogueSet.dialogues.Count - 1;
        SetInteractable(true);

        Resize();
    }


    public override void SetCurrentDialogue()
    {
        base.SetCurrentDialogue();

        var clone = Instantiate(dialoguePiece_prefab, dialogue_panel_content);
        var piece = clone.GetComponent<DialoguePiece>();
        var currentDialogue = GetCurrentDialogue();
        piece.SetText(this, currentDialogue.text);
        piece.ShowText();
        Resize(true);
    }

    void Resize(bool jumptolast = false)
    {
        if (!resize) return;
        StartCoroutine(_resize(jumptolast));
    }
    IEnumerator _resize(bool jumptolast)
    {
        yield return new WaitForEndOfFrame();
        var totalY = 0f;
        for (int i = 0; i < dialogue_panel_content.childCount; i++)
        {
            totalY += dialogue_panel_content.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }

        totalY += layoutGroup.spacing * (dialogue_panel_content.childCount - 1);

        dialogue_panel_content.sizeDelta = new Vector2(
            dialogue_panel_content.sizeDelta.x,
            totalY
        );
        if (jumptolast)
        {
            dialogue_panel_content.anchoredPosition = new Vector2(
                dialogue_panel_content.anchoredPosition.x,
                0
                );
        }
    }

    public override void OnClickAreaClicked()
    {
        //base.OnClickAreaClicked();
    }

}
