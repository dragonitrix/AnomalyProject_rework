using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimplePopup : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    [SerializeField]
    private Button confirmBtn;
    [SerializeField]
    private Button cancleBtn;

    public delegate void OnConfirmDelegate();
    public OnConfirmDelegate onConfirm = () => { };

    public delegate void OnCancleDelegate();
    public OnConfirmDelegate onCancle = () => { };

    public bool hideOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        confirmBtn.onClick.AddListener(OnConfirmClick);
        cancleBtn.onClick.AddListener(OnCancleClick);

        if (hideOnStart)
        {
            Hide();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnConfirmClick()
    {
        onConfirm();
        Hide();
    }

    public void OnCancleClick()
    {
        onCancle();
        Hide();
    }

    public void Show()
    {
        canvasGroup.ShowAll();
    }
    public void Hide()
    {
        canvasGroup.HideAll();
    }

}
