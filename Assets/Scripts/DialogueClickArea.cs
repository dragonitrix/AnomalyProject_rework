using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueClickArea : MonoBehaviour, IPointerClickHandler
{
    public DialogueController controller;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        controller.OnClickAreaClicked();
    }


}
