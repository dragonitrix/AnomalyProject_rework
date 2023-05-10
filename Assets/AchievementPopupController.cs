using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopupController : MonoBehaviour
{
    public static AchievementPopupController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public CanvasGroup mainCanvasGroup;
    public CanvasGroup contentCanvasGroup;
    public CanvasGroup overlayCanvasGroup;

    public GameObject subbadge_prefab;
    public RectTransform subbadgePanel;
    public CanvasGroup icon_complete;

    public SimpleProgressbar mainProgressbar;

    public List<SubBadgeController> subBadgeControllers = new List<SubBadgeController>();

    public bool isInit = false;

    public void InitSubBadge()
    {
        for (int i = 0; i < 6; i++)
        {
            var clone = Instantiate(subbadge_prefab, subbadgePanel);
            var script = clone.GetComponent<SubBadgeController>();
            script.SetDimension((Dimension)(i + 1));
            script.Init();
            subBadgeControllers.Add(script);
        }
        isInit = true;
        Resize();
    }

    public void UpdateSubBadges()
    {
        var completeCount = 0;
        var totalCount = 0;
        for (int i = 0; i < subBadgeControllers.Count; i++)
        {
            subBadgeControllers[i].UpdateTask();
            //if (subBadgeControllers[i].complete) completeCount++;

            completeCount += subBadgeControllers[i].completeCount;
            totalCount += subBadgeControllers[i].achievements.Count;
        }


        float progressValue = (float)completeCount / (float)totalCount;

        mainProgressbar.SetValue(progressValue);
        mainProgressbar.SetText((progressValue * 100f).ToString("00") + "%");

        if (completeCount == totalCount)
        {
            icon_complete.alpha = 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitSubBadge();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        DatabaseManagerMongo.instance.FetchAchievementProgress((data) =>
        {
            AchievementManager.instance.UpdateAchievement_Achievement(() =>
            {
                DatabaseManagerMongo.instance.FetchAchievementProgress((data) =>
                {
                    UpdateSubBadges();
                    contentCanvasGroup.ShowAll();
                    overlayCanvasGroup.HideAll();
                });
            });
        });
        mainCanvasGroup.ShowAll();
        contentCanvasGroup.HideAll();
        overlayCanvasGroup.ShowAll();
    }
    public void Hide()
    {
        mainCanvasGroup.HideAll();
    }

    void Resize(bool jumptolast = false)
    {
        StartCoroutine(_resize(jumptolast));
    }
    IEnumerator _resize(bool jumptolast)
    {
        yield return new WaitForEndOfFrame();
        var totalX = 0f;
        var totalY = 0f;
        for (int i = 0; i < subbadgePanel.childCount; i++)
        {
            totalX += subbadgePanel.GetChild(i).GetComponent<RectTransform>().sizeDelta.x;
            totalY += subbadgePanel.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }

        var layoutGroup = subbadgePanel.GetComponent<HorizontalOrVerticalLayoutGroup>();

        totalX += layoutGroup.spacing * (subbadgePanel.childCount - 1) + layoutGroup.padding.left + layoutGroup.padding.right;
        totalY += layoutGroup.spacing * (subbadgePanel.childCount - 1) + layoutGroup.padding.top + layoutGroup.padding.bottom;

        subbadgePanel.sizeDelta = new Vector2(
            totalX,
            subbadgePanel.sizeDelta.y
        );
        if (jumptolast)
        {
            subbadgePanel.anchoredPosition = new Vector2(
                0,
                subbadgePanel.anchoredPosition.y
                );
        }
    }
}
