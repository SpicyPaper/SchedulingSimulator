using System.Collections;
using System.Collections.Generic;
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
    private bool done;
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
        scheduler = new Scheduler(scheduling, slots, quantum, SpawnPoint);
        processes = new List<Process>();
        done = false;

        processesObjects = new GameObject
        {
            name = "Processes"
        };

        firstTime = true;

        GenerateComplexProcesses();
        stats = new Statistics(processes.Count);
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
        if (done)
        {

        }
        else if (processes.Count == 0 && scheduler.IsDone())
        {
            float results = stats.Results();
            Debug.Log(results + " seconds per process");
            done = true;
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

    public void StartSimulation()
    {
        Debug.Log(string.Format("Simulation {0} Started", algorithmSelection.CurrentAlgo));
        IsRunning = true;
        ShowLeftScreen(false);
    }

    public void StopSimulation()
    {
        if (IsRunning)
        {
            Log log = new Log(System.DateTime.Now.ToShortTimeString(), algorithmSelection.CurrentAlgo);
            GetComponent<AddObjectToList>().AddItem(log);
            Debug.Log("Simulation stopped");
            IsRunning = false;
            ShowLeftScreen(true);
        }
    }

    public void DisplayLog(Log log)
    {
        ScreenDisplay display = GetComponents<ScreenDisplay>().Where(s => s.Screen == 1).ToArray().First();
        
        if (!display.IsDisplayed)
            display.Deploy();

        GetComponent<UpdateLogScreen>().SetLog(log);
    }

    public void HideLog()
    {
        ScreenDisplay display = GetComponents<ScreenDisplay>().Where(s => s.Screen == 1).ToArray().First();
        display.Undeploy();
    }

    public void ShowLeftScreen(bool show)
    {
        ScreenDisplay display = GetComponents<ScreenDisplay>().Where(s => s.Screen == 0).ToArray().First();
        if (show)
            display.Undeploy();
        else
            display.Deploy();
    }

    public void SetSimulationSpeed(Slider slider)
    {
        SimulationSpeed = slider.value;
    }
    
}
