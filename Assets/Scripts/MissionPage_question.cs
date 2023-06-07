using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionPage_question : MissionPage
{
    [Header("Question")]
    public QuestionObj questionObj;

    [Header("Prefabs")]
    public GameObject choice_prefab;

    [Header("Obj Ref")]
    public TextMeshProUGUI questionText;
    public RectTransform choiceGroup;
    public CanvasGroup resultPanel;

    public CanvasGroup correctTitle;
    public CanvasGroup incorrectTitle;

    public TextMeshProUGUI explanationText;

    List<MissionPage_question_choice> choices = new List<MissionPage_question_choice>();

    int answerIndex = 0;

    public override void InitPage()
    {
        base.InitPage();

        questionText.text = questionObj.question.question;

        for (int i = 0; i < questionObj.question.choices.Count; i++)
        {
            var choice = questionObj.question.choices[i];
            var clone = Instantiate(choice_prefab, choiceGroup);
            var choiceScript = clone.GetComponent<MissionPage_question_choice>();
            choiceScript.controller = this;
            choiceScript.index = i;
            choiceScript.SetText(choice);
            choices.Add(choiceScript);
        }

    }

    public override void StartPage()
    {
        base.StartPage();

    }
    public override void FinishPage()
    {
        base.FinishPage();

    }

    public void OnQuestionClick(int index)
    {
        //Debug.Log("click: " + index);

        answerIndex = index;

        for (int i = 0; i < choices.Count; i++)
        {
            var choice = choices[i];
            if (i != index)
            {
                choice.canvasGroup.alpha = 0.5f;
            }

            choice.canvasGroup.interactable = false;
            choice.canvasGroup.blocksRaycasts = false;
        }

        //check answer
        var result = CheckAnswer(index);
        ShowResult(result);

        manager.score_max++;

        switch (result)
        {
            case true:
                manager.score++;
                choices[index].OnCorrectAnswer();
                break;
            case false:
                choices[index].OnWrongAnswer();
                break;
        }



        if (!result)
        {
            manager.DeductHealth(1);
        }

    }

    bool CheckAnswer(int rawindex)
    {

        var answer = rawindex + 1;

        if (questionObj.question.answer.ToString().Length == 1)
        {
            if (answer == questionObj.question.answer)
            {
                return true;
            }
        }
        else
        {
            var multianswer = questionObj.question.answer.ToString();
            for (int i = 0; i < multianswer.Length; i++)
            {
                if (answer.ToString() == multianswer[i].ToString())
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void OnResultClicked()
    {
        FinishPage();

        resultPanel.HideAll();
    }

    public void ShowResult(bool result)
    {
        resultPanel.ShowAll();
        if (result)
        {
            correctTitle.alpha = 1;
            incorrectTitle.alpha = 0;
        }
        else
        {
            correctTitle.alpha = 0;
            incorrectTitle.alpha = 1;
        }

        var id = questionObj.question.id[questionObj.question.id.Length - 1].ToString();
        var int_id = int.Parse(id);

        var explanation = manager.missionExplanation.missionExplanation[int_id - 1];

        explanationText.text = explanation.explanation[answerIndex];

    }

}
