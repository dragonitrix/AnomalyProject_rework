using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphGridController : MonoBehaviour
{

    public GameObject line_prefab;
    public RectTransform line_group;

    public GameObject text_prefab;
    public RectTransform text_group;

    [ContextMenu("TestGrid")]
    public void TestGrid()
    {
        CreateGrid(2, 4, 2, 60);
    }

    public void CreateGrid(int col, int row, int linewidth, float height, float offset = 10)
    {
        for (int i = 0; i < col + 1; i++)
        {
            var clone = Instantiate(line_prefab, line_group);
            var line_rect = clone.GetComponent<RectTransform>();

            var posX = line_group.sizeDelta.x * ((float)i / (float)(col));
            var posY = 0f;

            line_rect.anchoredPosition = new Vector2(posX, posY);
            line_rect.sizeDelta = new Vector2(linewidth, line_group.sizeDelta.y);
        }

        for (int i = 0; i < row + 1; i++)
        {
            var clone = Instantiate(line_prefab, line_group);
            var line_rect = clone.GetComponent<RectTransform>();

            var posX = 0f;
            var posY = line_group.sizeDelta.y * ((float)i / (float)(row));

            line_rect.anchoredPosition = new Vector2(posX - offset, posY);
            line_rect.sizeDelta = new Vector2(line_group.sizeDelta.x + offset, linewidth);

            var text_clone = Instantiate(text_prefab, text_group);
            var text_rect = text_clone.GetComponent<RectTransform>();
            var text_text = text_clone.GetComponent<TextMeshProUGUI>();

            text_rect.anchoredPosition = new Vector2(posX - offset - 3, posY);
            text_text.text = Mathf.Round(height * ((float)i / (float)(row))).ToString();
        }
    }

}
