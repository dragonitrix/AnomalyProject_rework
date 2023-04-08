using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControllerAdditive : DialogueController
{
    public GameObject dialoguePiece_prefab;

    public RectTransform dialogue_panel_main;
    public RectTransform dialogue_panel_content;
    public HorizontalOrVerticalLayoutGroup layoutGroup;

    private void Start()
    {
        InitDialogue();
        Resize();
    }

    public override void NextDialogue()
    {
        base.NextDialogue();
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

}
