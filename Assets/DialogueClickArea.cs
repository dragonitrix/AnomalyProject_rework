using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueClickArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler
{
    public DialogueController controller;
    public ScrollRect scrollRect;
    private Vector3 mousePosOnDragStart;
    private bool passingEvent = false;

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        //Debug.Log("OnBeginDrag");
        mousePosOnDragStart = Input.mousePosition;
        // Or something else you need to do at the start of the drag.
    }

    public void OnDrag(PointerEventData pointerEventData)
    {

        var threshold = EventSystem.current.pixelDragThreshold;

        //Debug.Log("OnDrag: " + (Input.mousePosition - mousePosOnDragStart).sqrMagnitude);
        if ((Input.mousePosition - mousePosOnDragStart).sqrMagnitude > threshold && !passingEvent) // Checks if mouse has moved further than 1. Use your on logic here
        {
            ExecuteEvents.Execute(scrollRect.gameObject, pointerEventData, ExecuteEvents.beginDragHandler);
            passingEvent = true;
        }

        if (passingEvent) // Don't send dragHandler before beginDragHandler has been called. It gives unwanted results...
        {
            ExecuteEvents.Execute(scrollRect.gameObject, pointerEventData, ExecuteEvents.dragHandler);
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        //Debug.Log("OnEndDrag");
        ExecuteEvents.Execute(scrollRect.gameObject, pointerEventData, ExecuteEvents.endDragHandler);
        passingEvent = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (passingEvent) return;
        controller.OnClickAreaClicked();
    }


}
