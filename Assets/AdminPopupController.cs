using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminPopupController : MonoBehaviour
{
    public CanvasGroup mainCanvasGroup;
    public CanvasGroup contentCanvasGroup;
    public CanvasGroup overlayCanvasGroup;

    public RectTransform contentPanel;

    public GameObject graph_prefab;
    public GameObject graphDetail_prefab;

    public RectTransform testScorePanel;
    public RectTransform missionScorePanel;
    public RectTransform evalScorePanel;

    public RectTransform testScoreDetailPanel;
    public RectTransform missionScoreDetailPanel;
    public RectTransform evalScoreDetailPanel;

    public List<Color> testScoreColors = new List<Color>();
    public List<Color> missionScoreColors = new List<Color>();
    public List<Color> evalScoreColors = new List<Color>();

    public List<string> testScoreTexts = new List<string>();
    public List<string> missionScoreTexts = new List<string>();
    public List<string> evalScoreTexts = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ClearAll()
    {
        foreach (Transform transform in testScorePanel)
        {
            Destroy(transform.gameObject);
        }
        foreach (Transform transform in missionScorePanel)
        {
            Destroy(transform.gameObject);
        }
        foreach (Transform transform in evalScorePanel)
        {
            Destroy(transform.gameObject);
        }

        foreach (Transform transform in testScoreDetailPanel)
        {
            Destroy(transform.gameObject);
        }
        foreach (Transform transform in missionScoreDetailPanel)
        {
            Destroy(transform.gameObject);
        }
        foreach (Transform transform in evalScoreDetailPanel)
        {
            Destroy(transform.gameObject);
        }
    }

    public void InitInfos(GroupScores groupScores)
    {

        ClearAll();

        float height = Mathf.Ceil((float)groupScores.count / 20f) * 20;
        float subDivision = (height <= 20) ? 5 : 20;

        for (int i = 0; i < groupScores.testDatas.Count; i++)
        {
            var clone = Instantiate(graph_prefab, testScorePanel);
            var graph = clone.GetComponent<BarGraphController>();
            graph.InitGraph(height, 50, subDivision);
            graph.SetGraphValue(groupScores.testDatas[i]);
            graph.SetGraphText($"D{i + 1}");
            graph.SetGraphColor(testScoreColors);

            var ts = new List<List<string>>();
            for (int j = 0; j < groupScores.testDatas[i].Count; j++)
            {
                var t = new List<string>();
                t.Add("value: " + groupScores.testDatas[i][j].ToString());
                t.Add("sd: ?");
                ts.Add(t);
            }
            graph.SetGraphInfoText(ts);
        }
        for (int i = 0; i < testScoreTexts.Count; i++)
        {
            var clone = Instantiate(graphDetail_prefab, testScoreDetailPanel);
            clone.transform.GetChild(0).GetComponent<Image>().color = testScoreColors[i];
            clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = testScoreTexts[i];
        }

        for (int i = 0; i < groupScores.missionDatas.Count; i++)
        {
            var clone = Instantiate(graph_prefab, missionScorePanel);
            var graph = clone.GetComponent<BarGraphController>();
            graph.InitGraph(height, 70, subDivision);
            graph.SetGraphValue(groupScores.missionDatas[i]);
            graph.SetGraphText($"D{i + 1}");
            graph.SetGraphColor(missionScoreColors);

            var ts = new List<List<string>>();
            for (int j = 0; j < groupScores.missionDatas[i].Count; j++)
            {
                var t = new List<string>();
                t.Add("value: " + groupScores.missionDatas[i][j].ToString());
                t.Add("sd: ?");
                ts.Add(t);
            }
            graph.SetGraphInfoText(ts);
        }
        for (int i = 0; i < missionScoreTexts.Count; i++)
        {
            var clone = Instantiate(graphDetail_prefab, missionScoreDetailPanel);
            clone.transform.GetChild(0).GetComponent<Image>().color = missionScoreColors[i];
            clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = missionScoreTexts[i];
        }

        for (int i = 0; i < groupScores.evalCounts.Count; i++)
        {
            var clone = Instantiate(graph_prefab, evalScorePanel);
            var graph = clone.GetComponent<BarGraphController>();
            graph.InitGraph(height, 100, subDivision);
            graph.SetGraphValue(groupScores.evalCounts[i]);
            graph.SetGraphText($"D{i + 1}");
            graph.SetGraphColor(evalScoreColors);

            var ts = new List<List<string>>();
            for (int j = 0; j < groupScores.evalCounts[i].Count; j++)
            {
                var t = new List<string>();
                t.Add("value: " + groupScores.evalCounts[i][j].ToString());
                t.Add("sd: ?");
                ts.Add(t);
            }
            graph.SetGraphInfoText(ts);
        }
        for (int i = 0; i < evalScoreTexts.Count; i++)
        {
            var clone = Instantiate(graphDetail_prefab, evalScoreDetailPanel);
            clone.transform.GetChild(0).GetComponent<Image>().color = evalScoreColors[i];
            clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = evalScoreTexts[i];
        }

        //Resize();
    }

    public void Show()
    {
        //
        //Debug.Log("groupid: " + PlayerInfoManager.instance.account.groupid);
        //DatabaseManagerMongo.instance.FetchPlayerScoreInfos(PlayerInfoManager.instance.account.groupid, (data) =>
        //{
        //    Debug.Log("fect data complete");
        //    InitInfos(data);
        //    contentCanvasGroup.ShowAll();
        //    overlayCanvasGroup.HideAll();
        //});


        Debug.Log("groupid: " + PlayerInfoManager.instance.account.groupid);
        DatabaseManagerMongo.instance.FetchGroupScores(PlayerInfoManager.instance.account.groupid, (data) =>
        {
            Debug.Log("fect data complete");
            InitInfos(data);
            OnTestBtnClick();
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

    public void OnTestBtnClick()
    {
        testScorePanel.GetComponent<CanvasGroup>().ShowAll();
        missionScorePanel.GetComponent<CanvasGroup>().HideAll();
        evalScorePanel.GetComponent<CanvasGroup>().HideAll();

        testScoreDetailPanel.GetComponent<CanvasGroup>().ShowAll();
        missionScoreDetailPanel.GetComponent<CanvasGroup>().HideAll();
        evalScoreDetailPanel.GetComponent<CanvasGroup>().HideAll();

    }
    public void OnMissionBtnClick()
    {
        testScorePanel.GetComponent<CanvasGroup>().HideAll();
        missionScorePanel.GetComponent<CanvasGroup>().ShowAll();
        evalScorePanel.GetComponent<CanvasGroup>().HideAll();

        testScoreDetailPanel.GetComponent<CanvasGroup>().HideAll();
        missionScoreDetailPanel.GetComponent<CanvasGroup>().ShowAll();
        evalScoreDetailPanel.GetComponent<CanvasGroup>().HideAll();
    }
    public void OnEvalBtnClick()
    {
        testScorePanel.GetComponent<CanvasGroup>().HideAll();
        missionScorePanel.GetComponent<CanvasGroup>().HideAll();
        evalScorePanel.GetComponent<CanvasGroup>().ShowAll();

        testScoreDetailPanel.GetComponent<CanvasGroup>().HideAll();
        missionScoreDetailPanel.GetComponent<CanvasGroup>().HideAll();
        evalScoreDetailPanel.GetComponent<CanvasGroup>().ShowAll();
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