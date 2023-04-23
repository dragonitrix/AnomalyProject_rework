using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionChoiceController : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI choiceText;
    public CanvasGroup canvasGroup;
    public Button button;

    public QuestionTestManager manager;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }
    public void InitChoice(QuestionTestManager manager,int index)
    {
        this.manager = manager;
        this.index = index;
    }
    public void SetChoice(string text)
    {
        choiceText.text = text;
        canvasGroup.ShowAll();
    }

    public void DisableChoice()
    {
        canvasGroup.HideAll();
    }

    public void OnClick()
    {
        manager.OnChoiceClick(index);
    }

}
