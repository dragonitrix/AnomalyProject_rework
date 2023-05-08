using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionPageController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<QuestionChoiceController> questionChoices = new List<QuestionChoiceController>();

    public QuestionTestManager manager;

    public void InitQuestionPage(QuestionTestManager manager)
    {
        this.manager = manager;

        for (int i = 0; i < questionChoices.Count; i++)
        {
            questionChoices[i].InitChoice(this, i);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetQuestion(QuestionData question)
    {
        question.RefineQuestion();

        questionText.text = question.question;

        //switch (question.type)
        //{
        //    case QuestionData.Type._0_truefalse:
        //        break;
        //    case QuestionData.Type._1_multichoice:
        //    case QuestionData.Type._2_filling:
        //        break;
        //}
        for (int i = 0; i < question.choices.Count; i++)
        {
            var choice = question.choices[i];
            if (choice != "")
            {
                questionChoices[i].SetChoice(question.choices[i]);
            }
            else
            {
                questionChoices[i].HideChoice();
            }
        }
    }

    public void OnChoiceClick(int index)
    {

        if (manager.isTweening) return;

        for (int i = 0; i < questionChoices.Count; i++)
        {
            if (i == index)
            {
                questionChoices[i].canvasGroup.blocksRaycasts = false;
            }
            else
            {
                questionChoices[i].canvasGroup.alpha = questionChoices[i].canvasGroup.alpha == 0 ? 0 : 0.7f;
                questionChoices[i].canvasGroup.interactable = false;
                questionChoices[i].canvasGroup.blocksRaycasts = false;
            }
        }

        manager.OnChoiceClick(index);
    }



}
