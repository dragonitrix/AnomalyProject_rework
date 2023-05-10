using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionTestManager : MonoBehaviour
{
    public Dimension dimension;
    public GameObject page_prefab;
    public GameObject resultDetail_prefab;

    public int batchCount = 5;

    public List<QuestionData> currentQuestions = new List<QuestionData>();
    public List<Answer> currentAnswer = new List<Answer>();

    public int currentQuestionIndex = 0;
    public int question_preview_index = 0;
    public int question_preview_max = 0;
    public bool isTweening = false;

    public RectTransform pageGroup;
    public RectTransform resultPage;
    public RectTransform resultContent;
    public List<QuestionPageController> pages = new List<QuestionPageController>();

    [Header("UI")]
    public CanvasGroup btn_prev_cg;
    public CanvasGroup btn_next_cg;

    public PageIndicatorController pageIndicator;

    public void InitTest()
    {
        dimension = QuestionPool.instance.currentDimension;
        NewBatch();
    }

    public void NewBatch()
    {
        var pool = QuestionPool.instance.questions_unanswered;
        //pool.Shuffle();
        currentQuestions.Clear();

        if (pool.Count >= batchCount)
        {
            currentQuestions.AddRange(pool.GetRange(0, batchCount));
            pool.RemoveRange(0, batchCount);
        }
        else
        {
            Debug.Log("out of question");
            currentQuestions.AddRange(pool.GetRange(0, pool.Count));
            pool.Clear();
            pool.AddRange(QuestionPool.instance.questions_all);
            pool.Shuffle();
            currentQuestions.AddRange(pool.GetRange(0, batchCount - currentQuestions.Count));
        }

        currentQuestionIndex = 0;
        question_preview_index = 0;
        question_preview_max = 0;
        //SetQuestion();

        for (int i = 0; i < pages.Count; i++)
        {
            Destroy(pages[i].gameObject);
        }
        pages.Clear();

        for (int i = 0; i < currentQuestions.Count; i++)
        {
            var clone = Instantiate(page_prefab, pageGroup);
            var page = clone.GetComponent<QuestionPageController>();
            page.InitQuestionPage(this);
            page.SetQuestion(currentQuestions[i]);
            pages.Add(page);
        }

        resultPage.SetAsLastSibling();

        pageGroup.sizeDelta = new Vector2(
            1920 * pageGroup.transform.childCount,
            pageGroup.sizeDelta.y
            );

        pageIndicator.InitIndicator(pages.Count + 1);
        LayoutRebuilder.ForceRebuildLayoutImmediate(pageGroup);
        MoveTo(0);

        //currentAnswer.Clear();

    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < batchCount - 1)
        {
            currentQuestionIndex++;
            if (question_preview_max < currentQuestionIndex) question_preview_max = currentQuestionIndex;
            MoveNext();
        }
        else
        {
            OnTestFinished();
        }
    }

    public void OnTestFinished()
    {

        //foreach (var item in questionChoices)
        //{
        //    item.DisableChoice();
        //}

        question_preview_max += 1;

        foreach(Transform t in resultContent)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < currentQuestions.Count; i++)
        {
            var clone = Instantiate(resultDetail_prefab, resultContent);
            var resultDetail = clone.GetComponent<QuestionResultDetail>();
            resultDetail.InitResult(this, i);
            resultDetail.SetCorrect(currentAnswer[i].isCorrected, currentQuestions[i].question);
        }

        MoveNext(() =>
        {
            //update player's answer to database
            GameManager.instance.UpdatePlayerAnswers(currentAnswer, (data) =>
            {
                currentAnswer.Clear();
                //Debug.Log("new batch");
                //NewBatch();
                ShowResult();
                AchievementManager.instance.UpdateAchievement_Test(dimension);
            });
        });

    }

    public void ShowResult()
    {

    }

    public void OnBackToMenu()
    {
        GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mainmenu);
    }

    public void OnContinue()
    {
        Debug.Log("new batch");
        NewBatch();
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

    public void OnChoiceClick(int index)
    {
        var currentQuestion = currentQuestions[currentQuestionIndex];

        var answerChoice = (index + 1);

        var result = currentQuestion.answer == answerChoice;

        var answer = new Answer(
            PlayerInfoManager.instance.currentPlayerId,
            AnswerType._TEST,
            currentQuestion.id,
            currentQuestion.dimension,
            answerChoice,
            result
            );

        //answer.Log();

        Debug.Log("currentQuestion.answer: " + currentQuestion.answer);
        Debug.Log("answer: " + answerChoice);
        Debug.Log("result: " + result);

        currentAnswer.Add(answer);

        NextQuestion();
    }

    public void MovePrev()
    {
        MovePrev(() => { });
    }
    public void MoveNext()
    {
        MoveNext(() => { });
    }

    public void MovePrev(System.Action callback)
    {
        if (isTweening) return;
        if (question_preview_index <= 0) return;
        MoveTo(question_preview_index - 1, callback);
    }
    public void MoveNext(System.Action callback)
    {
        if (isTweening) return;
        if (question_preview_index >= question_preview_max) return;
        MoveTo(question_preview_index + 1, callback);
    }

    public void MoveTo(int index)
    {
        MoveTo(index, () => { });
    }

    public void MoveTo(int index, System.Action callback)
    {
        question_preview_index = index;
        pageIndicator.MoveIndicator(index);
        MoveToCurrentPreview(callback);
    }

    public void MoveToCurrentPreview(System.Action callback)
    {

        if (isTweening) return;

        var screenWidth = 1920;
        //missionPanel.anchoredPosition = new Vector2(
        //    question_preview_index * -screenWidth,
        //    missionPanel.anchoredPosition.y
        //    );

        var target = new Vector2(
            question_preview_index * -screenWidth,
            pageGroup.anchoredPosition.y
            );

        System.Action<ITween<Vector2>> onUpdate = (t) =>
        {
            pageGroup.anchoredPosition = t.CurrentValue;
        };

        System.Action<ITween<Vector2>> onComplete = (t) =>
        {
            isTweening = false;
            UpdatePageButton();
            callback();
        };

        isTweening = true;

        gameObject.Tween(null, pageGroup.anchoredPosition, target, 0.5f, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);
    }

    protected QuestionPageController GetCurrentPreviewPage()
    {
        if (question_preview_index >= 0 && question_preview_index < pages.Count)
        {
            return pages[question_preview_index];
        }
        else
        {
            return null;
        }
    }

    void UpdatePageButton()
    {
        if (question_preview_index <= 0)
        {
            btn_prev_cg.alpha = 0.5f;
        }
        else
        {
            btn_prev_cg.alpha = 1f;
        }


        if (question_preview_index >= question_preview_max)
        {
            btn_next_cg.alpha = 0.5f;
        }
        else
        {
            btn_next_cg.alpha = 1f;
        }
    }
}
