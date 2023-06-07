using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/CSVTemplate")]
public class CSVTemplate : ScriptableObject
{
    [SerializeField]
    List<ListWrapper> datas = new List<ListWrapper>();

    public List<List<string>> GetDatas()
    {
        List<List<string>> strings = new List<List<string>>();

        for (int i = 0; i < datas.Count; i++)
        {
            strings.Add(datas[i].myList);
        }

        return strings;
    }

    public int GetCol()
    {
        return datas[0].myList.Count;
    }


}

[Serializable]
public class ListWrapper
{
    [TextArea(2, 2)]
    public List<string> myList;
}