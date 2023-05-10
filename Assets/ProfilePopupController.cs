using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProfilePopupController : MonoBehaviour
{
    List<EvalData> evalDatas = new List<EvalData>();
    List<Answer> playerEval = new List<Answer>();

    public WebGraphController webGraph;

    [ContextMenu("GetGraphValue")]
    public void GetGraphValue()
    {
        //DatabaseManagerMongo.instance.FetchPlayerEval((data) =>
        //{
        //    playerEval = data.ToList();
        //    DatabaseManagerMongo.instance.FetchAllEval((data) =>
        //    {
        //        evalDatas = data.ToList();
        //        CalculateEvalScore();
        //    });
        //});

        // type: 2=pre test, 3=post test

        DatabaseManagerMongo.instance.GetPlayerEvalScore(2, (data) =>
        {

            for (int i = 0; i < data.Count; i++)
            {
                Debug.Log("data: " + (i+1).ToString() + " " + data[i]);
            }

            webGraph.SetShapeValue(data);
        });


    }

    //public void CalculateEvalScore()
    //{
    //    List<float> scores = new List<float>();
    //
    //    for (int i = 0; i < 6; i++)
    //    {
    //        var dimension = (Dimension)(i + 1);
    //        var evals = evalDatas.GetEvalRange(dimension);
    //        var score = playerEval.GetEvalAnswerScore(dimension);
    //
    //        var total = evals.Count * 4;
    //
    //        Debug.Log($"cal score: {dimension} {score} of {total}");
    //        scores.Add((float)score / (float)total);
    //    }
    //
    //    webGraph.SetShapeValue(scores);
    //}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
