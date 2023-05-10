using System.Collections;
using System.Collections.Generic;
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
    public RectTransform taskPanel;
    public SimpleProgressbar mainProgressbar;

    public List<Achievement> achievements = new List<Achievement>();
    public List<AchievementTask> achievementTasks = new List<AchievementTask>();

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

        var mainVal = (float)completeCount / (float)achievements.Count;

        mainProgressbar.SetValue(mainVal);
        mainProgressbar.SetText((mainVal*100f).ToString()+"%");

        if (completeCount >= achievements.Count)
        {
            frame_normal.alpha = 0;
            frame_complete.alpha = 1;
            badgeIcon_complete.GetComponent<CanvasGroup>().alpha = 1;

            complete = true;
        }
        Resize();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Resize(bool jumptolast = false)
    {
        StartCoroutine(_resize(jumptolast));
    }
    IEnumerator _resize(bool jumptolast)
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
        if (jumptolast)
        {
            taskPanel.anchoredPosition = new Vector2(
                taskPanel.anchoredPosition.x,
                0
                );
        }
    }
}
