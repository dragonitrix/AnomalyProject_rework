using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DialoguePiece_fadein : DialoguePiece
{
    public float fadeinDuration = 0.5f;
    public override void ShowText()
    {
        base.ShowText();
        canvasGroup.alpha = 0f;


        System.Action<ITween<float>> onUpdate = (t) =>
        {
            canvasGroup.alpha = t.CurrentValue;
        };

        System.Action<ITween<float>> onComplete = (t) =>
        {
            canvasGroup.alpha = t.CurrentValue;
        };


        gameObject.Tween(null, 0, 1, fadeinDuration, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);
    }
}
