using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageIndicatorController : MonoBehaviour
{

    public GameObject indicatorSmall_prefab;
    public RectTransform indicatorGroup;
    public RectTransform indicatorMain;

    public void InitIndicator(int page)
    {
        foreach (Transform indicator in indicatorGroup)
        {
            Destroy(indicator.gameObject);
        }

        for (int i = 0; i < page; i++)
        {
            Instantiate(indicatorSmall_prefab, indicatorGroup);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(indicatorGroup);

        var target = indicatorGroup.GetChild(0).GetComponent<RectTransform>().localPosition;
        target.y = 0;
        indicatorMain.anchoredPosition = target;
    }

    public void MoveIndicator(int index)
    {
        var target = indicatorGroup.GetChild(index).GetComponent<RectTransform>().localPosition;
        target.y = 0;

        System.Action<ITween<Vector2>> onUpdate = (t) =>
        {
            indicatorMain.anchoredPosition = t.CurrentValue;
        };

        System.Action<ITween<Vector2>> onComplete = (t) =>
        {
            indicatorMain.anchoredPosition = t.CurrentValue;
        };

        gameObject.Tween("indicatorTween", indicatorMain.anchoredPosition, target, 0.5f, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);
    }
}
