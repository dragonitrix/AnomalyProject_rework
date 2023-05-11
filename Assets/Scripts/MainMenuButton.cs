using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour,IPointerClickHandler
{
    public Vector2 idleSize;
    public Vector2 expandSize;
    public Vector2 shrinkSize;

    public State state;

    Tween<Vector2> idleTween;
    Tween<Vector2> expandTween;
    Tween<Vector2> shrinkTween;

    public RectTransform rectTransform;
    public float tweenDuration = 0.25f;

    public CanvasGroup mainCanvasGroup;
    public CanvasGroup iconCanvasGroup;
    public CanvasGroup contentCanvasGroup;

    public List<MainMenuContentButton> contents = new List<MainMenuContentButton>();

    public delegate void OnPanelClickDelegate();
    public OnPanelClickDelegate onPanelClick;

    public delegate void OnContentClickDelegate(int index);
    public OnContentClickDelegate onContentClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound("click_scifi");

        onPanelClick();
        //switch (state)
        //{
        //    case State._IDLE:
        //        SetState(State._EXPAND);
        //        break;
        //    case State._EXPAND:
        //        SetState(State._IDLE);
        //        break;
        //    case State._SHRINK:
        //        break;
        //    default:
        //        break;
        //}
    }

    public void SetState(State state)
    {
        if (this.state == state)
        {
            return;
        }
        this.state = state;
        switch (this.state)
        {
            case State._IDLE:
                ToIdle();
                break;
            case State._EXPAND:
                ToExpand();
                break;
            case State._SHRINK:
                ToShrink();
                break;
            default:
                break;
        }
    }

    public void ToExpand()
    {

        System.Action<ITween<Vector2>> onUpdate = (t) =>
        {
            rectTransform.sizeDelta = t.CurrentValue;
            iconCanvasGroup.alpha = t.CurrentProgress.Remap(0, 1, 1, 0);
            if (t.CurrentProgress >= 0.5f)
            {
                contentCanvasGroup.alpha = t.CurrentProgress.Remap(0.5f, 1, 0, 1);
            }
        };

        System.Action<ITween<Vector2>> onComplete = (t) =>
        {
            rectTransform.sizeDelta = t.CurrentValue;
            iconCanvasGroup.alpha = t.CurrentProgress.Remap(0, 1, 1, 0);
            contentCanvasGroup.ShowAll();
        };
        expandTween =  gameObject.Tween(null, rectTransform.sizeDelta, expandSize, tweenDuration, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);
    }

    public void ToIdle()
    {
        contentCanvasGroup.interactable = false;
        contentCanvasGroup.blocksRaycasts = false;
        contentCanvasGroup.alpha = 0;
        System.Action<ITween<Vector2>> onUpdate = (t) =>
        {
            rectTransform.sizeDelta = t.CurrentValue;
            if (iconCanvasGroup.alpha < t.CurrentProgress) iconCanvasGroup.alpha = t.CurrentProgress;
            //if (t.CurrentProgress <= 0.5f)
            //{
            //    contentCanvasGroup.alpha = t.CurrentProgress.Remap(0, 0.5f, 1, 0);
            //}
        };

        System.Action<ITween<Vector2>> onComplete = (t) =>
        {
            rectTransform.sizeDelta = t.CurrentValue;
            iconCanvasGroup.alpha = t.CurrentProgress;
            contentCanvasGroup.HideAll();
        };
        idleTween = gameObject.Tween(null, rectTransform.sizeDelta, idleSize, tweenDuration, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);

    }

    public void ToShrink()
    {
        contentCanvasGroup.interactable = false;
        contentCanvasGroup.blocksRaycasts = false;
        contentCanvasGroup.alpha = 0;
        System.Action<ITween<Vector2>> onUpdate = (t) =>
        {
            rectTransform.sizeDelta = t.CurrentValue;
            if(iconCanvasGroup.alpha< t.CurrentProgress) iconCanvasGroup.alpha = t.CurrentProgress;
        };

        System.Action<ITween<Vector2>> onComplete = (t) =>
        {
            rectTransform.sizeDelta = t.CurrentValue;
            iconCanvasGroup.alpha = t.CurrentProgress;
        };
        idleTween = gameObject.Tween(null, rectTransform.sizeDelta, shrinkSize, tweenDuration, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);

    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < contents.Count; i++)
        {
            contents[i].InitContentButton(this, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnContentClick(int index)
    {
        AudioManager.instance.PlaySound("click_soft");
        onContentClick(index);
    }


    public enum State
    {
        _IDLE,
        _EXPAND,
        _SHRINK
    }



}
