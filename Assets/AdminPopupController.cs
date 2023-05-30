using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminPopupController : MonoBehaviour
{
    public CanvasGroup mainCanvasGroup;
    public CanvasGroup contentCanvasGroup;
    public CanvasGroup overlayCanvasGroup;

    public GameObject info_prefab;
    public RectTransform contentPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitInfos(List<PlayerScoreInfo> playerScoreInfos)
    {
        for (int i = 0; i < playerScoreInfos.Count; i++)
        {
            var playerScoreInfo = playerScoreInfos[i];

            var clone = Instantiate(info_prefab, contentPanel);
            var script = clone.GetComponent<PlayerInfoListController>();
            script.SetText(playerScoreInfo);
        }
        Resize();
    }

    public void Show()
    {
        //
        Debug.Log("groupid: " + PlayerInfoManager.instance.account.groupid);
        DatabaseManagerMongo.instance.FetchPlayerScoreInfos(PlayerInfoManager.instance.account.groupid, (data) =>
        {
            Debug.Log("fect data complete");
            InitInfos(data);
            contentCanvasGroup.ShowAll();
            overlayCanvasGroup.HideAll();
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
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            totalX += contentPanel.GetChild(i).GetComponent<RectTransform>().sizeDelta.x;
            totalY += contentPanel.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }

        var layoutGroup = contentPanel.GetComponent<HorizontalOrVerticalLayoutGroup>();

        totalX += layoutGroup.spacing * (contentPanel.childCount - 1) + layoutGroup.padding.left + layoutGroup.padding.right;
        totalY += layoutGroup.spacing * (contentPanel.childCount - 1) + layoutGroup.padding.top + layoutGroup.padding.bottom;

        contentPanel.sizeDelta = new Vector2(
            contentPanel.sizeDelta.x,
            totalY
        );
        if (jumptolast)
        {
            contentPanel.anchoredPosition = new Vector2(
                contentPanel.anchoredPosition.x,
                0
                );
        }
    }
}

[Serializable]
public class PlayerScoreInfo
{
    public string id;
    public string name;
    public string email;
    public List<PlayerScoreProgress> testProgress;
    public List<PlayerScoreProgress> evalProgress_pre;
    public List<PlayerScoreProgress> evalProgress_post;
}

[Serializable]
public class PlayerScoreProgress
{
    public int progress;
    public int total;
}