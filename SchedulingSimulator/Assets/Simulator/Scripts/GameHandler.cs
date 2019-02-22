using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public GameObject processPrefab;
    public Scheduler.Scheduling scheduling;
    public int slots;

    private float timePassed;
    private Scheduler scheduler;
    private List<Process> processes;
    private bool firstTime;
    private GameObject processesObjects;

    // Start is called before the first frame update
    void Start()
    {
        scheduler = new Scheduler(scheduling, slots);
        processes = new List<Process>();

        processesObjects = new GameObject
        {
            name = "Processes"
        };

        firstTime = true;

        GenerateSimpleProcesses();
    }

    private void GenerateSimpleProcesses()
    {
        processes.Add(new Process(processPrefab, "P1", 0.0f, 7.0f));
        processes.Add(new Process(processPrefab, "P2", 2.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P3", 4.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P4", 5.0f, 4.0f));
    }

    private void GenerateComplexProcesses()
    {
        processes.Add(new Process(processPrefab, "P1", 0.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P2", 0.4f, 0.3f));
        processes.Add(new Process(processPrefab, "P3", 0.8f, 0.5f));
        processes.Add(new Process(processPrefab, "P4", 1.2f, 1.0f));
        processes.Add(new Process(processPrefab, "P5", 1.6f, 0.2f));
        processes.Add(new Process(processPrefab, "P6", 2.0f, 0.4f));
        processes.Add(new Process(processPrefab, "P7", 2.4f, 0.2f));
        processes.Add(new Process(processPrefab, "P8", 2.8f, 0.2f));
        processes.Add(new Process(processPrefab, "P9", 3.2f, 0.6f));
        processes.Add(new Process(processPrefab, "P10", 3.6f, 0.7f));
        processes.Add(new Process(processPrefab, "P11", 4.0f, 1.4f));
        processes.Add(new Process(processPrefab, "P12", 4.4f, 0.3f));
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        if (firstTime)
        {
            firstTime = false;
        }
        else timePassed += deltaTime;

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

        scheduler.Run(deltaTime);
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
