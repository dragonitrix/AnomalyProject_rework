using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    public static void ShowAll(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public static void HideAll(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public static void SetValue(this CanvasGroup canvasGroup, bool value)
    {
        canvasGroup.alpha = value ? 1 : 0;
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }
    public static void SetValue(this CanvasGroup canvasGroup, float alpha,bool interactable,bool blocksRaycasts)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = blocksRaycasts;
    }

    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static int GetEvalAnswerScore(this List<Answer> answers, Dimension dimension)
    {
        int score = 0;

        var filtered = answers.GetAnswerRange(dimension);

        for (int i = 0; i < filtered.Count; i++)
        {
            score += filtered[i].answer;
        }

        return score;
    }

    public static List<Answer> GetAnswerRange(this List<Answer> answers, Dimension dimension)
    {
        var _answers = new List<Answer>();

        for (int i = 0; i < answers.Count; i++)
        {
            if ((Dimension)answers[i].dimension == dimension)
            {
                _answers.Add(answers[i]);
            }
        }
        return _answers;
    }

    public static List<EvalData> GetEvalRange(this List<EvalData> evals, Dimension dimension)
    {
        var _evals = new List<EvalData>();

        for (int i = 0; i < evals.Count; i++)
        {
            if ((Dimension)evals[i].dimension == dimension)
            {
                _evals.Add(evals[i]);
            }
        }
        return _evals;
    }



    public static List<float> GetSDs(List<List<float>> list)
    {
        var result = new List<float>();

        for (int i = 0; i < list.Count; i++)
        {
            result.Add(GetSD(list[i]));
        }

        return result;
    }

    public static float GetSD(List<float> values)
    {
        float standardDeviation = 0;

        if (values.Any())
        {
            // Compute the average.     
            float avg = values.Average();

            // Perform the Sum of (value-avg)_2_2.      
            float sum = values.Sum(d => Mathf.Pow(d - avg, 2));

            // Put it all together.      
            standardDeviation = Mathf.Sqrt((sum) / (values.Count() - 1));
        }

        return standardDeviation;
    }

    public static float GetAVG(List<float> floats)
    {
        float avg = 0;

        for (int i = 0; i < floats.Count; i++)
        {
            avg += floats[i];
        }
        avg /= floats.Count;

        return avg;
    }

}