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
        if (processes[index] == null)
        {
            process.Admit();
            process.Place(spawnPoint.position + Vector3.right * SPACING * index);
            processes[index] = process;

            if (scheduling == Scheduling.SHORTEST_JOB_FIRST_PREEMPTIVE)
            {   
                AttributeProcess();
            }
            if (scheduling == Scheduling.ROUND_ROBIN)
            {
                FIFO.Add(process);
            }
            return true;
        }
        return false;
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
        if (scheduling == Scheduling.ROUND_ROBIN)
        {
            indexRR--;
            if (indexRR < 0)
            {
                indexRR = 0;
            }
            timeInRR = 0;
            FIFO.Remove(runningProcess);
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
        if (runningProcess == null && processes.Length > 0)
        {
            AttributeProcess();
        } 
        else
        {
            //DrawFIFO();
            float overTime = runningProcess.Consume(timePassed);
            bool attribute = false;

            if (scheduling == Scheduling.ROUND_ROBIN)
            {
                timeInRR += timePassed;
                if (timeInRR >= quantum)
                {
                    timeInRR = 0f;
                    indexRR++;
                    if (indexRR >= FIFO.Count)
                    {
                        indexRR = 0;
                    }
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
        }

    }

    private void AttributeProcess()
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
    }

}
