using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionPage : MonoBehaviour
{
    [HideInInspector]
    public MissionManager manager;
    public bool isFinished = false;

    public virtual void InitPage()
    {
        Debug.Log("init page");
    }

    public virtual void StartPage()
    {
        Debug.Log("start page");

    }
    public virtual void FinishPage()
    {
        if (isFinished)
        {
            return;
        }
        Debug.Log("finish page");
        isFinished = true;
        manager.OnCurrentPageFinished();
    }
}
