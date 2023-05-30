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
using UnityEngine.SocialPlatforms.Impl;

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

    public bool mockGroupID = false;
    public string _mockGroupID;

    public bool isLoggedin = false;

    public float timeoutDuration = 5f;

    // Start is called before the first frame update
    void Start()
    {

        setupEndpoint();
        if (mock) // fake login
        {

            PlayerInfoManager.instance.SetPlayerAccount(new PlayerAccount(
                _mockAccountID,
                "",
                "mockup@gmail.com"
                ));

            GetPlayerInfo(_mockAccountID, (data) =>
            {
                Debug.Log("mockID login success");
                isLoggedin = true;
            });

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
                    Debug.Log(uri);
                    Debug.Log("Error: " + webRequest.error);
                    callback(null);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(uri);
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


    public void Regis(string email, string password, string name, System.Action<string> callback)
    {
        StartCoroutine(_Regis(email, password, name, callback));
    }

    IEnumerator _Regis(string email, string password, string name, System.Action<string> callback)
    {
        var groupid = TryGetGroupID();

        password = SecureHelper.HashSalt(password, _passwordSalt);
        var uri = endpoint + "/register";
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("name", name);
        if (groupid != null) form.AddField("groupid", groupid);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result != null)
        {
            var tempinfo = JsonConvert.DeserializeObject<TempPlayerAccount>(result);

            PlayerInfoManager.instance.SetPlayerAccount(new PlayerAccount(
                tempinfo.id,
                tempinfo.groupid,
                tempinfo.email
                ));

            callback(tempinfo.id);
        }
        else
        {
            callback(null);
        }
    }

    public void Login(string email, string password, System.Action<string> callback)
    {
        StartCoroutine(_Login(email, password, callback));
    }

    IEnumerator _Login(string email, string password, System.Action<string> callback)
    {
        var groupid = TryGetGroupID();

        //password = SecureHelper.HashSalt(password, _passwordSalt);
        var uri = endpoint + "/login";
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        if (groupid != null) form.AddField("groupid", groupid);
        //form.AddField("password", password);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result != null)
        {
            try
            {
                var tempinfo = JsonConvert.DeserializeObject<TempPlayerAccount>(result);
                if (tempinfo.admin)
                {
                    Debug.Log("admin login");
                    Debug.Log(result);
                    PlayerInfoManager.instance.SetPlayerAccount(new PlayerAccount(
                        tempinfo.id,
                        tempinfo.groupid,
                        tempinfo.email,
                        tempinfo.admin
                        ));
                    callback(tempinfo.id);
                    tempinfo = null;
                }
                else
                {
                    //validate
                    if (ValidatePassword(password, tempinfo.password))
                    {
                        PlayerInfoManager.instance.SetPlayerAccount(new PlayerAccount(
                            tempinfo.id,
                            tempinfo.groupid,
                            tempinfo.email,
                            tempinfo.admin
                            ));
                        callback(tempinfo.id);
                        tempinfo = null;
                    }
                    else
                    {
                        callback(null);
                    }
                }
            }
            catch (Exception)
            {
                callback(null);
            }
        }
        else
        {
            callback(null);
        }
    }

    public void GetPlayerInfo(string id, System.Action<string> callback)
    {
        StartCoroutine(_GetPlayerInfo(id, callback));
    }

    IEnumerator _GetPlayerInfo(string id, System.Action<string> callback)
    {
        var uri = endpoint + "/getPlayerInfo";

        WWWForm form = new WWWForm();
        form.AddField("id", id);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });
        if (result != null)
        {
            Debug.Log(result);
            var info = JsonConvert.DeserializeObject<PlayerInfo>(result);
            PlayerInfoManager.instance.SetPlayerInfo(info);

            callback(result);

            //callback(result);
            //FetchAchievementProgress((data) =>
            //{
            //    callback(result);
            //});
        }
        else
        {
            callback(null);
        }
    }

    public void UpdatePlayerInfo(System.Action<string> callback)
    {
        StartCoroutine(_UpdatePlayerInfo(callback));
    }

    IEnumerator _UpdatePlayerInfo(System.Action<string> callback)
    {
        var uri = endpoint + "/updatePlayerInfo";

        WWWForm form = new WWWForm();
        form.AddField("playerInfo", PlayerInfoManager.instance.info.JSON());

        Debug.Log("update player info");
        Debug.Log(PlayerInfoManager.instance.info.JSON());

        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });
        if (result != null)
        {
            callback(result);
        }
        else
        {
            callback(null);
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
        form.AddField("playerID", PlayerInfoManager.instance.currentPlayerId);
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
            aform.AddField("playerID", PlayerInfoManager.instance.currentPlayerId);
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

    public void FetchPlayerEval(System.Action<List<Answer>> callback)
    {
        StartCoroutine(_FetchPlayerEval(callback));
    }

    IEnumerator _FetchPlayerEval(System.Action<List<Answer>> callback)
    {
        var uri = endpoint + "/getPlayerEvalAnswerIDs";
        WWWForm form = new WWWForm();
        form.AddField("playerID", PlayerInfoManager.instance.currentPlayerId);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result == null)
        {
            Debug.Log("FAIL");
            yield break;
        }

        var idList = JsonConvert.DeserializeObject<List<string>>(result);

        List<Answer> answers = new List<Answer>();

        var aURI = endpoint + "/getPlayerAnswer";
        for (int i = 0; i < idList.Count; i++)
        {
            var id = idList[i];
            //Debug.Log("questionID: " + id);

            WWWForm aform = new WWWForm();
            aform.AddField("playerID", PlayerInfoManager.instance.currentPlayerId);
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

        var playerID = PlayerInfoManager.instance.currentPlayerId;

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

    public void FetchEval(int dimension, System.Action<List<EvalData>> callback)
    {
        StartCoroutine(_FetchEval(dimension, callback));
    }

    IEnumerator _FetchEval(int dimension, System.Action<List<EvalData>> callback)
    {
        var uri = endpoint + "/getEvalIDs";
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

        Debug.Log(idList.Count + " evals found on dimension: " + dimension);

        List<EvalData> questionDatas = new List<EvalData>();

        var qURI = endpoint + "/getEval";
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
                var question = JsonConvert.DeserializeObject<EvalData>(q);
                questionDatas.Add(question);
            }
        }
        //foreach (var item in questionDatas)
        //{
        //    item.Log();
        //}
        callback(questionDatas);
    }


    public void FetchAllEval(System.Action<List<EvalData>> callback)
    {
        StartCoroutine(_FetchAllEval(callback));
    }

    IEnumerator _FetchAllEval(System.Action<List<EvalData>> callback)
    {
        var uri = endpoint + "/getAllEvalIDs";
        WWWForm form = new WWWForm();
        //form.AddField("dimension", "{\"$in\":[1,2,3,4,5,6]}");
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result == null)
        {
            Debug.Log("FAIL");
            yield break;
        }

        var idList = JsonConvert.DeserializeObject<List<string>>(result);

        List<EvalData> questionDatas = new List<EvalData>();

        var qURI = endpoint + "/getEval";
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
                var question = JsonConvert.DeserializeObject<EvalData>(q);
                questionDatas.Add(question);
            }
        }
        //foreach (var item in questionDatas)
        //{
        //    item.Log();
        //}
        callback(questionDatas);
    }

    public void GetPlayerEvalScore(int type, System.Action<List<float>> callback)
    {
        StartCoroutine(_GetPlayerEvalScore(type, callback));
    }

    IEnumerator _GetPlayerEvalScore(int type, System.Action<List<float>> callback)
    {
        var uri = endpoint + "/getPlayerEvalScore";
        WWWForm form = new WWWForm();
        form.AddField("playerID", PlayerInfoManager.instance.currentPlayerId);
        form.AddField("type", type);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result == null)
        {
            Debug.Log("FAIL");
            yield break;
        }

        var evalScore = JsonConvert.DeserializeObject<EvalScore>(result);

        callback(evalScore.evalScore);
    }


    public void UpdatePlayerAnswers(List<Answer> answers, System.Action<string> callback)
    {
        StartCoroutine(_UpdatePlayerAnswers(answers, callback));
    }
    IEnumerator _UpdatePlayerAnswers(List<Answer> answers, System.Action<string> callback)
    {
        var playerID = PlayerInfoManager.instance.currentPlayerId;
        var uri = endpoint + "/updatePlayerAnswer";

        for (int i = 0; i < answers.Count; i++)
        {
            var answer = answers[i];
            WWWForm form = new WWWForm();
            var json = JsonConvert.SerializeObject(answer);
            form.AddField("playerAnswer", json);

            yield return _SendWebRequest(uri, form, (data) =>
            {
                //Debug.Log(data);
            });
        }
        callback("update complete");
    }

    public void FetchAchievementProgress(System.Action<List<AchievementProgress>> callback)
    {
        StartCoroutine(_FetchAchievementProgress(callback));
    }

    IEnumerator _FetchAchievementProgress(System.Action<List<AchievementProgress>> callback)
    {
        //Debug.Log("start fetch routine");

        var uri = endpoint + "/getAchievementIDs";
        WWWForm form = new WWWForm();
        form.AddField("playerID", PlayerInfoManager.instance.currentPlayerId);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result == null)
        {
            Debug.Log("FAIL");
            yield break;
        }

        var idList = JsonConvert.DeserializeObject<List<string>>(result);

        Debug.Log(idList.Count + " achievement found");

        List<AchievementProgress> achievementProgresses = new List<AchievementProgress>();

        var qURI = endpoint + "/getAchievementProgress";
        for (int i = 0; i < idList.Count; i++)
        {
            var id = idList[i];
            //Debug.Log("id: " + id);

            WWWForm qform = new WWWForm();
            qform.AddField("playerID", PlayerInfoManager.instance.currentPlayerId);
            qform.AddField("achievementID", id);
            string q = null;
            yield return _SendWebRequest(qURI, qform, (string _result) => { q = _result; });

            if (q != null)
            {
                //Debug.Log(q);
                var achievement = JsonConvert.DeserializeObject<AchievementProgress>(q);
                achievementProgresses.Add(achievement);
            }
        }
        //foreach (var item in questionDatas)
        //{
        //    item.Log();
        //}
        PlayerInfoManager.instance.achievementProgresses.Clear();
        PlayerInfoManager.instance.achievementProgresses = achievementProgresses.ToList();
        //Debug.Log("finished fetch");
        callback(achievementProgresses);
    }

    public void UpdateAchievementProgress(AchievementProgress achievementProgress, System.Action<string> callback)
    {
        StartCoroutine(_UpdateAchievementProgress(achievementProgress, callback));
    }
    IEnumerator _UpdateAchievementProgress(AchievementProgress achievementProgress, System.Action<string> callback)
    {
        var playerID = PlayerInfoManager.instance.currentPlayerId;
        var uri = endpoint + "/updateAchievementProgress";

        WWWForm form = new WWWForm();
        var json = JsonConvert.SerializeObject(achievementProgress);
        form.AddField("achievement", json);

        yield return _SendWebRequest(uri, form, (data) =>
        {
            //Debug.Log(data);
        });

        callback("update complete");

        //callback("update complete");
    }


    public void FetchPlayerScoreInfos(string groupid, System.Action<List<PlayerScoreInfo>> callback)
    {
        StartCoroutine(_FetchPlayerScoreInfos(groupid, callback));
    }
    IEnumerator _FetchPlayerScoreInfos(string groupid, System.Action<List<PlayerScoreInfo>> callback)
    {
        var uri = endpoint + "/getPlayerIDs";
        WWWForm form = new WWWForm();
        form.AddField("groupid", groupid);
        string result = null;
        yield return _SendWebRequest(uri, form, (string _result) => { result = _result; });

        if (result == null)
        {
            Debug.Log("FAIL");
            yield break;
        }

        //Debug.Log(result);

        var idList = JsonConvert.DeserializeObject<List<string>>(result);

        Debug.Log(idList.Count + " id found");

        List<PlayerScoreInfo> scoreInfos = new List<PlayerScoreInfo>();

        var qURI = endpoint + "/getPlayerScoreInfo";
        for (int i = 0; i < idList.Count; i++)
        {
            var id = idList[i];
            WWWForm qform = new WWWForm();
            qform.AddField("playerid", id);
            string q = null;
            yield return _SendWebRequest(qURI, qform, (string _result) => { q = _result; });

            if (q != null)
            {
                //Debug.Log(q);
                var scoreInfo = JsonConvert.DeserializeObject<PlayerScoreInfo>(q);
                scoreInfos.Add(scoreInfo);
            }
            else
            {
                Debug.Log("something wrong");
            }
        }
        callback(scoreInfos);
    }

    private static string _passwordSalt = "anmly";

    private bool ValidatePassword(string _password, string _passwordHash)
    {
        var passwordHash = SecureHelper.HashSalt(_password, _passwordSalt);

        return passwordHash == _passwordHash;
    }

    public string TryGetGroupID()
    {
        if (mockGroupID)
        {
            return _mockGroupID;
        }

        try
        {
            var groupid = URLParameters.GetSearchParameters()["groupid"];
            return groupid;
        }
        catch (Exception)
        {
            return null;
        }
    }

}

[Serializable]
public class EvalScore
{
    public List<float> evalScore = new List<float>();
}

[Serializable]
public class TempPlayerAccount
{
    public string id;
    public string email;
    public string password;
    public string groupid;
    public bool admin = false;
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