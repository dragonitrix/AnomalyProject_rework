using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObject/DatabaseEndpoint")]
[Serializable]
public class DatabaseEndpoint : ScriptableObject
{
    public string dev;
    public string production;
}
