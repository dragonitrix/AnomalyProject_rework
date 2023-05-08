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

    public QuestionPageController controller;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }
    public void InitChoice(QuestionPageController controller,int index)
    {
        this.controller = controller;
        this.index = index;
    }
    public void SetChoice(string text)
    {
        choiceText.text = text;
        canvasGroup.ShowAll();
    }

    public void HideChoice()
    {
        canvasGroup.HideAll();
    }

    public void DisableChoice()
    {
        //canvasGroup.HideAll();
        canvasGroup.interactable = false;
    }

    public void OnClick()
    {
        controller.OnChoiceClick(index);
    }

}
