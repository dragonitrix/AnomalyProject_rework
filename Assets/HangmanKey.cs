using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangmanKey : MonoBehaviour
{
    public string character;
    public CanvasGroup canvasGroup;
    public Button button;
    public TextMeshProUGUI textMesh;
    public HangmanController hangmanController;

    public bool interectable = true;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    public void InitKey(string character, HangmanController hangmanController)
    {
        this.hangmanController = hangmanController;
        this.character = character.ToUpper();
        textMesh.text = this.character;
    }

    public void OnClick()
    {
        hangmanController.OnKeypadClick(character);
    }

    public void DisableKey()
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.interactable = false;
    }

}
