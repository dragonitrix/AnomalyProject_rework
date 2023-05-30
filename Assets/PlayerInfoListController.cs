using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoListController : MonoBehaviour
{

    public string playerID;

    public TextMeshProUGUI email_text;
    public List<TextMeshProUGUI> test_texts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> eval_pre_texts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> eval_post_texts = new List<TextMeshProUGUI>();

    public void SetText(PlayerScoreInfo playerScoreInfo)
    {
        email_text.text = playerScoreInfo.email;

        for (int i = 0; i < playerScoreInfo.testProgress.Count; i++)
        {
            PlayerScoreProgress p = playerScoreInfo.testProgress[i];
            var text = $"{p.progress} / {p.total}";
            test_texts[i].text = text;
        }

        for (int i = 0; i < playerScoreInfo.evalProgress_pre.Count; i++)
        {
            PlayerScoreProgress p = playerScoreInfo.evalProgress_pre[i];
            var text = $"{p.progress} / {p.total}";
            eval_pre_texts[i].text = text;
        }

        for (int i = 0; i < playerScoreInfo.evalProgress_post.Count; i++)
        {
            PlayerScoreProgress p = playerScoreInfo.evalProgress_post[i];
            var text = $"{p.progress} / {p.total}";
            eval_post_texts[i].text = text;
        }
    }

}
