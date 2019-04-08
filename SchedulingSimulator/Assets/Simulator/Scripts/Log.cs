using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log 
{
    public string Name;
    public Scheduler.Scheduling SchedulingType;

    public Log(string name, Scheduler.Scheduling schedulingType)
    {
        Name = name;
        SchedulingType = schedulingType;
    }

    public string GetShortenedLogDisplay()
    {
        return string.Format("{0}", SchedulingType);
    }
}
