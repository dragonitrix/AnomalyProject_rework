using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminPopupController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CSVDownloader(string str, string fn);

    public GroupInfo groupInfo;

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

    public CSVTemplate csvTemplate;

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
        Debug.Log("groupid: " + PlayerInfoManager.instance.account.groupid);

        DatabaseManagerMongo.instance.GetGroupInfo(PlayerInfoManager.instance.account.groupid, (data) =>
        {
            groupInfo = data;
            groupInfo.Log();
            DatabaseManagerMongo.instance.FetchGroupScores(PlayerInfoManager.instance.account.groupid, (data) =>
            {
                Debug.Log("fect data complete");
                InitInfos(data);
                OnTestBtnClick();
                contentCanvasGroup.ShowAll();
                overlayCanvasGroup.HideAll();
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

    [ContextMenu("OnExportCSVClick")]
    public void OnExportCSVClick()
    {
        contentCanvasGroup.HideAll();
        overlayCanvasGroup.ShowAll();
        RetrivePlayerScoreInfo();
    }

    public void RetrivePlayerScoreInfo()
    {
        DatabaseManagerMongo.instance.FetchPlayerScoreInfos(PlayerInfoManager.instance.account.groupid, (data) =>
        {
            Debug.Log("fect data complete");
            ConvertInfoToCSV(data, true);
            contentCanvasGroup.ShowAll();
            overlayCanvasGroup.HideAll();
        });
    }

    public string ConvertInfoToCSV(List<PlayerScoreInfo> playerScoreInfos, bool saveToDevice)
    {

        var mainCSV = new EZcsv(csvTemplate);

        mainCSV.SetData(groupInfo.adminUser, 1, 2);
        mainCSV.SetData(groupInfo.link, 1, 3);
        mainCSV.SetData(groupInfo.classStartTime.ToShortDateString(), 1, 4);
        mainCSV.SetData(groupInfo.studentCount.ToString(), 1, 5);

        for (int i = 0; i < playerScoreInfos.Count; i++)
        {
            var pScore = playerScoreInfos[i];
            var csv = PlayerScore2CSV(pScore);
            mainCSV.AddRows(csv.datas);
            string[] blankrow = { "", "", "", "", "", "", "" };
            mainCSV.AddRow(blankrow.ToList());
        }

        if (saveToDevice)
        {
            ExportCSVToDevice(mainCSV.GetCSVStrings());
        }


        return mainCSV.GetCSVStrings();

    }

    public EZcsv PlayerScore2CSV(PlayerScoreInfo playerScoreInfo)
    {
        var csv = new EZcsv(7);
        var score = playerScoreInfo.playerScore;

        string[] r1 = { playerScoreInfo.email, "", "", "", "", "", "" };
        csv.AddRow(r1.ToList());

        string[] r2 = { "", "Dimension 1", "Dimension 2", "Dimension 3", "Dimension 4", "Dimension 5", "Dimension 6" };
        csv.AddRow(r2.ToList());

        string[] r3 = { "Test", "", "", "", "", "", "" };
        var r3_l = r3.ToList();
        for (int i = 0; i < score.testScores.Count; i++)
        {
            r3_l[i + 1] = score.testScores[i] + " | " + score.testAnswers[i];
        }
        csv.AddRow(r3_l);

        string[] r4 = { "Mission", "", "", "", "", "", "" };
        var r4_l = r4.ToList();
        for (int i = 0; i < score.missionScores.Count; i++)
        {
            r4_l[i + 1] = score.missionScores[i] + " | " + 3;
        }
        csv.AddRow(r4_l);

        string[] r5 = { "Pre Test", "", "", "", "", "", "" };
        var r5_l = r5.ToList();
        for (int i = 0; i < score.preEvalScores.Count; i++)
        {
            r5_l[i + 1] = score.preEvalScores[i] + " | " + 6;
        }
        csv.AddRow(r5_l);

        string[] r6 = { "Post Test", "", "", "", "", "", "" };
        var r6_l = r6.ToList();
        for (int i = 0; i < score.postEvalScores.Count; i++)
        {
            r6_l[i + 1] = score.postEvalScores[i] + " | " + 6;
        }
        csv.AddRow(r6_l);

        string csvString = csv.GetCSVStrings();

        //Debug.Log(csvString);

        return csv;

    }

    void ExportCSVToDevice(string csvString)
    {
        DateTime dateTime = DateTime.Now;
        var filename = "export_" + dateTime.ToString("yyyyMMddHHmmss");
#if !UNITY_EDITOR && UNITY_WEBGL
        //iphone
        Debug.Log("Downloading..." + filename);
        CSVDownloader(csvString, filename);
#else
        string parentPath = Application.persistentDataPath + "/ExportData";
        if (!Directory.Exists(parentPath))
        {
            Directory.CreateDirectory(parentPath);
        }
        string path = parentPath + "/" + filename + ".csv";
        System.IO.File.WriteAllText(path, csvString);
        Debug.Log(path);
#endif
    }
}

//[Serializable]
//public class PlayerScoreInfo
//{
//    public string id;
//    public string name;
//    public string email;
//    public List<PlayerScoreProgress> testProgress;
//    public List<PlayerScoreProgress> evalProgress_pre;
//    public List<PlayerScoreProgress> evalProgress_post;
//}
//
//[Serializable]
//public class PlayerScoreProgress
//{
//    public int progress;
//    public int total;
//}