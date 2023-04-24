using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPage_hangman : MissionPage
{
    public HangmanController controller;

    public CanvasGroup askPanel;
    public Button accept_button;
    public Button declined_button;

    public override void InitPage()
    {
        base.InitPage();
        controller.InitHangmanGame();

        controller.onHangmanGameFinished += (result) =>
        {
            manager.AddHealth(1);
            FinishPage();
        };

        accept_button.onClick.AddListener(OnAccept);
        declined_button.onClick.AddListener(OnDeclined);

    }

    public override void StartPage()
    {
        base.StartPage();
        askPanel.ShowAll();
    }

    public void OnAccept()
    {
        askPanel.HideAll();
        controller.StartHangmanGame();
    }

    public void OnDeclined()
    {
        askPanel.interactable=false;
        FinishPage();
    }

}
