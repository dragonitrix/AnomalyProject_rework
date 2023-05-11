using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
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

    public AchievementGroup achievementGroup;

    public void UpdateAchievement_Test(Dimension dimension)
    {
        DatabaseManagerMongo.instance.FetchPlayerAnswer((int)dimension,
            (data) =>
            {
                var count = data.Count;
                string id_5 = "", id_10 = "", id_20 = "";
                switch (dimension)
                {
                    case Dimension._D1:
                        id_5 = "p1WouAKD84A5bRGM5QE8bY";
                        id_10 = "xwr6oYqaNpHMXbNEJK3GxL";
                        id_20 = "vXaqjEk7QFpdVBfMy1aLUY";
                        break;
                    case Dimension._D2:
                        id_5 = "vC6wrwR87A4xvYczU51A5z";
                        id_10 = "g6sE1k5mE8r6qXyQ8tCiW9";
                        id_20 = "27FCSgXGeCdCerxfMe7B6E";
                        break;
                    case Dimension._D3:
                        id_5 = "32m9rYLtG2NJjXqUiTLNxz";
                        id_10 = "kEirQjHoCDXq5LGC7JuEY9";
                        id_20 = "gtbtQyr71iDAJDhHrzM5DF";
                        break;
                    case Dimension._D4:
                        id_5 = "9thTJHEKqtoGoPGWRJ3X7Q";
                        id_10 = "j7HHQ64qECHrjQRMXQLReV";
                        id_20 = "hcdka5YRmBNjF5nyXiqvsH";
                        break;
                    case Dimension._D5:
                        id_5 = "pCG1qboCKbcv3tpDvBpJT1";
                        id_10 = "fCphonkEFxK4BLtrqvNBB9";
                        id_20 = "f4vLQkB46AoEEWpaNsYGQj";
                        break;
                    case Dimension._D6:
                        id_5 = "8CuPQHRaV7B2ozcPoCDqRc";
                        id_10 = "18JhvW91yotqgSDaAQsAHw";
                        id_20 = "cCd1jgv4z9hSp1wYsP7vKN";
                        break;
                }
                UpdateProgress(id_5, count);
                UpdateProgress(id_10, count);
                UpdateProgress(id_20, count);
            });
    }

    public void UpdateProgress(string id, int val)
    {
        UpdateProgress(id, val, () => { });
    }

    public void UpdateProgress(string id, int val, System.Action callback)
    {
        if (id == "" || id == null)
        {
            Debug.Log("invalid id");
            return;
        }

        //Debug.Log("ValidateTestCount: " + id);

        // read current progress
        var progress = GetProgress(id);
        var achievement = GetArchivement(id);
        if (progress == null) // no progress
        {
            progress = new AchievementProgress(PlayerInfoManager.instance.currentPlayerId, id, 0);
        }

        progress.currentProgress = val;
        if (progress.currentProgress > achievement.goal) progress.currentProgress = achievement.goal;

        DatabaseManagerMongo.instance.UpdateAchievementProgress(progress, (data) =>
        {
            callback();
        });
    }

    public void UpdateAchievement_Eval(Dimension dimension, bool isPre, System.Action callback)
    {
        var pre_id = "";
        var post_id = "";

        switch (dimension)
        {
            case Dimension._D1:
                pre_id = "3BXZb61PhNiGhJqxnuz6ma";
                post_id = "v1WhCtUqKGodYwA3aMC5rQ";
                break;
            case Dimension._D2:
                pre_id = "p9nd8S1XTiNP4YyY7j32JU";
                post_id = "9Tqy7K2AT5XTVpo23u46HG";
                break;
            case Dimension._D3:
                pre_id = "iXd2wBuMZkAeSgDMhjQfDJ";
                post_id = "8bh8HiC9Y14ydzUsGCm2tK";
                break;
            case Dimension._D4:
                pre_id = "86WBzqu8VEnkZqqAPsDDRF";
                post_id = "uc6E2MWHvL1THT8rHF32u2";
                break;
            case Dimension._D5:
                pre_id = "95vbtkaonLpFLeVVA6vebz";
                post_id = "oXZccnGbJyotAuV1HVAW7s";
                break;
            case Dimension._D6:
                pre_id = "4F35qzx2Qtt9seh9CcAo6t";
                post_id = "2jVu3qgd39Qkxf13Xpm3iN";
                break;
        }

        var id = isPre ? pre_id : post_id;
        UpdateProgress(id, 1, callback);

    }

    public void UpdateAchievement_Achievement(System.Action callback)
    {
        for (int i = 0; i < 6; i++)
        {
            var dimension = (Dimension)(i + 1);
            UpdateAchievement_Achievement(dimension, i == 6 - 1 ? callback : () => { });
        }
    }

    public void UpdateAchievement_Achievement(Dimension dimension, System.Action callback)
    {
        var achievements = GetArchivementsByDimension(dimension);
        var progresses = GetProgressesByDimension(dimension);
        var completeCount = 0;

        for (int i = 0; i < progresses.Count; i++)
        {
            var progress = progresses[i];
            var achievement = GetArchivement(progress.achievementID);

            if (progress.currentProgress == achievement.goal)
            {
                completeCount++;
            }
        }

        var id_3 = "";
        var id_full = "";

        switch (dimension)
        {
            case Dimension._D1:
                id_3 = "cTDbQhDgWkj7h2gcfZtef9";
                id_full = "v4dm6Xb3y3NhtbgzHLeSW5";
                break;
            case Dimension._D2:
                id_3 = "jNssMEKuqzhZGintMVEK8W";
                id_full = "miPHJVokQyXk5CGEGJdiTs";
                break;
            case Dimension._D3:
                id_3 = "qHUB3TSdBfZV2SaBrgNryS";
                id_full = "3jApozX5xu6sBSUNvFCpeP";
                break;
            case Dimension._D4:
                id_3 = "nzMATJoAdcaxVFWYzfTYpY";
                id_full = "1cJuwy1D91Mfzd3RG6Pb7p";
                break;
            case Dimension._D5:
                id_3 = "naPsHPxccAM9LjTB9U9k5y";
                id_full = "j5eEGa4pUsFwyE1epUuwsY";
                break;
            case Dimension._D6:
                id_3 = "jp6tWsNCtDi8VEtPToKqeb";
                id_full = "cz2B3w1uQbFXixH5MWYArW";
                break;
            default:
                break;
        }

        UpdateProgress(id_3, completeCount, callback);

        if (completeCount >= achievements.Count - 1)
        {
            UpdateProgress(id_full, 1, callback);
        }

    }


    public void UpdateAchievement_Mission(Dimension dimension, bool isOver, bool isPerfect)
    {

        var id_finished = "";
        var id_finished_normal = "";
        var id_finished_perfect = "";


        switch (dimension)
        {
            case Dimension._D1:
                id_finished = "3dpTiiCnZ9RR6nzXntk2DB";
                id_finished_normal = "gGuove6vaWsiKmercmJzxf";
                id_finished_perfect = "9x2EfuRJhYfT6phTZwDbSS";
                break;
            case Dimension._D2:
                id_finished = "jjbxNCgdZPWaPxYPQC5o35";
                id_finished_normal = "8ge2gDyDwUtEYfCJ4u9btn";
                id_finished_perfect = "7V9Aj8KM3HMPoXsp2LrGjh";
                break;
            case Dimension._D3:
                id_finished = "gieagNQB8xxYhNWHds85vd";
                id_finished_normal = "hxSjKQBAfgGM98vHqJ5EWe";
                id_finished_perfect = "bF3ca6QTti9JpF5gdQXYGp";
                break;
            case Dimension._D4:
                id_finished = "rPaTur4X2bmo8hb5zvuBNq";
                id_finished_normal = "4MVHyAbgjVGUcoZ8wFaVTM";
                id_finished_perfect = "m1j5VhQLPQ6jQYeri7PPYB";
                break;
            case Dimension._D5:
                id_finished = "dzyw463KZujvbkhTF273kJ";
                id_finished_normal = "uL5miRSqF6TFn7nYCFjz6q";
                id_finished_perfect = "5y6ULqs81KBJvpGh7LthWp";
                break;
            case Dimension._D6:
                id_finished = "skBgLH3ohXQV8A8Pkmn1oi";
                id_finished_normal = "cDdrvpPezzSfFYGv3vD1z4";
                id_finished_perfect = "f3Ewv6DwJNqYzkChQstKo1";
                break;
            default:
                break;
        }

        UpdateProgress(id_finished, 1);

        if (!isOver) UpdateProgress(id_finished_normal, 1);

        if (isPerfect) UpdateProgress(id_finished_perfect, 1);


    }

    public List<Achievement> GetArchivementsByDimension(Dimension dimension)
    {
        var achievements = new List<Achievement>();
        for (int i = 0; i < achievementGroup.achievements.Count; i++)
        {
            if (achievementGroup.achievements[i].dimension == dimension)
            {
                achievements.Add(achievementGroup.achievements[i]);
            }
        }
        return achievements;
    }
    public List<AchievementProgress> GetProgressesByDimension(Dimension dimension)
    {
        var progresses = new List<AchievementProgress>();
        for (int i = 0; i < PlayerInfoManager.instance.achievementProgresses.Count; i++)
        {
            var progress = PlayerInfoManager.instance.achievementProgresses[i];
            var achievement = GetArchivement(progress.achievementID);
            if (achievement.dimension == dimension)
            {
                progresses.Add(progress);
            }
        }
        return progresses;
    }

    public Achievement GetArchivement(string id)
    {
        return achievementGroup.achievements.Find((x) => x.id == id);
    }
    public AchievementProgress GetProgress(string id)
    {
        return PlayerInfoManager.instance.GetAchievementProgress(id);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
