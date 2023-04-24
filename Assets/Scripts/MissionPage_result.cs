using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPage_result : MissionPage
{

    public Button back_button;
    public Button test_button;

    public override void InitPage()
    {
        base.InitPage();
        back_button.onClick.AddListener(ToMainMenu);
        test_button.onClick.AddListener(ToTest);
    }

    public override void StartPage()
    {
        base.StartPage();
        manager.OnMissionFinished();
    }
    public override void FinishPage()
    {
        base.FinishPage();
    }

    public void ToMainMenu()
    {
        manager.ToMainMenu();
    }

    public void ToTest()
    {
        manager.ToTest();
    }


}
