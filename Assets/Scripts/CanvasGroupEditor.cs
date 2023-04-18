#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CanvasGroup))]
public class CanvasGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("toggle"))
        {
            toggleCanvasGroup();
        }
    }

    public void toggleCanvasGroup()
    {
        CanvasGroup canvasGroup = (CanvasGroup)target;

        if (canvasGroup.alpha != 1)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

}
#endif