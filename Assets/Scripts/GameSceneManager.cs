using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public Animator transitionAnim;

    public float transtitionTime = 1.0f;

    public bool transitionOnLoaded = true;

    public CanvasGroup transitionCanvasGroup;

    private void OnLevelWasLoaded(int level)
    {
        if (transitionOnLoaded)
        {
            transitionAnim.SetTrigger("Out");
        }
    }

    private void Start()
    {
        if (transitionOnLoaded)
        {
            transitionAnim.SetTrigger("Out");
        }
        
    }

    public void NextScene()
    {
        JumptoScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void JumptoScene(int index)
    {
        StartCoroutine(_JumptoScene(index));
    }

    public void JumptoScene(string name)
    {
        StartCoroutine(_JumptoScene(name));
    }

    IEnumerator _JumptoScene(int index)
    {
        transitionAnim.SetTrigger("In");
        yield return new WaitForSeconds(transtitionTime);
        SceneManager.LoadScene(index);
    }

    IEnumerator _JumptoScene(string name)
    {
        transitionAnim.SetTrigger("In");
        yield return new WaitForSeconds(transtitionTime);
        SceneManager.LoadScene(name);
    }

    public void LoadSceneAdditive(string name)
    {
        StartCoroutine(_LoadSceneAdditive(name));
    }

    IEnumerator _LoadSceneAdditive(string name)
    {
        transitionAnim.SetTrigger("In");
        yield return new WaitForSeconds(transtitionTime);
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    }

    public void UnloadScene(string name, Action callback)
    {
        StartCoroutine(_UnloadScene(name, callback));
    }
    IEnumerator _UnloadScene(string name,Action callback)
    {
        transitionAnim.SetTrigger("Out");
        yield return new WaitForSeconds(transtitionTime);
        SceneManager.UnloadSceneAsync(name);
        callback();
    }

}
