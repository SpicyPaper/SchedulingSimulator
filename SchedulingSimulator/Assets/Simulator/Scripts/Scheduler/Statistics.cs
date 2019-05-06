using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Statistics based on the waiting times of processes
/// </summary>
public class Statistics
{
    private readonly List<Process> processes;
    private readonly float startTime;
    private readonly float simulationSpeed;

    private float averageTimeScore;
    private float maxTimeScore;
    private float finalScore;

    public Statistics(List<Process> processes, float simulationSpeed)
    {
        this.processes = processes;
        this.simulationSpeed = simulationSpeed;
        startTime = Time.time;

        averageTimeScore = 0;
        maxTimeScore = 0;
        finalScore = 0;
    }

    /// <summary>
    /// Compute average waiting time
    /// </summary>
    /// <returns>Average waiting time</returns>
    public string AverageWaitingTime()
    {
        float result = 0f;
        foreach (Process process in processes)
        {
            result += process.TimeWaited;
        }
        result /= processes.Count;
        result *= simulationSpeed;

        return result.ToString("F2");
    }

    /// <summary>
    /// Compute the score based on the average waiting time
    /// </summary>
    /// <param name="duration">Duration of the simulation</param>
    /// <returns>The score based on the average waiting time</returns>
    public string AverageWaitingTimeScore(float duration)
    {
        averageTimeScore = 0f;
        foreach (Process process in processes)
        {
            averageTimeScore += process.TimeWaited;
        }
        averageTimeScore /= processes.Count;
        averageTimeScore *= simulationSpeed;

        float ratio = averageTimeScore / duration;
        averageTimeScore = 10 * (Mathf.Pow(ratio - 1, 2));
        return averageTimeScore.ToString("F2") + " / 10";
    }

    /// <summary>
    /// Compute the maximum waiting time
    /// </summary>
    /// <returns>The maximum waiting time</returns>
    public string MaxWaitingTime()
    {
        float maxWaitingTime = float.MinValue;
        foreach (Process process in processes)
        {
            if (process.TimeWaited > maxWaitingTime)
            {
                maxWaitingTime = process.TimeWaited;
            }
        }
        maxWaitingTime *= simulationSpeed;

        return maxWaitingTime.ToString("F2");
    }

    /// <summary>
    /// Compute the score based on the maximum waiting time
    /// </summary>
    /// <param name="duration">Duration of the simulation</param>
    /// <returns>The score based on the maximum waiting time</returns>
    public string MaxWaitingTimeScore(float duration)
    {
        maxTimeScore = float.MinValue;
        foreach (Process process in processes)
        {
            if (process.TimeWaited > maxTimeScore)
            {
                maxTimeScore = process.TimeWaited;
            }
        }
        maxTimeScore *= simulationSpeed;

        float ratio = maxTimeScore / duration;
        maxTimeScore = 10 * (Mathf.Pow(ratio - 1, 2));
        return maxTimeScore.ToString("F2") + " / 10";
    }

    /// <summary>
    /// Final score based on average waiting time and maximum waiting time
    /// </summary>
    /// <returns>The final score</returns>
    public string FinalScore()
    {
        finalScore = (averageTimeScore + maxTimeScore) / 2;
        return finalScore.ToString("F2") + " / 10";
    }
}
