﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameObject Plateform;
    public GameObject processPrefab;
    public Scheduler.Scheduling scheduling;
    public int slots;
    public float quantum;
    public float speed;
    public Text SimulationState;

    private float timePassed;
    private Scheduler scheduler;
    private List<Process> processes;
    private bool firstTime;
    private GameObject processesObjects;
    private AlgorithmSelection algorithmSelection;

    private bool isRunning;

    public bool IsRunning
    {
        get { return isRunning; }
        set
        {
            isRunning = value;
            if (SimulationState != null)
            {
                SimulationState.text = isRunning ? "RUNNING" : "STOPPED";
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        IsRunning = false;
        algorithmSelection = GetComponent<AlgorithmSelection>();
        scheduler = new Scheduler(scheduling, slots, quantum, SpawnPoint);
        processes = new List<Process>();

        processesObjects = new GameObject
        {
            name = "Processes"
        };

        firstTime = true;

        GeneratorRandomProcesses();
    }

    public void GeneratorRandomProcesses()
    {
        for (int i = 0; i < 16; i++)
        {
            processes.Add(new Process(processPrefab, Plateform, "P" + i.ToString(), Random.Range(0, 20), Random.Range(1, 10)));
        }
    }

    private void GenerateSimpleProcesses()
    {
        /*processes.Add(new Process(processPrefab, "P1", 0.0f, 7.0f));
        processes.Add(new Process(processPrefab, "P2", 2.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P3", 4.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P4", 5.0f, 4.0f));*/
    }

    private void GenerateComplexProcesses()
    {
        /*processes.Add(new Process(processPrefab, "P1", 0.0f, 7.0f));
        processes.Add(new Process(processPrefab, "P2", 2.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P3", 4.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P4", 5.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P5", 6.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P6", 8.0f, 8.0f));
        processes.Add(new Process(processPrefab, "P7", 11.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P8", 12.0f, 2.0f));
        processes.Add(new Process(processPrefab, "P9", 13.0f, 10.0f));
        processes.Add(new Process(processPrefab, "P10", 13.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P11", 15.0f, 2.0f));
        processes.Add(new Process(processPrefab, "P12", 18.0f, 6.0f));
        processes.Add(new Process(processPrefab, "P13", 19.0f, 4.0f));
        processes.Add(new Process(processPrefab, "P14", 20.0f, 5.0f));
        processes.Add(new Process(processPrefab, "P15", 22.0f, 1.0f));
        processes.Add(new Process(processPrefab, "P16", 25.0f, 2.0f));*/
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

            process.Update();
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

    public void StartSimulation()
    {
        Debug.Log(string.Format("Simulation {0} Started", algorithmSelection.CurrentAlgo));
        IsRunning = true;
    }

    public void StopSimulation()
    {
        if (IsRunning)
        {
            Log log = new Log(System.DateTime.Now.ToShortTimeString(), algorithmSelection.CurrentAlgo);
            GetComponent<AddObjectToList>().AddItem(log);
            Debug.Log("Simulation stopped");
            IsRunning = false;
        }
    }

    public void DisplayLog(Log log)
    {
        MiddleScreenDisplay display = GetComponent<MiddleScreenDisplay>();
        
        if (!display.IsDisplayed)
            display.Deploy();

        GetComponent<UpdateLogScreen>().SetLog(log);
    }
}
