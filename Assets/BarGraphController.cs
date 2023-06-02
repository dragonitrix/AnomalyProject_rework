using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarGraphController : MonoBehaviour
{
    public TextMeshProUGUI XText;
    public TextMeshProUGUI YText;

    public GraphGridController gridController;

    public float graphHeight;

    public List<float> datas = new List<float>();

    public GameObject bar_prefab;
    public RectTransform bar_group;
    public float bar_width;

    public float subDivision = 20;

    List<GraphPieceController> pieces = new List<GraphPieceController>();

    public void SetGraphText(string XText)
    {
        this.XText.text = XText;
    }
    public void InitGraph(float graphHeight, float bar_width, float subDivision)
    {
        this.graphHeight = graphHeight;
        this.bar_width = bar_width;
        this.subDivision = subDivision;
        InitGraph();
    }
    public void InitGraph()
    {
        gridController.CreateGrid(2, (int)(graphHeight / subDivision), 2, graphHeight);
    }

    public void SetGraphValue(List<float> datas)
    {
        this.datas.Clear();
        this.datas.AddRange(datas);
        SetGraphValue();
    }

    public void SetGraphValue()
    {
        pieces.Clear();
        for (int i = 0; i < datas.Count; i++)
        {
            var clone = Instantiate(bar_prefab, bar_group);
            var bar_height = bar_group.sizeDelta.y * ((float)datas[i] / (float)graphHeight);
            //var bar_rect = clone.GetComponent<RectTransform>();
            //bar_rect.sizeDelta = new Vector2(bar_width, bar_height);

            var controller = clone.GetComponent<GraphPieceController>();
            controller.SetHeight((float)datas[i], bar_width, bar_height, bar_group.sizeDelta.y);
            pieces.Add(controller);
        }
    }

    public void SetGraphColor(List<Color> colors)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].SetColor(colors[i]);
        }
    }

    public void SetGraphInfoText(List<List<string>> strings)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].SetInfoText(strings[i][0], strings[i][1]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
