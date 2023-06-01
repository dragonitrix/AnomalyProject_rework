using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubBadgeController : MonoBehaviour
{
    public GameObject task_prefab;

    public Dimension dimension;
    public CanvasGroup frame_normal;
    public CanvasGroup frame_complete;
    public Image badgeIcon_normal;
    public Image badgeIcon_complete;
    public RectTransform badgeIcon_bg;
    public RectTransform badgeIcon_mask;
    public RectTransform taskPanel;
    public SimpleProgressbar mainProgressbar;
    public TextMeshProUGUI title_text;

    public List<Achievement> achievements = new List<Achievement>();
    public List<AchievementTask> achievementTasks = new List<AchievementTask>();

    [TextArea(0, 5)]
    public List<string> titles = new List<string>();

    public List<Sprite> icons_normal = new List<Sprite>();
    public List<Sprite> icons_complete = new List<Sprite>();

    public bool complete = false;

    public int completeCount = 0;

    public void SetDimension(Dimension dimension)
    {
        this.dimension = dimension;

        achievements.Clear();
        achievements.AddRange(AchievementManager.instance.GetArchivementsByDimension(dimension));

        badgeIcon_normal.sprite = icons_normal[(int)dimension - 1];
        badgeIcon_complete.sprite = icons_complete[(int)dimension - 1];
        badgeIcon_normal.SetNativeSize();
        badgeIcon_complete.SetNativeSize();

        title_text.text = titles[(int)dimension - 1];

    }

    public void Init()
    {
        achievementTasks.Clear();
        for (int i = 0; i < achievements.Count; i++)
        {
            var clone = Instantiate(task_prefab, taskPanel);
            var taskScript = clone.GetComponent<AchievementTask>();
            achievementTasks.Add(taskScript);
        }
        UpdateTask();
    }

    public void UpdateTask()
    {
        var completeCount = 0;
        for (int i = 0; i < achievementTasks.Count; i++)
        {
            //var achievementID = achievementIDs[i];
            //var achievement = AchievementManager.instance.GetArchivement(achievementID);
            var achievement = achievements[i];
            var progress = AchievementManager.instance.GetProgress(achievement.id);

            var progressVal = progress != null ? progress.currentProgress : 0;

            var taskScript = achievementTasks[i];
            taskScript.InitAchievementTask(
                achievement.description,
                progressVal,
                achievement.goal
                );

            if (progressVal >= achievement.goal) completeCount++;
        }

        this.completeCount = completeCount;

        var mainVal = Mathf.Clamp01((float)completeCount / (float)achievements.Count);

        mainProgressbar.SetValue(mainVal);
        mainProgressbar.SetText((mainVal * 100f).ToString() + "%");

        badgeIcon_mask.sizeDelta = new Vector2(badgeIcon_bg.sizeDelta.x, badgeIcon_bg.sizeDelta.y * mainVal);

        if (completeCount >= achievements.Count)
        {
            frame_normal.alpha = 0;
            frame_complete.alpha = 1;
            //badgeIcon_complete.GetComponent<CanvasGroup>().alpha = 1;

            complete = true;
        }
        Resize(true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Resize(bool jumptotop = false)
    {
        StartCoroutine(_resize(jumptotop));
    }
    IEnumerator _resize(bool jumptotop)
    {
        yield return new WaitForEndOfFrame();
        var totalY = 0f;
        for (int i = 0; i < taskPanel.childCount; i++)
        {
            totalY += taskPanel.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }

        var layoutGroup = taskPanel.GetComponent<HorizontalOrVerticalLayoutGroup>();

        totalY += layoutGroup.spacing * (taskPanel.childCount - 1) + layoutGroup.padding.top + layoutGroup.padding.bottom;

        taskPanel.sizeDelta = new Vector2(
            taskPanel.sizeDelta.x,
            totalY
        );
        if (jumptotop)
        {
            taskPanel.anchoredPosition = new Vector2(
                taskPanel.anchoredPosition.x,
                0
                );
        }
    }
}
