﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler
{
    public enum Scheduling { FCFS, SJFP, SJFNP, RR };

    private Process[] processes;
    private Scheduling scheduling;
    private Process runningProcess;

    public Scheduler(Scheduling scheduling, int slots)
    {
        this.scheduling = scheduling;
        processes = new Process[slots];
        runningProcess = null;
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
            process.Place(new Vector3(1.5f * index, 0f, 0f));
            processes[index] = process;
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
        runningProcess = null;
    }

    public void Run(float timePassed)
    {
        if (runningProcess == null && processes.Length > 0)
        {
            AttributeProcess();
        } 
        else
        {
            float overTime = runningProcess.Consume(timePassed);
            
            if (runningProcess.GetState() == Process.State.Terminated)
            {
                RemoveRunningProcess();
            }
        }

        /*switch (scheduling)
        {
            case Scheduling.FCFS:
                break;
        }*/

    }

    private void AttributeProcess()
    {
        switch (scheduling)
        {
            case Scheduling.FCFS:
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
        }
    }

}
