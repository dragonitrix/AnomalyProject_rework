using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DigitalRuby.Tween;
using UnityEditor.Tilemaps;
using static UnityEngine.GraphicsBuffer;
using System.Reflection;

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

    public PageIndicatorController pageIndicator;

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

        pageIndicator.InitIndicator(missionPages.Count);

    }

    public void StartMission()
    {
        Debug.Log("StartMission");
        missionpage_index = 0;

        var target = new Vector2(
            0,
            missionPanel.anchoredPosition.y
            );

        missionPanel.anchoredPosition = target;

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
        MoveTo(missionpage_preview_index - 1);
    }
    public void MoveNext()
    {
        if (isTweening) return;
        if (missionpage_preview_index >= missionpage_preview_max) return;
        MoveTo(missionpage_preview_index + 1);
    }

    public void MoveTo(int index)
    {
        GetCurrentPreviewPage().OnExitPreview();
        missionpage_preview_index = index;
        pageIndicator.MoveIndicator(index);
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
            GetCurrentPreviewPage().OnEnterPreview();
            UpdatePageButton();
        };

        isTweening = true;

        gameObject.Tween("", missionPanel.anchoredPosition, target, 0.5f, TweenScaleFunctions.QuadraticEaseOut, onUpdate, onComplete);
    }

    protected MissionPage GetCurrentMissionPage()
    {
        if (missionpage_index >= 0 && missionpage_index < missionPages.Count)
        {
            return missionPages[missionpage_index];
        }
        else
        {
            return null;
        }
    }

    protected MissionPage GetCurrentPreviewPage()
    {
        if (missionpage_preview_index >= 0 && missionpage_preview_index < missionPages.Count)
        {
            return missionPages[missionpage_preview_index];
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
