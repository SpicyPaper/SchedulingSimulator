using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameObject processPrefab;
    public Scheduler.Scheduling scheduling;
    public int slots;
    public float quantum;
    public float speed;

    private float timePassed;
    private Scheduler scheduler;
    private List<Process> processes;
    private bool firstTime;
    private GameObject processesObjects;

    // Start is called before the first frame update
    void Start()
    {
        scheduler = new Scheduler(scheduling, slots, quantum, SpawnPoint);
        processes = new List<Process>();

        processesObjects = new GameObject
        {
            name = "Processes"
        };

        firstTime = true;

        GenerateComplexProcesses();
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
        processes.Add(new Process(processPrefab, "P1", 0.0f, 7.0f));
        processes.Add(new Process(processPrefab, "P2", 2.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P3", 4.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P4", 5.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P5", 6.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P6", 8.0f, 8.0f));
        processes.Add(new Process(processPrefab, "P7", 11.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P8", 12.0f, 2.0f));
        processes.Add(new Process(processPrefab, "P9", 13.0f, 12.0f));
        processes.Add(new Process(processPrefab, "P10", 13.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P11", 15.0f, 2.0f));
        processes.Add(new Process(processPrefab, "P12", 18.0f, 6.0f));
        processes.Add(new Process(processPrefab, "P13", 19.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P14", 20.0f, 5.0f));
        processes.Add(new Process(processPrefab, "P15", 22.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P16", 25.0f, 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = speed * Time.deltaTime;
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
