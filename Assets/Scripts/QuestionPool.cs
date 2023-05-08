using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPool : MonoBehaviour
{
    public static QuestionPool instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public List<QuestionData> questions_unanswered = new List<QuestionData>();
    public List<QuestionData> questions_all = new List<QuestionData>();

}
