/// <summary>
/// Serves as a holder for a log
/// </summary>
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

    /// <summary>
    /// Get the name of a scheduling type in a readable format
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Get the date and time
    /// </summary>
    /// <returns></returns>
    public string GetDateAndTime()
    {
        string date = dateTime.Day + "." + dateTime.Month + "." + dateTime.Year;
        string time = dateTime.ToShortTimeString();
        return date + " (" + time + ")";
    }

    /// <summary>
    /// Return the time in a long format (HH:MM:SS)
    /// </summary>
    /// <returns></returns>
    public string GetTime()
    {
        return dateTime.ToLongTimeString();
    }

    /// <summary>
    /// Get the duration of the simulation
    /// </summary>
    /// <returns></returns>
    public string GetDuration()
    {
        return duration.ToString("F2") + " seconds";
    }

    /// <summary>
    /// Get a string indicating whether or not the simulation has been interrupted
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Get the average waiting time
    /// </summary>
    /// <returns></returns>
    public string GetAverage()
    {
        return stats.AverageWaitingTime() + " seconds";
    }

    /// <summary>
    /// Get the average score
    /// </summary>
    /// <returns></returns>
    public string GetAverageScore()
    {
        return stats.AverageWaitingTimeScore(duration);
    }

    /// <summary>
    /// Get the max waiting time
    /// </summary>
    /// <returns></returns>
    public string GetMax()
    {
        return stats.MaxWaitingTime() + " seconds";
    }

    /// <summary>
    /// Get the max waiting score
    /// </summary>
    /// <returns></returns>
    public string GetMaxScore()
    {
        return stats.MaxWaitingTimeScore(duration);
    }

    /// <summary>
    /// Get the final score
    /// </summary>
    /// <returns></returns>
    public string GetFinalScore()
    {
        return stats.FinalScore();
    }

    /// <summary>
    /// Get the simulation speed
    /// </summary>
    /// <returns></returns>
    public string GetSpeed()
    {
        return "x" + speed;
    }

    /// <summary>
    /// Get the source (seed of file)
    /// </summary>
    /// <returns></returns>
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

}

