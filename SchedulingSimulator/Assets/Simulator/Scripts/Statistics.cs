using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string FinalScore()
    {
        finalScore = (averageTimeScore + maxTimeScore) / 2;
        return finalScore.ToString("F2") + " / 10";
    }

    public Color AverageScoreColor()
    {
        return ColorScore(averageTimeScore);
    }

    public Color MaxScoreColor()
    {
        return ColorScore(maxTimeScore);
    }

    public Color FinalScoreColor()
    {
        return ColorScore(finalScore);
    }

    private Color ColorScore(float score)
    {
        if (score <= 3) return Color.red;
        if (score <= 5) return new Color(244, 152, 66);
        if (score <= 8) return Color.yellow;
        else return Color.green;
    }
}
