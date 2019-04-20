﻿using System.Collections.Generic;
using System.Linq;
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

    private static float simulationSpeed = 1;

    public static float SimulationSpeed
    {
        get { return simulationSpeed; }
        set
        {
            simulationSpeed = value;
            Physics.gravity = initialGravity * simulationSpeed;
        }
    }

    private static Vector3 initialGravity;
    private float timePassed;
    private Scheduler scheduler;
    private List<Process> processes;
    private bool firstTime;
    private GameObject processesObjects;
    private Statistics stats;
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
        initialGravity = Physics.gravity;
        IsRunning = false;
        algorithmSelection = GetComponent<AlgorithmSelection>();
        processes = new List<Process>();

        processesObjects = new GameObject
        {
            name = "Processes"
        };

        firstTime = true;
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
        processes.Add(new Process(processPrefab, Plateform, "P1", 0.0f, 7.0f));
        processes.Add(new Process(processPrefab, Plateform, "P2", 2.0f, 4.0f));
        processes.Add(new Process(processPrefab, Plateform, "P3", 4.0f, 1.0f));
        processes.Add(new Process(processPrefab, Plateform, "P4", 5.0f, 4.0f));
    }

    private void GenerateComplexProcesses()
    {
        processes.Add(new Process(processPrefab, Plateform, "P1", 0.0f, 7.0f));
        processes.Add(new Process(processPrefab, Plateform, "P2", 2.0f, 4.0f));
        processes.Add(new Process(processPrefab, Plateform, "P3", 4.0f, 1.0f));
        processes.Add(new Process(processPrefab, Plateform, "P4", 5.0f, 4.0f));
        processes.Add(new Process(processPrefab, Plateform, "P5", 6.0f, 4.0f));
        processes.Add(new Process(processPrefab, Plateform, "P6", 8.0f, 8.0f));
        processes.Add(new Process(processPrefab, Plateform, "P7", 11.0f, 1.0f));
        processes.Add(new Process(processPrefab, Plateform, "P8", 12.0f, 2.0f));
        processes.Add(new Process(processPrefab, Plateform, "P9", 13.0f, 10.0f));
        processes.Add(new Process(processPrefab, Plateform, "P10", 13.0f, 4.0f));
        processes.Add(new Process(processPrefab, Plateform, "P11", 15.0f, 2.0f));
        processes.Add(new Process(processPrefab, Plateform, "P12", 18.0f, 6.0f));
        processes.Add(new Process(processPrefab, Plateform, "P13", 19.0f, 4.0f));
        processes.Add(new Process(processPrefab, Plateform, "P14", 20.0f, 5.0f));
        processes.Add(new Process(processPrefab, Plateform, "P15", 22.0f, 1.0f));
        processes.Add(new Process(processPrefab, Plateform, "P16", 25.0f, 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
        {

        }
        else if (processes.Count == 0 && scheduler.IsDone())
        {
            float results = stats.Results();
            Debug.Log(results + " seconds per process");
            isRunning = false;
        }
        else
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
                    process.Arrived();

                    int nextIndex = GetNextIndex();

                    if (nextIndex >= 0)
                    {
                        if (scheduler.AddProcess(process, nextIndex))
                        {
                            processes.Remove(process);
                        }
                    }
                }
            }

            scheduler.Run(deltaTime);
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

    /// <summary>
    /// Called when starting the animation with the start button
    /// </summary>
    public void StartSimulation()
    {
        IsRunning = true;
        ShowLeftScreen(false);
        scheduler = new Scheduler(algorithmSelection.CurrentAlgo, slots, quantum, SpawnPoint);

        GenerateComplexProcesses();
        stats = new Statistics(processes.Count);
    }

    /// <summary>
    /// Called when stopping the simulation with the stop button
    /// </summary>
    public void StopSimulation()
    {
        if (IsRunning)
        {
            Log log = new Log(System.DateTime.Now.ToShortTimeString(), algorithmSelection.CurrentAlgo);
            GetComponent<AddObjectToList>().AddItem(log);
            IsRunning = false;
            ShowLeftScreen(true);
            
            DestroyProcesses();
            firstTime = true;
        }
    }

    public void DestroyProcesses()
    {
        foreach (Transform child in processesObjects.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Display the log on the middle screen
    /// </summary>
    /// <param name="log">The log to display</param>
    public void DisplayLog(Log log)
    {
        ScreenDisplay display = GetComponents<ScreenDisplay>().Where(s => s.Screen == 1).ToArray().First();
        
        if (!display.IsDisplayed)
            display.Deploy();

        GetComponent<UpdateLogScreen>().SetLog(log);
    }

    /// <summary>
    /// Hide the middle screen that show the log detail
    /// </summary>
    public void HideLog()
    {
        ScreenDisplay display = GetComponents<ScreenDisplay>().Where(s => s.Screen == 1).ToArray().First();
        display.Undeploy();
    }

    /// <summary>
    /// Deploy or undeploy the left screen
    /// </summary>
    /// <param name="show">Whether or not the screen must be showed</param>
    public void ShowLeftScreen(bool show)
    {
        ScreenDisplay display = GetComponents<ScreenDisplay>().Where(s => s.Screen == 0).ToArray().First();
        if (show)
            display.Undeploy();
        else
            display.Deploy();
    }

    /// <summary>
    /// Change the simulation speed
    /// </summary>
    /// <param name="slider">Slider holding the simulation speed</param>
    public void SetSimulationSpeed(Slider slider)
    {
        SimulationSpeed = slider.value;
    }
    
}
