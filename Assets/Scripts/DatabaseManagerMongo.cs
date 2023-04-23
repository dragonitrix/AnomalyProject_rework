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
        //Debug.Log("_SendWebRequest");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            var handler = webRequest.SendWebRequest();
            float startTime = 0.0f;
            while (!handler.isDone)
            {
                startTime += Time.deltaTime;
                if (startTime > timeoutDuration)
                {
                    Debug.Log("Timeout");
                    callback(null);
                    break;
                }
                yield return null;
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Error: " + webRequest.error);
                    callback(null);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("HTTP Error: " + webRequest.error);
                    callback(null);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                    callback(webRequest.downloadHandler.text);
                    break;
            }

        }
    }

    public void FetchAllQuestion(int dimension, System.Action<List<QuestionData>> callback)
    {
        StartCoroutine(_FetchAllQuestion(dimension, callback));
    }

    IEnumerator _FetchAllQuestion(int dimension, System.Action<List<QuestionData>> callback)
    {
        var uri = endpoint + "/getQuestionIDs";
        WWWForm form = new WWWForm();
        form.AddField("dimension", dimension);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result == null)
        {
            Debug.Log("FAIL");
            yield break;
        }

        var idList = JsonConvert.DeserializeObject<List<string>>(result);

        Debug.Log(idList.Count + " questions found on dimension: " + dimension);

        List<QuestionData> questionDatas = new List<QuestionData>();

        var qURI = endpoint + "/getQuestion";
        for (int i = 0; i < idList.Count; i++)
        {
            var id = idList[i];
            //Debug.Log("id: " + id);

            WWWForm qform = new WWWForm();
            qform.AddField("id", id);
            string q = null;
            yield return _SendWebRequest(qURI, qform, (string _result) => { q = _result; });

            if (q != null)
            {
                //Debug.Log(q);
                var question = JsonConvert.DeserializeObject<QuestionData>(q);
                questionDatas.Add(question);
            }
        }
        //foreach (var item in questionDatas)
        //{
        //    item.Log();
        //}
        callback(questionDatas);
    }

    public void FetchPlayerAnswer(int dimension, System.Action<List<Answer>> callback)
    {
        StartCoroutine(_FetchPlayerAnswer(dimension, callback));
    }

    IEnumerator _FetchPlayerAnswer(int dimension, System.Action<List<Answer>> callback)
    {
        var uri = endpoint + "/getPlayerAnswerIDs";
        WWWForm form = new WWWForm();
        form.AddField("playerID", PlayerInfoManager.instance.CurrentPlayerId);
        form.AddField("dimension", dimension);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result == null)
        {
            Debug.Log("FAIL");
            yield break;
        }

        var idList = JsonConvert.DeserializeObject<List<string>>(result);

        Debug.Log(idList.Count + " answers found on dimension: " + dimension);

        List<Answer> answers = new List<Answer>();

        var aURI = endpoint + "/getPlayerAnswer";
        for (int i = 0; i < idList.Count; i++)
        {
            var id = idList[i];
            //Debug.Log("questionID: " + id);

            WWWForm aform = new WWWForm();
            aform.AddField("playerID", PlayerInfoManager.instance.CurrentPlayerId);
            aform.AddField("questionID", id);
            string a = null;
            yield return _SendWebRequest(aURI, aform, (string _result) => { a = _result; });

            if (a != null)
            {
                //Debug.Log(a);
                var answer = JsonConvert.DeserializeObject<Answer>(a);
                answers.Add(answer);
            }
        }
        //foreach (var item in answers)
        //{
        //    item.Log();
        //}
        callback(answers);
    }

    public void FetchUnansweredQuestion(int dimension, System.Action<List<QuestionData>, List<QuestionData>> callback)
    {
        StartCoroutine(_FetchUnansweredQuestion(dimension, callback));
    }

    IEnumerator _FetchUnansweredQuestion(int dimension, System.Action<List<QuestionData>, List<QuestionData>> callback)
    {

        var playerID = PlayerInfoManager.instance.CurrentPlayerId;

        List<QuestionData> questions = new List<QuestionData>();
        List<Answer> answers = new List<Answer>();

        yield return _FetchAllQuestion(dimension, (data) => { questions = data; });
        yield return _FetchPlayerAnswer(dimension, (data) => { answers = data; });


        List<QuestionData> unanswered = new List<QuestionData>();

        for (int i = 0; i < questions.Count; i++)
        {
            var q = questions[i];

            var isAnswered = false;

            for (int j = 0; j < answers.Count; j++)
            {
                var a = answers[j];
                if (q.id == a.questionID)
                {
                    isAnswered = true;
                    break;
                }
            }
            if (!isAnswered)
            {
                unanswered.Add(q);
            }
        }

        Debug.Log("Unanswered count: " + unanswered.Count);

        //foreach (var item in unanswered)
        //{
        //    item.Log();
        //}

        callback(questions, unanswered);

    }

    public void UpdatePlayerAnswers(List<Answer> answers, System.Action<string> callback)
    {
        StartCoroutine(_UpdatePlayerAnswers(answers, callback));
    }
    IEnumerator _UpdatePlayerAnswers(List<Answer> answers, System.Action<string> callback)
    {
        var playerID = PlayerInfoManager.instance.CurrentPlayerId;
        var uri = endpoint + "/updatePlayerAnswer";

        for (int i = 0; i < answers.Count; i++)
        {
            var answer = answers[i];
            WWWForm form = new WWWForm();
            var json = JsonConvert.SerializeObject(answer);
            form.AddField("playerAnswer", json);

            yield return _SendWebRequest(uri, form, (data) => {
                Debug.Log(data);
            });
        }
        callback("update complete");
    }
}

// future implementation

//public class WaitForWebRequest : CustomYieldInstruction
//{
//    public override bool keepWaiting
//    {
//        get
//        {
//            return !Input.GetMouseButtonDown(1);
//        }
//    }
//
//    public WaitForWebRequest()
//    {
//        Debug.Log("Waiting for Mouse right button down");
//    }
//}
//
//// Implementation of WaitWhile yield instruction. This can be later used as:
//// yield return new WaitWhile(() => Princess.isInCastle);
//class WaitWhile1 : CustomYieldInstruction
//{
//    Func<bool> m_Predicate;
//
//    public override bool keepWaiting { get { return m_Predicate(); } }
//
//    public WaitWhile1(Func<bool> predicate) { m_Predicate = predicate; }
//}