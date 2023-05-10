using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Achievement
{
    public string id;
    public Dimension dimension;
    [TextArea(5,10)]
    public string description;
    public int goal;
    public int weight;
}
