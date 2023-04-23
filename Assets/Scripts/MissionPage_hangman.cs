using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionPage_hangman : MissionPage
{
    public HangmanController controller;

    public override void InitPage()
    {
        base.InitPage();
        controller.InitHangmanGame();

        controller.onHangmanGameFinished += () =>
        {
            FinishPage();
        };

    }


}
