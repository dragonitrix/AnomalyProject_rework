using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public CanvasGroup small_loadOverlay;
    public CanvasGroup big_loadOverlay;
    // Start is called before the first frame update
    void Start()
    {
        HideLoadOverlay(LoadOverlayType._ALL);
    }
    public void ShowLoadOverlay(LoadOverlayType type)
    {
        switch (type)
        {
            case LoadOverlayType._ALL:
                small_loadOverlay.ShowAll();
                big_loadOverlay.ShowAll();
                break;
            case LoadOverlayType._BIG:
                big_loadOverlay.ShowAll();
                break;
            case LoadOverlayType._SMALL:
                small_loadOverlay.ShowAll();
                break;
        }
    }
    public void HideLoadOverlay(LoadOverlayType type)
    {
        switch (type)
        {
            case LoadOverlayType._ALL:
                small_loadOverlay.HideAll();
                big_loadOverlay.HideAll();
                break;
            case LoadOverlayType._BIG:
                big_loadOverlay.HideAll();
                break;
            case LoadOverlayType._SMALL:
                small_loadOverlay.HideAll();
                break;
        }
    }

    public void PrepareAndGoToQuestionScene(Dimension dimension)
    {
        //DatabaseManagerMongo.instance.FetchAllQuestion(1, (data) => { Debug.Log(data.Count); });
        //DatabaseManagerMongo.instance.FetchUnansweredQuestion(1);
        Debug.Log("PrepareAndGoToQuestionScene: " + dimension);
        ShowLoadOverlay(LoadOverlayType._BIG);
        DatabaseManagerMongo.instance.FetchUnansweredQuestion((int)dimension, (all, unanswered) =>
        {
            QuestionPool.instance.questions_all = all;
            QuestionPool.instance.questions_unanswered = unanswered;
            QuestionPool.instance.questions_unanswered.Shuffle();
            HideLoadOverlay(LoadOverlayType._ALL);
            GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_question);
        });
    }

    public void GoToMission(Dimension dimension)
    {
        switch (dimension)
        {
            case Dimension._D1:
                GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mission_1);
                break;
            case Dimension._D2:
                GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mission_2);
                break;
            case Dimension._D3:
                GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mission_3);
                break;
            case Dimension._D4:
                GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mission_4);
                break;
            case Dimension._D5:
                GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mission_5);
                break;
            case Dimension._D6:
                GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mission_6);
                break;
            default:
                break;
        }

    }

    public void UpdatePlayerAnswers(List<Answer> answers, System.Action<string> callback)
    {
        ShowLoadOverlay(LoadOverlayType._SMALL);
        DatabaseManagerMongo.instance.UpdatePlayerAnswers(answers, (data) =>
        {
            callback(data);
            HideLoadOverlay(LoadOverlayType._SMALL);
        });
    }

    public enum LoadOverlayType
    {
        _ALL,
        _BIG,
        _SMALL,
    }



}
