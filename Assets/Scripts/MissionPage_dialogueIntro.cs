using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MissionPage_dialogueIntro : MissionPage_dialogue
{
    public CanvasGroup bgCanvasGroup;

    Tween<float> enterTween, exitTween = null;

    public override void OnEnterPreview()
    {
        if (exitTween != null)
        {
            exitTween.Stop(TweenStopBehavior.Complete);
        }

        System.Action<ITween<float>> onUpdate = (t) =>
        {
            bgCanvasGroup.alpha = t.CurrentValue;
        };

        System.Action<ITween<float>> onComplete = (t) =>
        {
            bgCanvasGroup.alpha = t.CurrentValue;
            enterTween = null;
        };

        enterTween = gameObject.Tween("enterTween", 0f, 1f, 0.5f, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);

    }
    public override void OnExitPreview()
    {
        if (enterTween != null)
        {
            enterTween.Stop(TweenStopBehavior.Complete);
        }

        System.Action<ITween<float>> onUpdate = (t) =>
        {
            bgCanvasGroup.alpha = t.CurrentValue;
        };

        System.Action<ITween<float>> onComplete = (t) =>
        {
            bgCanvasGroup.alpha = t.CurrentValue;
            exitTween = null;
        };

        exitTween = gameObject.Tween("exitTween", 1f, 0f, 0.5f, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);

    }

}
