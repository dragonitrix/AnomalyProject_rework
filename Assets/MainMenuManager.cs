using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public MainMenuButton questionButton;
    public MainMenuButton missionButton;
    public MainMenuButton evalButton;

    public MainMenuBlankArea blankArea;

    // Start is called before the first frame update
    void Start()
    {
        questionButton.onPanelClick = OnQuestionClick;
        missionButton.onPanelClick = OnMissionClick;
        evalButton.onPanelClick = OnEvalClick;

        questionButton.onContentClick = OnQuestionContentClick;
        missionButton.onContentClick = OnMissionContentClick;
        evalButton.onContentClick = OnEvalContentClick;

        blankArea.onPanelClick = SetAllIdle;
    }

    public void OnQuestionClick()
    {
        if (questionButton.state == MainMenuButton.State._IDLE)
        {
            questionButton.SetState(MainMenuButton.State._EXPAND);
            missionButton.SetState(MainMenuButton.State._SHRINK);
            evalButton.SetState(MainMenuButton.State._SHRINK);
        }
        else
        {
            SetAllIdle();
        }
    }
    public void OnMissionClick()
    {
        if (missionButton.state == MainMenuButton.State._IDLE)
        {
            questionButton.SetState(MainMenuButton.State._SHRINK);
            missionButton.SetState(MainMenuButton.State._EXPAND);
            evalButton.SetState(MainMenuButton.State._SHRINK);
        }
        else
        {
            SetAllIdle();
        }

    }
    public void OnEvalClick()
    {

        if (evalButton.state == MainMenuButton.State._IDLE)
        {
            questionButton.SetState(MainMenuButton.State._SHRINK);
            missionButton.SetState(MainMenuButton.State._SHRINK);
            evalButton.SetState(MainMenuButton.State._EXPAND);
        }
        else
        {
            SetAllIdle();
        }
    }
    public void OnQuestionContentClick(int index)
    {
        GameManager.instance.PrepareAndGoToQuestionScene((Dimension)index + 1);
    }
    public void OnMissionContentClick(int index)
    {
        GameManager.instance.GoToMission((Dimension)index + 1);
    }
    public void OnEvalContentClick(int index)
    {

    }

    public void SetAllIdle()
    {
        questionButton.SetState(MainMenuButton.State._IDLE);
        missionButton.SetState(MainMenuButton.State._IDLE);
        evalButton.SetState(MainMenuButton.State._IDLE);
    }

}
