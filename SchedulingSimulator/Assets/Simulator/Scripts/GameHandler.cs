using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public GameObject processPrefab;
    public int slots;

    private float timePassed;
    private Scheduler scheduler;
    private List<Process> processes;
    private bool firstTime;
    private GameObject processesObjects;

    // Start is called before the first frame update
    void Start()
    {
        scheduler = new Scheduler(Scheduler.Scheduling.FCFS, slots);
        processes = new List<Process>();

        processesObjects = new GameObject
        {
            name = "Processes"
        };

        firstTime = true;

        GenerateProcesses();
    }

    private void GenerateProcesses()
    {
        processes.Add(new Process(processPrefab, "P1", 0.0f, 7.0f));
        processes.Add(new Process(processPrefab, "P2", 2.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P3", 4.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P4", 5.0f, 4.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (firstTime)
        {
            firstTime = false;
            float wastedTime = Time.deltaTime;
        }
        else timePassed += Time.deltaTime;

        for (int i = processes.Count - 1; i >= 0; i--)
        {
            Process process = processes[i];

            if (timePassed >= process.Arrival)
            {
                int nextIndex = GetNextIndex();

                if (nextIndex >= 0)
                {
                    scheduler.AddProcess(process, nextIndex);
                    processes.Remove(process);
                }
            }
        }
    }

    private int GetNextIndex()
    {
        List<int> possibilities = new List<int>();
        for (int i = 0; i < scheduler.GetProcesses().Length; i++)
        {
            if (scheduler.GetProcesses()[i] == null)
            {
                possibilities.Add(i);
            }
        }

        if (possibilities.Count > 0)
        {
            int rand = Random.Range(0, possibilities.Count - 1);
            return possibilities[rand];
        }
        else
        {
            return -1;
        }
    }
}
