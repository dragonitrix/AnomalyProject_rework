using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuContentButton : MonoBehaviour,IPointerEnterHandler, IPointerClickHandler,IPointerExitHandler
{
    MainMenuButton controller;
    public int index;
    public CanvasGroup overlay;

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.OnContentClick(index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        overlay.ShowAll();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overlay.HideAll();
    }
    public void InitContentButton(MainMenuButton controller,int index)
    {
        this.controller = controller;
        this.index = index;
        overlay.HideAll();
    }
}
