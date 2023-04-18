using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DigitalRuby.Tween;

public class MissionManager : MonoBehaviour
{
    public RectTransform missionPanel;

    public List<MissionPage> missionPages = new List<MissionPage>();

    public int missionpage_index = 0;
    public int missionpage_preview_index = 0;
    public int missionpage_preview_max = 0;

    public bool isTweening = false;

    [Header("UI")]
    public CanvasGroup btn_prev_cg;
    public CanvasGroup btn_next_cg;

    // Start is called before the first frame update
    void Start()
    {
        FetchPages();
        InitMissionPage();
        StartMission();
        UpdatePageButton();
    }

    public void FetchPages()
    {
        missionPages.Clear();
        foreach (Transform t in missionPanel)
        {
            missionPages.Add(t.GetComponent<MissionPage>());
        }
    }

    public void InitMissionPage()
    {
        foreach (var item in missionPages)
        {
            item.manager = this;
            item.InitPage();
        }
    }

    public void StartMission()
    {
        Debug.Log("StartMission");
        missionpage_index = 0;
        SetCurrentMissionPage();
    }

    public void NextMissionPage()
    {
        if (missionpage_index >= 0 && missionpage_index < missionPages.Count - 1)
        {
            missionpage_index++;
            if (missionpage_preview_max < missionpage_index) missionpage_preview_max = missionpage_index;
            SetCurrentMissionPage();
            MoveNext();
        }
        else
        {
            OnFinished();
        }
    }

    public void SetCurrentMissionPage()
    {
        GetCurrentMissionPage().StartPage();
    }

    public void OnCurrentPageFinished()
    {
        NextMissionPage();
    }

    public void OnFinished()
    {
        Debug.Log("mission finished");
    }

    public void MovePrev()
    {
        if (isTweening) return;
        if (missionpage_preview_index <= 0) return;
        missionpage_preview_index--;
        MoveToCurrentPreview();
    }
    public void MoveNext()
    {
        if (isTweening) return;
        if (missionpage_preview_index >= missionpage_preview_max) return;
        missionpage_preview_index++;
        MoveToCurrentPreview();
    }

    public void MoveToCurrentPreview()
    {

        if (isTweening) return;

        var screenWidth = 1920;
        //missionPanel.anchoredPosition = new Vector2(
        //    missionpage_preview_index * -screenWidth,
        //    missionPanel.anchoredPosition.y
        //    );

        var target = new Vector2(
            missionpage_preview_index * -screenWidth,
            missionPanel.anchoredPosition.y
            );

        System.Action<ITween<Vector2>> onUpdate = (t) =>
        {
            missionPanel.anchoredPosition = t.CurrentValue;
        };

        System.Action<ITween<Vector2>> onComplete = (t) =>
        {
            isTweening = false;
            UpdatePageButton();
        };

        isTweening = true;

        gameObject.Tween("", missionPanel.anchoredPosition, target, 0.5f, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);
    }

    protected MissionPage GetCurrentMissionPage()
    {
        if (missionpage_index >= 0 && missionpage_index < missionPages.Count)
        {
            Debug.Log("??");
            return missionPages[missionpage_index];
        }
        else
        {
            return null;
        }
    }

    void UpdatePageButton()
    {
        //Debug.Log("missionpage_preview_index: " + missionpage_preview_index);
        if (missionpage_preview_index <= 0)
        {
            btn_prev_cg.alpha = 0.5f;
        }
        else
        {
            btn_prev_cg.alpha = 1f;
        }


        if (missionpage_preview_index >= missionpage_preview_max)
        {
            btn_next_cg.alpha = 0.5f;
        }
        else
        {
            btn_next_cg.alpha = 1f;
        }
    }

}
