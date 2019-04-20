using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics
{
    private readonly List<Process> processes;
    private readonly float startTime;
    private readonly float simulationSpeed;

    public Statistics(List<Process> processes, float simulationSpeed)
    {
        this.processes = processes;
        this.simulationSpeed = simulationSpeed;
        startTime = Time.time;
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
}
