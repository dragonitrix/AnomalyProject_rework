using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public CanvasGroup session_overlay;
    public TextMeshProUGUI session_text;

    public string currentSession = "";

    public delegate void EnableAdminFeatureDelegate();
    public EnableAdminFeatureDelegate enableAdminFeature = () => { };

    // Start is called before the first frame update
    void Start()
    {
        HideLoadOverlay(LoadOverlayType._ALL);

        var groupid = DatabaseManagerMongo.instance.TryGetGroupID();

        if (groupid != null)
        {
            currentSession = groupid;
            session_text.text = groupid;
            session_overlay.alpha = 1;
        }
        else
        {
            session_overlay.alpha = 0;
        }

        AudioManager.instance.PlayBGM("bgm_1");

        //
        //var path = URLParameters.location_pathname();
        //
        //Debug.Log("test URLParameters");
        //
        //Debug.Log("path: " + path);
        //Debug.Log("groupid: " + groupid);
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
            QuestionPool.instance.currentDimension = dimension;
            QuestionPool.instance.questions_all = all;
            QuestionPool.instance.questions_unanswered = unanswered;
            QuestionPool.instance.questions_unanswered.Shuffle();
            HideLoadOverlay(LoadOverlayType._ALL);
            GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_question);
        });
    }

    public void PrepareAndGoToEvalScene(Dimension dimension, bool redirectToMission = false)
    {
        Debug.Log("PrepareAndGoToEvalScene: " + dimension);
        ShowLoadOverlay(LoadOverlayType._BIG);
        DatabaseManagerMongo.instance.FetchEval((int)dimension, (all) =>
        {
            EvalPool.instance.currentDimension = dimension;
            EvalPool.instance.redirectToMission = redirectToMission;
            EvalPool.instance.evals = all;
            EvalPool.instance.evals.Shuffle();
            HideLoadOverlay(LoadOverlayType._ALL);
            GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_eval);
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

    public void UpdatePlayerInfo(System.Action<string> callback)
    {
        ShowLoadOverlay(LoadOverlayType._SMALL);
        DatabaseManagerMongo.instance.UpdatePlayerInfo((data) =>
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
