using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvalTestManager : MonoBehaviour
{

    public Dimension dimension;
    public bool redirectToMission;

    public GameObject eval_prefab;

    public RectTransform evalPanel;

    public List<EvalData> currentQuestions = new List<EvalData>();
    public List<Answer> currentAnswer = new List<Answer>();

    public List<EvalQuestionObj_radio> currentQuestionsObj = new List<EvalQuestionObj_radio>();

    public TextMeshProUGUI dimensionText;

    public void InitTest()
    {
        var pool = EvalPool.instance.evals;

        currentQuestions.AddRange(pool);

        for (int i = 0; i < pool.Count; i++)
        {
            var q = pool[i];
            var clone = Instantiate(eval_prefab, evalPanel);
            var script = clone.GetComponent<EvalQuestionObj_radio>();

            script.SetQuestion(q.question);

            currentQuestionsObj.Add(script);
        }

        dimension = EvalPool.instance.currentDimension;
        dimensionText.text = ((int)dimension).ToString();

        redirectToMission = EvalPool.instance.redirectToMission;

        Resize();
    }

    public void OnTestFinished()
    {
        if (!IsAllAnswered()) return;

        FetchAllResult();

        var preTestFlag = false;

        if (!PlayerInfoManager.instance.GetEvalStatus(EvalPool.instance.currentDimension)) //pre test detected
        {
            preTestFlag = true;
            PlayerInfoManager.instance.SetEvalStatus(EvalPool.instance.currentDimension, true);
        }

        //update player's answer to database
        GameManager.instance.UpdatePlayerAnswers(currentAnswer, (data) =>
        {
            currentAnswer.Clear();
            GameManager.instance.UpdatePlayerInfo((data) =>
            {
                AchievementManager.instance.UpdateAchievement_Eval(dimension, preTestFlag, () =>
                {
                    //do something
                    if (redirectToMission)
                    {
                        GameManager.instance.GoToMission(dimension);
                    }
                    else
                    {
                        GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mainmenu);
                    }
                });
            });
        });


    }

    public void FetchAllResult()
    {
        for (int i = 0; i < currentQuestionsObj.Count; i++)
        {
            var q = currentQuestions[i];
            var q_obj = currentQuestionsObj[i];

            var aType = !PlayerInfoManager.instance.GetEvalStatus(q.dimension) ? AnswerType._EVAL_PRE : AnswerType._EVAL_POST;

            var a = new Answer(
                PlayerInfoManager.instance.currentPlayerId,
                aType,
                q.id,
                q.dimension,
                q_obj.GetResult(),
                true
                );
            currentAnswer.Add(a);
        }
    }

    public bool IsAllAnswered()
    {
        var isAllAnswered = true;

        for (int i = 0; i < currentQuestionsObj.Count; i++)
        {
            if (currentQuestionsObj[i].GetResult() == -1) isAllAnswered = false;
        }

        return isAllAnswered;
    }

    public void OnBackToMenu()
    {
        GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mainmenu);
    }


    // Start is called before the first frame update
    void Start()
    {
        InitTest();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void Resize(bool jumptolast = false)
    {
        StartCoroutine(_resize(jumptolast));
    }
    IEnumerator _resize(bool jumptolast)
    {
        yield return new WaitForEndOfFrame();
        var totalY = 0f;
        for (int i = 0; i < evalPanel.childCount; i++)
        {
            totalY += evalPanel.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }

        var layoutGroup = evalPanel.GetComponent<HorizontalOrVerticalLayoutGroup>();

        totalY += layoutGroup.spacing * (evalPanel.childCount - 1) + layoutGroup.padding.top + layoutGroup.padding.bottom;

        evalPanel.sizeDelta = new Vector2(
            evalPanel.sizeDelta.x,
            totalY
        );
        if (jumptolast)
        {
            evalPanel.anchoredPosition = new Vector2(
                evalPanel.anchoredPosition.x,
                0
                );
        }
    }
}
