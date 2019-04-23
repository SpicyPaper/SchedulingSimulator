using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log
{
    private readonly Statistics stats;
    private readonly Scheduler.Scheduling schedulingType;
    private readonly float duration;
    private readonly bool interrupted;
    private readonly float speed;
    private readonly int seed;
    private readonly string path;
    private System.DateTime dateTime;

    public Log(Statistics stats, Scheduler.Scheduling schedulingType, float duration, bool interrupted, float speed, int seed, string path)
    {
        this.stats = stats;
        this.schedulingType = schedulingType;
        this.duration = duration;
        this.interrupted = interrupted;
        this.speed = speed;
        this.seed = seed;
        this.path = path;
        dateTime = System.DateTime.Now;
    }

    public string GetSchedulingName()
    {
        switch (schedulingType)
        {
            case Scheduler.Scheduling.FIRST_COME_FIRST_SERVED:
                return "First come, first served";
            case Scheduler.Scheduling.ROUND_ROBIN:
                return "Round Robin";
            case Scheduler.Scheduling.SHORTEST_JOB_FIRST_NON_PREEMTIVE:
                return "Shortest job first (non preemptive)";
            case Scheduler.Scheduling.SHORTEST_JOB_FIRST_PREEMPTIVE:
                return "Shortest job first (preemptive)";
        }
        return "?";
    }

    public string GetDateAndTime()
    {
        string date = dateTime.Day + "." + dateTime.Month + "." + dateTime.Year;
        string time = dateTime.Hour + ":" + dateTime.Minute;
        return date + " (" + time + ")";
    }

    public string GetTime()
    {
        return dateTime.ToLongTimeString();
    }

    public string GetDuration()
    {
        return duration.ToString("F2") + " seconds";
    }

    public string GetEndState()
    {
        if (interrupted)
        {
            return "Simulation got interrupted";
        }
        else
        {
            return "Simulation finished correctly";
        }
    }

    public string GetAverage()
    {
        return stats.AverageWaitingTime() + " seconds";
    }

    public string GetAverageScore()
    {
        return stats.AverageWaitingTimeScore(duration);
    }

    public string GetMax()
    {
        return stats.MaxWaitingTime() + " seconds";
    }

    public string GetMaxScore()
    {
        return stats.MaxWaitingTimeScore(duration);
    }

    public string GetFinalScore()
    {
        return stats.FinalScore();
    }

    public string GetSpeed()
    {
        return "x" + speed;
    }

    public string GetDatasource()
    {
        if (path != "")
        {
            return "JSON file (" + path + ")";
        }
        else
        {
            if (seed == 0)
            {
                return "Default seed (0)";
            }
            else
            {
                return "Seed (" + seed + ")";
            }
        }
    }

    public Color GetAverageScoreColor()
    {
        return stats.AverageScoreColor();
    }

    public Color GetMaxScoreColor()
    {
        return stats.MaxScoreColor();
    }

    public Color GetFinalScoreColor()
    {
        return stats.FinalScoreColor();
    }

}

