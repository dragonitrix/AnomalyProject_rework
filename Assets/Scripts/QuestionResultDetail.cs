using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionResultDetail : MonoBehaviour
{
    public CanvasGroup correct_cg;
    public CanvasGroup incorrect_cg;

    public TextMeshProUGUI textMesh;

    public Button button;

    int index;

    QuestionTestManager manager;

    public void SetCorrect(bool val, string text)
    {
        if (val)
        {
            correct_cg.ShowAll();
            incorrect_cg.HideAll();
        }
        else
        {
            correct_cg.HideAll();
            incorrect_cg.ShowAll();
        }
        textMesh.text = text;
    }

    public void InitResult(QuestionTestManager manager,int index)
    {
        this.manager = manager;
        this.index = index;

        button.onClick.AddListener(() =>
        {
            manager.MoveTo(index);
        });
    }

}
