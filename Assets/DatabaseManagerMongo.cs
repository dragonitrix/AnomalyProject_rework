//using MongoDB.Bson;
//using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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

    public delegate void onRegisSuccessDelegate();
    public onRegisSuccessDelegate onRegisSuccessCallback;

    public delegate void onLoginSuccessDelegate();
    public onLoginSuccessDelegate onLoginSuccessCallback;

    public delegate void onRegisFailDelegate();
    public onRegisFailDelegate onRegisFailCallback;

    public delegate void onLoginFailDelegate();
    public onLoginFailDelegate onLoginFailCallback;


    public void Register(string email, string password, string name)
    {
        Debug.Log("start register");
        GameManager.instance.ShowLoadOverlay();
        StartCoroutine(_RegisterCoroutine(email, password, name));
    }

    IEnumerator _RegisterCoroutine(string email, string password, string name)
    {
        password = SecureHelper.HashSalt(password, _passwordSalt);

        //string query = $"?email={email}&password={password}&name={name}";

        var uri = endpoint + "/register";

        //Debug.Log(uri);

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("name", name);

        UnityWebRequest webRequest = UnityWebRequest.Post(uri, form);

        var handler = webRequest.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > timeoutDuration)
            {
                break;
            }
            yield return null;
        }

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            //RawAccount account;
            try
            {
                //Debug.Log(webRequest.downloadHandler.text);
                //account = JsonConvert.DeserializeObject<RawAccount>(webRequest.downloadHandler.text);
                PlayerInfoManager.instance.account.id = webRequest.downloadHandler.text;
                PlayerInfoManager.instance.account.email = email;

                PlayerInfoManager.instance.info.nickname = name;

                Debug.Log("regis success id: " + PlayerInfoManager.instance.account.id);
                OnRegisSuccess();
            }
            catch (Exception)
            {
                OnRegisFail();
                yield break;
            }
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            OnRegisFail();
        }

    }

    public void Login(string email, string password)
    {
        GameManager.instance.ShowLoadOverlay();
        StartCoroutine(LoginCoroutine(email, password));
    }

    IEnumerator LoginCoroutine(string email, string password)
    {
        //string query = $"?email={email}&password={password}";

        var uri = endpoint + "/login";

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest webRequest = UnityWebRequest.Post(uri, form);

        var handler = webRequest.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > timeoutDuration)
            {
                break;
            }
            yield return null;
        }

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(webRequest.downloadHandler.text);

            RawAccount account = new RawAccount();
            try
            {
                account = JsonConvert.DeserializeObject<RawAccount>(webRequest.downloadHandler.text);
            }
            catch (Exception)
            {
                Debug.Log("DeserializeObject failled");
                OnLoginFail();
                yield break;
            }

            if (ValidatePassword(password, account.password))
            {
                PlayerInfoManager.instance.account.id = account.id;
                Debug.Log("login success");
                StartCoroutine(RetrivePlayerData(account.id, OnLoginSuccess));
                //OnLoginSuccess();
            }
            else
            {
                Debug.Log("validation failled");
                OnLoginFail();
            }
        }
        else
        {
            //Debug.Log(webRequest.downloadHandler.text);
            OnLoginFail();
        }
    }

    IEnumerator RetrivePlayerData(string id, UnityAction callback)
    {
        Debug.Log("start RetrivePlayerData");

        float startTime;

        //string query = $"?email={email}";
        var uri = endpoint + "/getPlayerInfo";

        WWWForm form = new WWWForm();
        form.AddField("id", id);

        UnityWebRequest webRequest = UnityWebRequest.Post(uri, form);

        var handler = webRequest.SendWebRequest();

        startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > timeoutDuration)
            {
                break;
            }
            yield return null;
        }

        //Debug.Log(webRequest.downloadHandler.text);

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            try
            {
                PlayerInfo info = new PlayerInfo();
                info = JsonConvert.DeserializeObject<PlayerInfo>(webRequest.downloadHandler.text);
                PlayerInfoManager.instance.info = info;
                callback();
            }
            catch (Exception)
            {
                Debug.Log("DeserializeObject fail");
            }
        }
        else
        {

            Debug.Log("request data fail");
        }


    }

    public void OnRegisSuccess()
    {
        Debug.Log("regis success");
        GameManager.instance.HideLoadOverlay();
        isLoggedin = true;
        onRegisSuccessCallback();
    }
    public void OnRegisFail()
    {
        Debug.Log("regis fail");
        GameManager.instance.HideLoadOverlay();
        onRegisFailCallback();
    }
    public void OnLoginSuccess()
    {
        Debug.Log("login success");
        GameManager.instance.HideLoadOverlay();
        isLoggedin = true;
        onLoginSuccessCallback();
    }
    public void OnLoginFail()
    {
        Debug.Log("login fail");
        GameManager.instance.HideLoadOverlay();
        onLoginFailCallback();
    }

    private static string _passwordSalt = "anmly";

    private bool ValidatePassword(string _password, string _passwordHash)
    {
        var passwordHash = SecureHelper.HashSalt(_password, _passwordSalt);

        return passwordHash == _passwordHash;
    }


    public void RequestPoolID(int dimension, UnityAction callback)
    {
        StartCoroutine(RequestPoolIDCoroutine(dimension, callback));
    }

    IEnumerator RequestPoolIDCoroutine(int dimension, UnityAction callback)
    {

        Debug.Log("start RequestPoolIDCoroutine: " + dimension);

        //string query = $"?email={email}";
        var uri = endpoint + "/getQuestionID";

        WWWForm form = new WWWForm();
        form.AddField("dimension", dimension);

        UnityWebRequest webRequest = UnityWebRequest.Post(uri, form);

        var handler = webRequest.SendWebRequest();

        float startTime;
        startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > timeoutDuration)
            {
                break;
            }
            yield return null;
        }

        //Debug.Log(webRequest.downloadHandler.text);

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            List<string> q_ids = new List<string>();
            try
            {
                //count = Int32.Parse(webRequest.downloadHandler.text);

                q_ids = JsonConvert.DeserializeObject<List<string>>(webRequest.downloadHandler.text);
                //QuestionManager.instance.questionPool_IDs = q_ids;
            }
            catch (Exception)
            {
                Debug.Log("DeserializeObject fail");
            }

            if (q_ids.Count > 0)
            {
                var uri2 = endpoint + "/getQuestion";

                for (int i = 0; i < q_ids.Count; i++)
                {

                    WWWForm form2 = new WWWForm();
                    //form2.AddField("dimension", dimension);
                    form2.AddField("id", q_ids[i]);

                    UnityWebRequest webRequest2 = UnityWebRequest.Post(uri2, form2);

                    var handler2 = webRequest2.SendWebRequest();

                    startTime = 0.0f;
                    while (!handler2.isDone)
                    {
                        startTime += Time.deltaTime;
                        if (startTime > timeoutDuration)
                        {
                            break;
                        }
                        yield return null;
                    }

                    if (webRequest2.result == UnityWebRequest.Result.Success)
                    {
                        try
                        {
                            QuestionData questionData = JsonConvert.DeserializeObject<QuestionData>(webRequest2.downloadHandler.text);

                            //Debug.Log($"question {questionData.id} deserial success");

                            //QuestionManager.instance.questionPool.Add(questionData);
                        }
                        catch (Exception)
                        {
                            Debug.Log("question deserial fail");
                        }

                    }
                    else
                    {
                        Debug.Log("request fail");
                        yield break;
                    }
                }

                callback();

            }
            else
            {
                Debug.Log("no q_dis");
            }
        }
        else
        {
            Debug.Log("request data fail");
        }
    }

    public void UpdatePlayerAnswer(Answer answer)
    {
        StartCoroutine(UpdatePlayerAnswerCoroutine(answer));
    }

    IEnumerator UpdatePlayerAnswerCoroutine(Answer answer)
    {

        var uri = endpoint + "/UpdatePlayerAnswer";

        //Debug.Log(uri);

        WWWForm form = new WWWForm();
        form.AddField("id", PlayerInfoManager.instance.CurrentPlayerId);
        form.AddField("answer", JsonConvert.SerializeObject(answer));

        UnityWebRequest webRequest = UnityWebRequest.Post(uri, form);

        var handler = webRequest.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > timeoutDuration)
            {
                break;
            }
            yield return null;
        }

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("update answer success");
        }
        else
        {
            //Debug.Log("update answer fail");
        }

    }

    public delegate void FetchPlayerScoreDelegate(List<SimpleScore> simpleScores);
    public FetchPlayerScoreDelegate OnFetchPlayerScoreComplete;

    public void FetchPlayerScore()
    {
        StartCoroutine(FetchPlayerScoreCoroutine());
    }

    IEnumerator FetchPlayerScoreCoroutine()
    {

        GameManager.instance.ShowLoadOverlay();

        var uri = endpoint + "/fetchPlayerScore";

        //Debug.Log(uri);

        WWWForm form = new WWWForm();
        form.AddField("id", PlayerInfoManager.instance.CurrentPlayerId);

        UnityWebRequest webRequest = UnityWebRequest.Post(uri, form);

        var handler = webRequest.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > timeoutDuration)
            {
                break;
            }
            yield return null;
        }

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            try
            {
                List<SimpleScore> simpleScores = JsonConvert.DeserializeObject<List<SimpleScore>>(webRequest.downloadHandler.text);
                OnFetchPlayerScoreComplete(simpleScores);
                GameManager.instance.HideLoadOverlay();
                OnFetchPlayerScoreComplete = (List<SimpleScore> simpleScores) => { };
            }
            catch (Exception)
            {
                Debug.Log("desereallize fail");
            }
        }
        else
        {
            Debug.Log("request fail");
        }

    }
}



[Serializable]
public class RawAccount
{
    public string id;
    public string email;
    public string password;
}

[Serializable]
public class PlayerData
{
    public PlayerInfo playerInfo;
    public PlayerScore playerScore;
}

[Serializable]
public class SimpleScore
{
    public int correct;
    public int total;
}