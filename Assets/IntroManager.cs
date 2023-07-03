using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public static IntroManager instance;

    public CanvasGroup introPanel;
    public CanvasGroup skipPanel;

    public IntroChatPanelController chatPanelController;
    public LoginManager loginManager;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void ShowIntroPanel()
    {
        introPanel.ShowAll();
    }
    public void HideIntroPanel()
    {
        introPanel.HideAll();
    }
    public void ShowSkipPanel()
    {
        skipPanel.ShowAll();
    }
    public void HideSkipPanel()
    {
        skipPanel.HideAll();
    }

    public void OnChatIntroConfirm()
    {
        loginManager.ToggleMain(false);
        HideIntroPanel();
        chatPanelController.canvasGroup.ShowAll();
        chatPanelController.InitIntroChat();
    }

    public void OnChatIntroCancel()
    {
        GotoMainMenu();
    }

    public void OnSkipClicked()
    {
        ShowSkipPanel();
    }

    public void OnSkipConfirm()
    {
        GotoMainMenu();
    }

    public void OnSkipCancle()
    {
        HideSkipPanel();
    }

    public void GotoMainMenu()
    {
        GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mainmenu);
    }

}
