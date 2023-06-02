using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GraphPieceController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform mainRect;
    public RectTransform displayRect;
    public RectTransform bgRect;

    public CanvasGroup infoCanvasGroup;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    public float rawValue;
    public float height;

    public void SetHeight(float rawValue, float width, float height,float maxHeight)
    {
        this.rawValue = rawValue;
        this.height = height;

        mainRect.sizeDelta = new Vector2(width, maxHeight);
        bgRect.sizeDelta = new Vector2(width, maxHeight);

        displayRect.sizeDelta = new Vector2(width, height);

        SetInfoText("", "");
    }

    public void SetColor(Color color)
    {
        displayRect.GetComponent<Image>().color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoCanvasGroup.ShowAll();
        var rt = infoCanvasGroup.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(mainRect.sizeDelta.x / 2, 0);
        bgRect.GetComponent<CanvasGroup>().alpha = 0.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoCanvasGroup.HideAll();
        bgRect.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void SetInfoText(string t1,string t2)
    {
        text1.text = t1;
        text2.text = t2;
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
