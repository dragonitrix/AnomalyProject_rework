//using MongoDB.Bson;
//using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DatabaseManagerMongo : MonoBehaviour
{
    public static DatabaseManagerMongo instance;

    public DatabaseEndpoint m_endpoint;
    [HideInInspector]
    public string endpoint = "";

    public enum Mode
    {
        _Dev,
        _Production
    }

    public Mode mode;

    private void Awake()
    {
        if (!instance) { instance = this; DontDestroyOnLoad(this); }
        else if (instance != this) Destroy(gameObject);
    }

    public bool mock = false;
    public string _mockAccountID;


    public bool isLoggedin = false;

    public float timeoutDuration = 5f;

    // Start is called before the first frame update
    void Start()
    {
        setupEndpoint();
        if (mock)
        {
            isLoggedin = true;
            PlayerInfoManager.instance.account.id = _mockAccountID;
            PlayerInfoManager.instance.info.nickname = "kot(mock)";
        }
    }

    public void setupEndpoint()
    {
        switch (mode)
        {
            case Mode._Dev:
                endpoint = m_endpoint.dev;
                break;
            case Mode._Production:
                endpoint = m_endpoint.production;
                break;
        }
    }

    public string SendWebRequest(string uri, WWWForm form)
    {
        var result = "";
        StartCoroutine(_SendWebRequest(uri, form, (string _result) => { _result = result; }));
        return result;
    }
    IEnumerator _SendWebRequest(string uri, WWWForm form, System.Action<string> callback)
    {
        Debug.Log("_SendWebRequest");

        UnityWebRequest webRequest = UnityWebRequest.Post(uri, form);
        var handler = webRequest.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > timeoutDuration)
            {
                Debug.Log("timeout");
                callback(null);
                break;
            }
            yield return null;
        }

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            callback(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("fail");
            callback(null);
        }
    }

    public void FetchAllQuestion()
    {
        StartCoroutine(_FetchAllQuestion());
    }

    IEnumerator _FetchAllQuestion()
    {
        var uri = endpoint + "/getQuestionIDs";
        WWWForm form = new WWWForm();
        form.AddField("dimension", 1);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result == null)
        {
            Debug.Log("FAIL");
            yield break;
        }

        var idList = JsonConvert.DeserializeObject<List<string>>(result);

        Debug.Log(idList.Count + " questions found on");

        List<QuestionData> questionDatas = new List<QuestionData>();

        var qURI = endpoint + "/getQuestion";
        for (int i = 0; i < idList.Count; i++)
        {
            var id = idList[i];
            Debug.Log("id: " + id);

            WWWForm qform = new WWWForm();
            qform.AddField("id", id);
            string q = null;
            yield return _SendWebRequest(qURI, qform, (string _result) => { q = _result; });

            if (q != null)
            {
                Debug.Log(q);
                var question = JsonConvert.DeserializeObject<QuestionData>(q);
                questionDatas.Add(question);
            }
        }

        //foreach (var item in questionDatas)
        //{
        //    item.Log();
        //}

    }
}

