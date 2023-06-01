using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangmanController : MonoBehaviour
{

    public HangmanPool hangmanTextPool;

    HangmanText hangmanText;

    public TextMeshProUGUI hintMesh;

    public RectTransform charGroup;
    public RectTransform keyGroup;

    public GameObject char_prefab;
    public GameObject key_prefab;

    public CanvasGroup mainPanel;
    public CanvasGroup resultPanel;
    public CanvasGroup correctPanel;
    public CanvasGroup incorrectPanel;

    Dictionary<string, HangmanKey> keys = new Dictionary<string, HangmanKey>();
    List<HangmanChar> chars = new List<HangmanChar>();

    public delegate void OnHangmanGameFinished(bool result);
    public OnHangmanGameFinished onHangmanGameFinished = (result) => { };

    public int correctCount = 0;
    public string nonDupe = "";
    public int health = 7;

    public HealthBar healthBar;

    public int firstHint = 1;

    bool result;

    public string word;

    public void InitHangmanGame()
    {
        //pick word

        InitKeypad();

        healthBar.InitHealthBar(health, health);

        InitWord();

        mainPanel.HideAll();

    }



    public void StartHangmanGame()
    {
        mainPanel.ShowAll();
    }

    public void Hint()
    {
        //Debug.Log("hint");
        //hint
        var hintchar = nonDupe[UnityEngine.Random.Range(0, nonDupe.Length)].ToString();
        if (!keys[hintchar].canvasGroup.interactable)
        {
            //Debug.Log("dupe hint?");
            Hint();
            return;
        }
        OnKeypadClick(hintchar);
    }

    [ContextMenu("InitWord")]
    void InitWord()
    {
        //reset keys
        foreach (var key in keys.Values)
        {
            key.EnableKey();
        }

        correctCount = 0;

        foreach (Transform item in charGroup)
        {
            Destroy(item.gameObject);
        }

        //random word
        hangmanText = hangmanTextPool.hangmanTexts[UnityEngine.Random.Range(0, hangmanTextPool.hangmanTexts.Count)];

        hintMesh.text = hangmanText.hint;

        this.word = hangmanText.word;
        var word = hangmanText.word;

        if (word.Contains(' '))
        {
            Debug.Log("space detected");
        }

        var wordgroup = word.Split(' ');

        for (int i = 0; i < wordgroup.Length; i++)
        {
            var currentWord = wordgroup[i];
            var row = Instantiate(new GameObject("row"), charGroup);

            var rect = row.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(charGroup.sizeDelta.x, 70f);

            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 5f;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;

            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;

            for (int j = 0; j < currentWord.Length; j++)
            {
                var clone = Instantiate(char_prefab, rect);
                var charScript = clone.GetComponent<HangmanChar>();
                charScript.InitChar(currentWord[j].ToString());
                chars.Add(charScript);
            }
        }

        nonDupe = "";
        for (int i = 0; i < word.Length; i++)
        {
            if (!nonDupe.Contains(word[i]) && word[i] != ' ')
            {
                nonDupe += word[i];
            }
        }


        for (int i = 0; i < firstHint; i++)
        {
            Hint();
        }

    }

    void InitKeypad()
    {
        for (int i = 65; i < 91; i++)
        {
            var character = ((char)i).ToString();
            var clone = Instantiate(key_prefab, keyGroup);
            var key = clone.GetComponent<HangmanKey>();

            key.InitKey(character, this);

            keys.Add(character, key);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //InitKeypad();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnKeypadClick(string character)
    {
        var result = nonDupe.Contains(character);

        if (result) // correct
        {
            foreach (var item in chars)
            {
                if (item.character == character) item.ShowText();
            }

            //Debug.Log("correct");
            correctCount++;

            if (correctCount == nonDupe.Length)
            {
                //Debug.Log("FINISHED");
                Gameover(result);
            }
        }
        else
        {
            //Debug.Log("wrong");
            health--;
            UpdateHealthBar();
            if (health <= 0)
            {
                Gameover(result);
            }
        }
        keys[character].DisableKey();
    }

    public void Gameover(bool result)
    {
        foreach (var character in keys)
        {
            character.Value.canvasGroup.interactable = false;
        }
        this.result = result;
        ShowResult(result);
    }

    public void UpdateHealthBar()
    {
        healthBar.SetHealth(health);
    }

    public void OnResultClicked()
    {
        resultPanel.HideAll();
        onHangmanGameFinished(this.result);
    }

    public void ShowResult(bool result)
    {
        resultPanel.ShowAll();
        if (result)
        {
            correctPanel.alpha = 1;
        }
        else
        {
            incorrectPanel.alpha = 1;
        }
    }

}

[Serializable]
public class HangmanText
{
    public string word;
    [TextArea(5, 10)]
    public string hint;
}