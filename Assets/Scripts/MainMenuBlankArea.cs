using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuBlankArea : MonoBehaviour, IPointerClickHandler
{
    public delegate void OnPanelClickDelegate();
    public OnPanelClickDelegate onPanelClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        onPanelClick();
    }

}
