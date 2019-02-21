using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler
{
    public enum Scheduling { FCFS, SJFP, SJFNP, RR };

    private Process[] processes;
    private Scheduling scheduling;

    public Scheduler(Scheduling scheduling, int slots)
    {
        this.scheduling = scheduling;
        processes = new Process[slots];
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

    public void Run()
    {
        switch (scheduling)
        {
            case Scheduling.FCFS:
            break;
        }

    }

}
