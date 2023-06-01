using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MissionPage_question_choice : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{

    public CanvasGroup canvasGroup;
    public Image image;
    public TextMeshProUGUI textMesh;

    public MissionPage_question controller;

    public int index;

    public Color hover_tintColor = new Color(91f / 225f, 95f / 255f, 210f / 255f);
    public Color correct_tintColor = new Color(91f / 225f, 95f / 255f, 210f / 255f);
    public Color wrong_tintColor = new Color(91f / 225f, 95f / 255f, 210f / 255f);

    Color textMeshColor_origin;

    // Start is called before the first frame update
    void Start()
    {
        textMeshColor_origin = textMesh.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canvasGroup.interactable) return;
        //throw new System.NotImplementedException();
        image.color = hover_tintColor;
        textMesh.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!canvasGroup.interactable) return;
        //throw new System.NotImplementedException();
        image.color = Color.white;
        textMesh.color = textMeshColor_origin;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnClick()
    {
        controller.OnQuestionClick(index);
    }

    public void OnCorrectAnswer()
    {
        image.color = correct_tintColor;
        textMesh.color = Color.white;
    }

    public void OnWrongAnswer()
    {
        image.color = wrong_tintColor;
        textMesh.color = Color.white;
    }
}
