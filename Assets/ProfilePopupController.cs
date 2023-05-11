using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ProfilePopupController : MonoBehaviour
{
    List<EvalData> evalDatas = new List<EvalData>();
    List<Answer> playerEval = new List<Answer>();

    public WebGraphController webGraph;

    public CanvasGroup mainCanvasGroup;
    public CanvasGroup contentCanvasGroup;
    public CanvasGroup overlayCanvasGroup;

    public enum GraphType
    {
        PRE_TEST = 2,
        POST_TEST
    }

    public Color PreTestColor;
    public Color PostTestColor;

    public GraphType currentType;

    [Header("text")]
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI email_text;

    bool isGettingGraphValue = false;

    [ContextMenu("GetGraphValue")]
    public void GetGraphValue(GraphType graphType, bool forced = false)
    {
        if (currentType == graphType && !forced) return;
        if (isGettingGraphValue) return;
        currentType = graphType;
        isGettingGraphValue = true;
        overlayCanvasGroup.ShowAll();

        // type: 2=pre test, 3=post test
        DatabaseManagerMongo.instance.GetPlayerEvalScore((int)graphType, (data) =>
        {
            switch (graphType)
            {
                case GraphType.PRE_TEST:
                    webGraph.SetColor(PreTestColor);
                    break;
                case GraphType.POST_TEST:
                    webGraph.SetColor(PostTestColor);
                    break;
            }

            webGraph.SetShapeValue(data);
            overlayCanvasGroup.HideAll();
            contentCanvasGroup.ShowAll();
            isGettingGraphValue = false;
        });
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Show()
    {
        name_text.text = PlayerInfoManager.instance.info.nickname;
        email_text.text = PlayerInfoManager.instance.account.email;

        GetGraphValue(GraphType.PRE_TEST, true); // forced
        mainCanvasGroup.ShowAll();
        contentCanvasGroup.HideAll();
    }
    public void Hide()
    {
        mainCanvasGroup.HideAll();
    }

    public void OnPreClick()
    {
        GetGraphValue(GraphType.PRE_TEST);
    }
    public void OnPostClick()
    {
        GetGraphValue(GraphType.POST_TEST);
    }

}
