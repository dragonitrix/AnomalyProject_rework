using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public int health_max = 5;
    public int health_current = 0;

    float original_barWidth;

    public RectTransform maskTransform;

    // Start is called before the first frame update
    void Start()
    {
        original_barWidth = maskTransform.sizeDelta.x;
    }

    public void InitHealthBar(int health, int maxHealth)
    {
        health_current = health;
        health_max = maxHealth;
    }

    public void SetHealth(int health)
    {
        this.health_current = health;

        maskTransform.sizeDelta = new Vector2(
            original_barWidth * ((float)health_current / (float)health_max),
            maskTransform.sizeDelta.y
            );
    }

}
