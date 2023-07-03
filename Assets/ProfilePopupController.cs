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

    public CanvasGroup editCanvasGroup;

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
    public TextMeshProUGUI fullname_text;
    public TextMeshProUGUI faculty_text;
    public TextMeshProUGUI uni_text;

    public TMP_InputField name_inputField;
    public TMP_InputField fullname_inputField;
    public TMP_InputField faculty_inputField;
    public TMP_InputField uni_inputField;

    public List<TextMeshProUGUI> graph_text;

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

            //set text alpha
            for (int i = 0; i < data.Count; i++)
            {
                graph_text[i].alpha = data[i] != 0f ? 1f : 0.2f;
            }

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
        UpdateProfileText();

        GetGraphValue(GraphType.POST_TEST, true); // forced
        mainCanvasGroup.ShowAll();
        contentCanvasGroup.HideAll();
        editCanvasGroup.HideAll();
    }
    public void Hide()
    {
        mainCanvasGroup.HideAll();
        editCanvasGroup.HideAll();
    }

    public void OnPreClick()
    {
        GetGraphValue(GraphType.PRE_TEST);
    }
    public void OnPostClick()
    {
        GetGraphValue(GraphType.POST_TEST);
    }



    public void OnEditClick()
    {
        name_inputField.text = PlayerInfoManager.instance.info.nickname;
        fullname_inputField.text = PlayerInfoManager.instance.info.fullname;
        faculty_inputField.text = PlayerInfoManager.instance.info.faculty;
        uni_inputField.text = PlayerInfoManager.instance.info.uni;

        editCanvasGroup.ShowAll();
    }
    public void OnCloseEditClick()
    {
        editCanvasGroup.HideAll();
    }

    public void OnEditSubmitClick()
    {
        PlayerInfoManager.instance.info.nickname = name_inputField.text;
        PlayerInfoManager.instance.info.fullname = fullname_inputField.text;
        PlayerInfoManager.instance.info.faculty = faculty_inputField.text;
        PlayerInfoManager.instance.info.uni = uni_inputField.text;

        DatabaseManagerMongo.instance.UpdatePlayerInfo((data) =>
        {
            UpdateProfileText();
            editCanvasGroup.HideAll();
        });
    }

    public void UpdateProfileText()
    {
        name_text.text = "" + PlayerInfoManager.instance.info.nickname;
        email_text.text = "" + PlayerInfoManager.instance.account.email;
        fullname_text.text= "fullname: " + PlayerInfoManager.instance.info.fullname;
        faculty_text.text= "faculty: " + PlayerInfoManager.instance.info.faculty;
        uni_text.text= "university: " + PlayerInfoManager.instance.info.uni;

    }

}
