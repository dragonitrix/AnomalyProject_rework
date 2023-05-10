using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField login_email;
    public TMP_InputField login_password;
    public TMP_InputField regis_email;
    public TMP_InputField regis_password;
    public TMP_InputField regis_name;

    public TextMeshProUGUI login_alert;
    public TextMeshProUGUI regis_alert;

    public CanvasGroup mainCanvasGroup;
    public CanvasGroup loginCanvasGroup;
    public CanvasGroup regisCanvasGroup;


    public EventSystem eventSystem;


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab) && eventSystem.currentSelectedGameObject)
        {
            Selectable next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {
                TMP_InputField inputfield = next.GetComponent<TMP_InputField>();
                if (inputfield != null)
                {
                    inputfield.OnPointerClick(new PointerEventData(eventSystem));
                }  //if it's an input field, also set the text caret

                eventSystem.SetSelectedGameObject(next.gameObject, new BaseEventData(eventSystem));
            }
            //else Debug.Log("next nagivation element not found");

        }
    }

    public void Login()
    {
        GameManager.instance.ShowLoadOverlay(GameManager.LoadOverlayType._SMALL);

        var email = login_email.text;
        var password = login_password.text;

        if (email == "" || password == "")
        {
            Alert("invalid input");
            return;
        }

        DatabaseManagerMongo.instance.Login(email, password, (data) =>
        {
            if (data != null)
            {
                Debug.Log(data);
                GetPlayerInfo(data);
            }
            else
            {
                Alert("an error occurred");
            }
        });

    }

    public void Regis()
    {
        GameManager.instance.ShowLoadOverlay(GameManager.LoadOverlayType._SMALL);
        var email = regis_email.text;
        var password = regis_password.text;
        var name = regis_name.text != "" ? regis_name.text : "Agent";

        if (email == "" || password == "")
        {
            Alert("invalid input");
            return;
        }

        DatabaseManagerMongo.instance.Regis(email, password, name, (data) =>
        {
            if (data != null)
            {
                Debug.Log(data);
                GetPlayerInfo(data);
            }
            else
            {
                Alert("an error occurred");
            }
        });

    }

    public void GetPlayerInfo(string id)
    {
        DatabaseManagerMongo.instance.GetPlayerInfo(id, (data) =>
        {
            OnLoginComplete();
        });
    }

    public void Alert(string text)
    {

        GameManager.instance.HideLoadOverlay(GameManager.LoadOverlayType._SMALL);

        if (regisCanvasGroup.alpha == 1)
        {
            regis_alert.text = text;
        }
        else
        {
            login_alert.text = text;
        }

        //login_alert.text = text;
        //regis_alert.text = text;
    }

    public void OnLoginComplete()
    {
        GameManager.instance.HideLoadOverlay(GameManager.LoadOverlayType._SMALL);
        GameSceneManager.instance.JumptoScene(GameSceneIndex.sc_mainmenu);
    }


    public void ToggleMain(bool val)
    {
        if (val)
        {
            mainCanvasGroup.ShowAll();
        }
        else
        {
            mainCanvasGroup.HideAll();
        }
    }

    public void ToggleLogin(bool val)
    {
        if (val)
        {
            loginCanvasGroup.ShowAll();
            regisCanvasGroup.HideAll();
        }
        else
        {
            loginCanvasGroup.HideAll();
            regisCanvasGroup.ShowAll();
        }
    }
    public void ToggleRegis(bool val)
    {
        if (val)
        {
            regisCanvasGroup.ShowAll();
            loginCanvasGroup.HideAll();
        }
        else
        {
            regisCanvasGroup.HideAll();
            loginCanvasGroup.ShowAll();
        }
    }
}
