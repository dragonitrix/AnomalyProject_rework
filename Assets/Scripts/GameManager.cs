using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public CanvasGroup loadOverlay;
    // Start is called before the first frame update
    void Start()
    {
        HideLoadOverlay();
    }
    public void ShowLoadOverlay()
    {
        loadOverlay.ShowAll();
    }
    public void HideLoadOverlay()
    {
        loadOverlay.HideAll();
    }

}
