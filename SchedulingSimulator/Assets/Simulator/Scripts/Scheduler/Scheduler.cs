using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All the algorithms are implemented in this scheduler
/// </summary>
public class Scheduler
{
    public enum Scheduling { FIRST_COME_FIRST_SERVED, SHORTEST_JOB_FIRST_PREEMPTIVE, SHORTEST_JOB_FIRST_NON_PREEMTIVE, ROUND_ROBIN };

    public static int SlotID;

    private Transform spawnPoint;
    private Process[] processes;
    private List<Process> FIFO;
    private Process runningProcess;
    private readonly Scheduling scheduling;
    private readonly double quantum;
    private int indexRR;
    private float timeInRR;
    private const float SPACING = 1.24f;

    public Scheduler(Scheduling scheduling, int slots, double quantum, Transform spawnPoint)
    {
        this.scheduling = scheduling;
        this.quantum = quantum;
        this.spawnPoint = spawnPoint;

        processes = new Process[slots];
        FIFO = new List<Process>();
        runningProcess = null;
        indexRR = -1;
        timeInRR = 0f;
        SlotID = 0;
    }

    public Process[] GetProcesses()
    {
        return processes;
    }

    /// <summary>
    /// Add a new process to treat
    /// </summary>
    /// <param name="process">New process to add</param>
    /// <param name="index">Index of the new process in the list</param>
    /// <returns></returns>
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

    /// <summary>
    /// Check if all slots of the scheduler are empty
    /// </summary>
    /// <returns>true if the scheduler is done</returns>
    public bool IsDone()
    {
        for (int i = 0; i < processes.Length; i++)
        {
            if (processes[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Remove a specific process from the scheduler's slots
    /// </summary>
    private void RemoveRunningProcess()
    {
        for (int i = 0; i < processes.Length; i++)
        {
            if (processes[i] == runningProcess)
            {
                processes[i] = null;
            }
        }
        runningProcess = null;
    }

    /// <summary>
    /// Main loop of the scheduler
    /// </summary>
    /// <param name="timePassed">Time passed between this frame and the previous one</param>
    public void Run(float timePassed)
    {
        // Find next candidate
        AttributeProcess();
        
        // Check when a process falls on the consumer and set it to ready if it's the case
        for (int i = 0; i < processes.Length; i++)
        {
            if (processes[i] != null)
            {
                processes[i].WatchOut();
            }
        }
        
        // Consume running process
        if (runningProcess != null)
        {
            runningProcess.Consume(timePassed);

            if (runningProcess.GetState() == Process.State.Terminated)
            {
                RemoveRunningProcess();
                timeInRR = (float) quantum;
            }
        }
    }

    /// <summary>
    /// Find the slot of a process
    /// </summary>
    /// <param name="process">Process to analyse</param>
    /// <returns>the slot id for a specific process</returns>
    public int SlotOfProcess(Process process)
    {
        for (int i = 0; i < processes.Length; i++)
        {
            if (processes[i] == process)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Method to find the next candidate to run
    /// </summary>
    private void AttributeProcess()
    {
        // Create a new list with potential candidates (Ready or Running state)
        List<Process> readyProcesses = new List<Process>();
        for (int i = 0; i < processes.Length; i++)
        {
            if (processes[i] != null && (processes[i].GetState() == Process.State.Ready || processes[i].GetState() == Process.State.Running))
            {
                readyProcesses.Add(processes[i]);
            }
        }

        // Find the candidate based on the scheduling algorithm
        Process process = null;
        float best = float.MaxValue;
        int selectedProcessSlot = -1;
        switch (scheduling)
        {
            case Scheduling.FIRST_COME_FIRST_SERVED:
                if (runningProcess == null)
                {
                    // Youngest process
                    for (int i = 0; i < readyProcesses.Count; i++)
                    {
                        if (readyProcesses[i] != null && readyProcesses[i].Arrival < best)
                        {
                            process = readyProcesses[i];
                            best = process.Arrival;
                        }
                    }

                    // Update slot
                    selectedProcessSlot = SlotOfProcess(process);
                    if (selectedProcessSlot >= 0) SlotID = selectedProcessSlot;

                    // Apply new candidate
                    if (process != null && process != runningProcess)
                    {
                        if (runningProcess != null)
                        {
                            runningProcess.Reset();
                        }
                        runningProcess = process;
                    }
                }
                break;
            case Scheduling.SHORTEST_JOB_FIRST_NON_PREEMTIVE:
                if (runningProcess == null)
                {
                    // Smallest process
                    for (int i = 0; i < readyProcesses.Count; i++)
                    {
                        if (readyProcesses[i] != null && readyProcesses[i].Progress < best)
                        {
                            process = readyProcesses[i];
                            best = process.Progress;
                        }
                    }

                    // Update slot
                    selectedProcessSlot = SlotOfProcess(process);
                    if (selectedProcessSlot >= 0) SlotID = selectedProcessSlot;

                    // Apply candidate
                    if (process != null)
                    {
                        if (runningProcess != null)
                        {
                            runningProcess.Reset();
                        }
                        runningProcess = process;
                    }
                }
                break;
            case Scheduling.SHORTEST_JOB_FIRST_PREEMPTIVE:
                // Smallest process
                for (int i = 0; i < readyProcesses.Count; i++)
                {
                    if (readyProcesses[i] != null && readyProcesses[i].Progress < best)
                    {
                        process = readyProcesses[i];
                        best = process.Progress;
                    }
                }

                // Update slot
                selectedProcessSlot = SlotOfProcess(process);
                if (selectedProcessSlot >= 0) SlotID = selectedProcessSlot;

                // Apply candidate
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
                // Find first process if first Round Robin call
                if (indexRR == -1)
                {
                    runningProcess = FindNextProcessInRR();
                }
                else
                {
                    // Increase quantum time used
                    timeInRR += Time.deltaTime;
                    if (timeInRR > quantum)
                    {
                        // Update candidate and restart quantum timer
                        Process processToReset = runningProcess;
                        runningProcess = FindNextProcessInRR();
                        if (processToReset != null)
                        {
                            processToReset.Reset();
                        }
                        timeInRR = 0;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Helper method to find next candidate for Round Robin scheduling
    /// </summary>
    /// <returns>The next candidate for Round Robin</returns>
    private Process FindNextProcessInRR()
    {
        // Shift along the list to find a candidate
        int numberOfShifts = 0;
        do
        {
            indexRR++;
            numberOfShifts++;

            // Loop in the list
            if (indexRR >= processes.Length)
            {
                indexRR = 0;
            }

            // Candidate condition
            if (processes[indexRR] != null && (processes[indexRR].GetState() == Process.State.Ready || processes[indexRR].GetState() == Process.State.Running))
            {
                SlotID = indexRR;
                return processes[indexRR];
            }
        } while (numberOfShifts < processes.Length);
        
        // Empty list
        return null;
    }
}
