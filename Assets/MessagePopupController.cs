using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessagePopupController : MonoBehaviour
{
    public CanvasGroup mainCanvasGroup;
    public CanvasGroup overlayCanvasGroup;

    public TMP_InputField inputField;

    public void Show()
    {
        mainCanvasGroup.ShowAll();
        inputField.text = string.Empty;
        //overlayCanvasGroup.ShowAll();
    }
    public void Hide()
    {
        mainCanvasGroup.HideAll();
    }

    public void Submit()
    {
        overlayCanvasGroup.ShowAll();
        DatabaseManagerMongo.instance.SendMessage(inputField.text, (data) => {
            Debug.Log("send message succes");
            overlayCanvasGroup.HideAll();
            Hide();
            inputField.text = string.Empty;
        });
    }


}
