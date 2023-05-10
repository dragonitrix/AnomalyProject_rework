using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Networking.UnityWebRequest;
using System.Reflection;

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
    public CanvasGroup correctPanel;
    public CanvasGroup incorrectPanel;

    List<MissionPage_question_choice> choices = new List<MissionPage_question_choice>();

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

        if (!result)
        {
            manager.DeductHealth(1);
            manager.isPerfectScore = false;
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
            correctPanel.alpha = 1;
        }
        else
        {
            incorrectPanel.alpha = 1;
        }
    }

}
