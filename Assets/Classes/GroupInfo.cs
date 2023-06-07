using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class GroupInfo 
{
    public string groupID;
    public string adminUser;
    public string link;
    [SerializeField]
    public DateTime classStartTime;
    public int studentCount;

    public void Log()
    {
        //var localTime = new DateTimeOffset(classStartTime);
        //var otherTime = localTime.ToOffset(TimeSpan.Zero);
        //
        //TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        //DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(classStartTime, cstZone);

        Debug.Log("groupID: " + groupID);
        Debug.Log("adminUser: " + adminUser);
        Debug.Log("link: " + link);
        Debug.Log("classStartTime: " + classStartTime);
        Debug.Log("studentCount: " + studentCount);


    }

}
