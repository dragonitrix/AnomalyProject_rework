using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementTask : MonoBehaviour
{
    public CanvasGroup iconFull;
    public SimpleProgressbar progressbar;
    public TextMeshProUGUI description;

    public void InitAchievementTask(string description, int val_current, int val_max)
    {
        this.description.text = description;
        progressbar.SetValue((float)val_current / (float)val_max);
        progressbar.SetText($"{val_current} / {val_max}");

        if (val_current == val_max)
        {
            iconFull.alpha = 1f;
        }
    }

}
