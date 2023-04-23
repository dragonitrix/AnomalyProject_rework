using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionTestManager : MonoBehaviour
{

    public int batchCount = 5;

    public List<QuestionData> currentQuestions = new List<QuestionData>();

    public List<Answer> currentAnswer = new List<Answer>();

    public int currentQuestionIndex = 0;

    public TextMeshProUGUI questionText;

    public List<QuestionChoiceController> questionChoices = new List<QuestionChoiceController>();




    public void InitTest()
    {

        for (int i = 0; i < questionChoices.Count; i++)
        {
            questionChoices[i].InitChoice(this,i);
        }

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
        SetQuestion();

        //currentAnswer.Clear();

    }

    public void SetQuestion()
    {
        var question = currentQuestions[currentQuestionIndex];
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
                questionChoices[i].DisableChoice();
            }
        }
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < batchCount - 1)
        {
            currentQuestionIndex++;
            SetQuestion();
        }
        else
        {
            OnTestFinished();
        }
    }

    public void OnTestFinished()
    {

        foreach (var item in questionChoices)
        {
            item.DisableChoice();
        }

        //update player's answer to database
        GameManager.instance.UpdatePlayerAnswers(currentAnswer, (data) => {
            currentAnswer.Clear();
            Debug.Log("new batch");
            NewBatch();
        });

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

        var result = currentQuestion.answer == index;

        var answer = new Answer(
            PlayerInfoManager.instance.CurrentPlayerId,
            AnswerType._TEST,
            currentQuestion.id,
            currentQuestion.dimension,
            index,
            result
            );

        currentAnswer.Add(answer);

        NextQuestion();
    }

}
