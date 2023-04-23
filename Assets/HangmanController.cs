using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HangmanController : MonoBehaviour
{

    public HangmanPool hangmanTextPool;

    HangmanText hangmanText;

    public TextMeshProUGUI hintMesh;

    public RectTransform charGroup;
    public RectTransform keyGroup;

    public GameObject char_prefab;
    public GameObject key_prefab;

    public CanvasGroup resultPanel;
    public CanvasGroup correctPanel;
    public CanvasGroup incorrectPanel;

    Dictionary<string, HangmanKey> keys = new Dictionary<string, HangmanKey>();
    List<HangmanChar> chars = new List<HangmanChar>();

    public delegate void OnHangmanGameFinished();
    public OnHangmanGameFinished onHangmanGameFinished = () => { };

    int correctCount = 0;
    string nonDupe = "";
    public int health = 7;

    public HealthBar healthBar;
    public void InitHangmanGame()
    {
        //pick word

        hangmanText = hangmanTextPool.hangmanTexts[UnityEngine.Random.Range(0, hangmanTextPool.hangmanTexts.Count)];

        InitKeypad();
        InitWord();

        healthBar.InitHealthBar(health, health);

    }

    void InitWord()
    {
        hintMesh.text = hangmanText.hint;

        var word = hangmanText.word;
        nonDupe = "";
        for (int i = 0; i < word.Length; i++)
        {
            if (!nonDupe.Contains(word[i]))
            {
                nonDupe += word[i];
            }

            var clone = Instantiate(char_prefab, charGroup);
            var charScript = clone.GetComponent<HangmanChar>();
            charScript.InitChar(word[i].ToString());
            chars.Add(charScript);
        }
    }

    void InitKeypad()
    {
        for (int i = 65; i < 91; i++)
        {
            var character = ((char)i).ToString();
            var clone = Instantiate(key_prefab, keyGroup);
            var key = clone.GetComponent<HangmanKey>();

            key.InitKey(character,this);

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
        ShowResult(result);
    }

    public void UpdateHealthBar()
    {
        healthBar.SetHealth(health);
    }

    public void OnResultClicked()
    {
        resultPanel.HideAll();
        onHangmanGameFinished();
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
    [TextArea(5,10)]
    public string hint;
}