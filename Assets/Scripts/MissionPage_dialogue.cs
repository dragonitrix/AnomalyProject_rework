using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionPage_dialogue : MissionPage
{

    public DialogueController controller;

    public override void InitPage()
    {
        base.InitPage();
        controller.InitDialogue();
        controller.onDialogueFinished += OnDialogueFinished;
    }

    public override void StartPage()
    {
        base.StartPage();
        controller.StartDialogue();

    }
    public override void FinishPage()
    {
        base.FinishPage();

    }

    public void OnDialogueFinished()
    {
        Debug.Log("OnDialogueFinished");
        FinishPage();
    }

}
