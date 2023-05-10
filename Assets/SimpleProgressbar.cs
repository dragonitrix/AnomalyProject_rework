using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleProgressbar : MonoBehaviour
{
    public float val = 0;

    public TextMeshProUGUI progressText;

    public RectTransform barbase;
    public RectTransform barfill;

    public void SetValue(float val)
    {
        this.val = Mathf.Clamp01(val);

        barfill.sizeDelta = new Vector2(
            barbase.sizeDelta.x * val,
            barfill.sizeDelta.y
            );
    }

    public void SetText(string text)
    {
        progressText.text = text;
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
