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

    // Start is called before the first frame update
    void Start()
    {
        
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
        //throw new System.NotImplementedException();
        image.color = new Color(91f / 225f, 95f / 255f, 210f / 255f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        image.color = new Color(1, 1, 1);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnClick()
    {
        controller.OnQuestionClick(index);
    }

}
