using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvalPool : MonoBehaviour
{
    public static EvalPool instance;
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
    public Dimension currentDimension;
    public List<EvalData> evals = new List<EvalData>();
    public bool redirectToMission = false;
}
