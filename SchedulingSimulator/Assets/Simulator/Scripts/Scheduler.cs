using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler
{
    public enum Scheduling { FIRST_COME_FIRST_SERVED, SHORTEST_JOB_FIRST_PREEMPTIVE, SHORTEST_JOB_FIRST_NON_PREEMTIVE, ROUND_ROBIN };

    private Transform spawnPoint;
    private Process[] processes;
    private List<Process> FIFO;
    private Process runningProcess;
    private readonly Scheduling scheduling;
    private readonly double quantum;
    private int indexRR;
    private float timeInRR;
    private int counter;
    private const float SPACING = 1.24f;

    public Scheduler(Scheduling scheduling, int slots, double quantum, Transform spawnPoint)
    {
        this.scheduling = scheduling;
        this.quantum = quantum;
        this.spawnPoint = spawnPoint;

        processes = new Process[slots];
        FIFO = new List<Process>();
        runningProcess = null;
        indexRR = 0;
        timeInRR = 0f;
        counter = 0;
    }

    public Process[] GetProcesses()
    {
        return processes;
    }

    public bool AddProcess(Process process, int index)
    {
        bool available = processes[index] == null;
        if (available)
        {
            process.Admit();
            process.Place(spawnPoint.position + Vector3.right * SPACING * index);
            processes[index] = process;
        }

        return available;
    }

    private void RemoveRunningProcess()
    {
        for (int i = 0; i < processes.Length; i++)
        {
            if (processes[i] == runningProcess)
            {
                processes[i] = null;
                break;
            }
        }
        runningProcess = null;
    }

    private void DrawFIFO()
    {
        counter++;
        string fifo = "|";
        for (int i = 0; i < FIFO.Count; i++)
        {
            fifo += FIFO[i].Name + "|";
        }
        Debug.Log(counter + ": " + fifo);
        string pointer = "";
        for (int i = 0; i < indexRR; i++)
        {
            pointer += "     ";
        }
        pointer += " ^";
        Debug.Log(counter + ": " + pointer);
        Debug.Log(counter + ": " + indexRR);
    }

    public void Run(float timePassed)
    {
        AttributeProcess();

        // Check when a process falls on the consumer and set it to ready if it's the case
        for (int i = 0; i < processes.Length; i++)
        {
            if (processes[i] != null)
            {
                processes[i].WatchOut();
            }
        }

        if (runningProcess != null)
        {
            runningProcess.Consume(timePassed);

            if (runningProcess.GetState() == Process.State.Terminated)
            {
                RemoveRunningProcess();
            }
        }
    }

    private void AttributeProcess()
    {
        List<Process> readyProcesses = new List<Process>();
        for(int i = 0; i < processes.Length; i++)
        {
            if (processes[i] != null && (processes[i].GetState() == Process.State.Ready || processes[i].GetState() == Process.State.Running))
            {
                readyProcesses.Add(processes[i]);
            }
        }

        Process process = null;
        float best = float.MaxValue;
        switch (scheduling)
        {
            case Scheduling.FIRST_COME_FIRST_SERVED:
                if (runningProcess == null)
                {
                    for (int i = 0; i < readyProcesses.Count; i++)
                    {
                        if (readyProcesses[i] != null && readyProcesses[i].Arrival < best)
                        {
                            process = readyProcesses[i];
                            best = process.Arrival;
                        }
                    }
                    if (process != null && process != runningProcess)
                    {
                        runningProcess = process;
                    }
                }
                break;
            case Scheduling.SHORTEST_JOB_FIRST_NON_PREEMTIVE:
                if (runningProcess == null)
                {
                    for (int i = 0; i < readyProcesses.Count; i++)
                    {
                        if (readyProcesses[i] != null && readyProcesses[i].Progress < best)
                        {
                            process = readyProcesses[i];
                            best = process.Progress;
                        }
                    }
                    if (process != null)
                    {
                        runningProcess = process;
                    }
                }
                break;
            case Scheduling.SHORTEST_JOB_FIRST_PREEMPTIVE:
                for (int i = 0; i < readyProcesses.Count; i++)
                {
                    if (readyProcesses[i] != null && readyProcesses[i].Progress < best)
                    {
                        process = readyProcesses[i];
                        best = process.Progress;
                    }
                }
                if (process != null)
                {
                    if (runningProcess != null)
                    {
                        runningProcess.Reset();
                    }
                    runningProcess = process;
                }
                break;
            case Scheduling.ROUND_ROBIN:
                break;
        }
    }

    /*private void x()
    { 
        switch (scheduling)
        {
            case Scheduling.FIRST_COME_FIRST_SERVED:
                Process firstProcess = null;
                float firstArrival = float.MaxValue;
                for (int i = 0; i < processes.Length; i++)
                {
                    if (processes[i] != null && processes[i].Arrival < firstArrival)
                    {
                        firstProcess = processes[i];
                        firstArrival = firstProcess.Arrival;
                    }
                }
                if (firstProcess != null)
                {
                    runningProcess = firstProcess;
                    runningProcess.Start();
                }
                break;
            case Scheduling.SHORTEST_JOB_FIRST_NON_PREEMTIVE:
                Process smallerProcess = null;
                float smallerDuration = float.MaxValue;
                for (int i = 0; i < processes.Length; i++)
                {
                    if (processes[i] != null && processes[i].Duration < smallerDuration)
                    {
                        smallerProcess = processes[i];
                        smallerDuration = smallerProcess.Duration;
                    }
                }
                if (smallerProcess != null)
                {
                    runningProcess = smallerProcess;
                    runningProcess.Start();
                }
                break;
            case Scheduling.SHORTEST_JOB_FIRST_PREEMPTIVE:
                Process progressProcess = null;
                float smallerProgression = float.MaxValue;
                for (int i = 0; i < processes.Length; i++)
                {
                    if (processes[i] != null && processes[i].Progress < smallerProgression)
                    {
                        progressProcess = processes[i];
                        smallerProgression = progressProcess.Progress;
                    }
                }
                if (progressProcess != null)
                {
                    if (runningProcess != null)
                    {
                        runningProcess.Interrupt();
                    }
                    runningProcess = progressProcess;
                    runningProcess.Start();
                }
                break;
            case Scheduling.ROUND_ROBIN:
                if (FIFO.Count > 0 && indexRR >= 0 && indexRR < FIFO.Count)
                {
                    runningProcess = FIFO[indexRR];
                }
                break;
        }
    }*/

}




// RUN()
/*if (runningProcess == null && processes.Length > 0)
{
    AttributeProcess();
} 
else
{
    //DrawFIFO();
    runningProcess.Consume(timePassed);
    bool attribute = false;

    if (scheduling == Scheduling.ROUND_ROBIN)
    {
        timeInRR += timePassed;
        if (timeInRR >= quantum)
        {
            timeInRR = 0f;
            int swap = 0;
            do {
                indexRR++;
                if (indexRR >= FIFO.Count)
                {
                    indexRR = 0;
                }
                swap++;
            } while ((processes[indexRR] == null || processes[indexRR].GetState() == Process.State.New) && swap < processes.Length);
            attribute = true;
        }
    }

    if (runningProcess.GetState() == Process.State.Terminated)
    {
        RemoveRunningProcess();
    }

    if (attribute)
    {
        AttributeProcess();
    }
}*/
