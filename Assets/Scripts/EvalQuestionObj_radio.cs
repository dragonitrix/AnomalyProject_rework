using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvalQuestionObj_radio : MonoBehaviour
{
    public TextMeshProUGUI title;
    public ToggleGroup toggleGroup;
    public List<Toggle> toggles = new List<Toggle>();

    public void InitEval()
    {

    }

    public void SetQuestion(string text)
    {
        title.text = text;
    }

    public int GetResult()
    {
        if (!toggleGroup.AnyTogglesOn())
        {
            return -1;
        }

        var result = 0;

        for (int i = 0; i < toggles.Count; i++)
        {
            if (toggles[i].isOn)
            {
                result = i; 
                break;
            }
        }
        return result;
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
