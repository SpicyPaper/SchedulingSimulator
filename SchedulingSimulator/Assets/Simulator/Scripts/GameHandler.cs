using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameObject processPrefab;
    public Scheduler.Scheduling scheduling;
    public int slots;
    public float quantum;
    public Text SimulationState;
    public Text Seed;
    public Text JsonPathFile;

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
    private List<Process> finishedProcesses;
    private bool firstTime;
    private GameObject processesObjects;
    private Statistics stats;
    private AlgorithmSelection algorithmSelection;
    private string jsonPath;
    private bool createLog;
    private int seed;

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

    public void OpenExplorer()
    {
        jsonPath = StandaloneFileBrowser.OpenFilePanel("Open File", "", "json", false)[0];

        if (jsonPath != "")
        {
            JsonPathFile.color = Color.black;
            JsonPathFile.text = Path.GetFileName(jsonPath);
        }
    }

    /// <summary>
    /// Read the content of a given file
    /// </summary>
    /// <param name="path">the path where the file is located</param>
    /// <returns>the content of the file</returns>
    public string ReadString(string path)
    {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string content = reader.ReadToEnd();
        reader.Close();

        return content;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Create a json file with process models
        // CreateTempProcessJsonFile();

        jsonPath = "";
        JsonPathFile.color = Color.blue;
        JsonPathFile.text = "No file imported";
        initialGravity = Physics.gravity;
        IsRunning = false;
        createLog = true;
        algorithmSelection = GetComponent<AlgorithmSelection>();
        processes = new List<Process>();
        finishedProcesses = new List<Process>();
        seed = 0;

        processesObjects = new GameObject
        {
            name = "Processes"
        };

        firstTime = true;
    }

    public void GeneratorRandomProcesses()
    {
        string seedText = Seed.text;
        if(seedText != "")
        {
            try
            {
                seed = int.Parse(seedText);
            }
            catch (System.FormatException) {}
        }
        Random.InitState(seed);

        int nbProcess = Random.Range(0, 50);

        for (int i = 0; i < nbProcess; i++)
        {
            processes.Add(new Process(processPrefab, "P" + i.ToString(), Random.Range(0, 60), Random.Range(1, 10)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
        {
            // Action available
        }
        else if (processes.Count == 0 && scheduler.IsDone())
        {
            StopSimulation(false);
        }
        else
        {
            float deltaTime = SimulationSpeed * Time.deltaTime;
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
                            finishedProcesses.Add(process);
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

    private void CreateProcessJsonFile()
    {
        ListProcessesModel listProcessModel = new ListProcessesModel();
        listProcessModel.processes = new List<ProcessModel>();

        ProcessModel processModel = new ProcessModel
        {
            arrival = 0,
            duration = 2
        };
        listProcessModel.processes.Add(processModel);

        ProcessModel processModel2 = new ProcessModel
        {
            arrival = 1,
            duration = 2
        };
        listProcessModel.processes.Add(processModel2);

        ProcessModel processModel3 = new ProcessModel
        {
            arrival = 0,
            duration = 5
        };
        listProcessModel.processes.Add(processModel3);

        Debug.Log(JsonUtility.ToJson(listProcessModel));
    }

    /// <summary>
    /// Create the processes based on the json file content
    /// </summary>
    /// <param name="jsonFileContent">The content of a json file containing process</param>
    private bool CreateProcessesBasedOnJsonFile(string jsonFileContent)
    {
        try
        {
            ListProcessesModel listProcessesModel = JsonUtility.FromJson<ListProcessesModel>(jsonFileContent);

            int counter = 0;
            float processDuration = 0;
            foreach (ProcessModel processModel in listProcessesModel.processes)
            {
                counter++;
                if(processModel.duration > 10)
                {
                    processDuration = 10;
                }
                else
                {
                    processDuration = processModel.duration;
                }
                processes.Add(new Process(processPrefab, "P" + counter, processModel.arrival, processDuration));
            }

            Random.InitState(0);

            return true;
        }
        catch (System.Exception)
        {
            JsonPathFile.color = Color.red;
            JsonPathFile.text = "An error occured!";
            createLog = false;
            StopSimulation(false);

            return false;
        }
    }

    /// <summary>
    /// Called when starting the animation with the start button
    /// </summary>
    public void StartSimulation()
    {
        bool isValid = true;
        HideLog();
        IsRunning = true;
        ShowLeftScreen(false);
        scheduler = new Scheduler(algorithmSelection.CurrentAlgo, slots, quantum / SimulationSpeed, SpawnPoint);

        processes = new List<Process>();
        finishedProcesses = new List<Process>();
        if(jsonPath == "")
        {
            GeneratorRandomProcesses();
        }
        else
        {
            isValid = CreateProcessesBasedOnJsonFile(ReadString(jsonPath));
        }

        if(isValid)
        {
            stats = new Statistics(finishedProcesses, SimulationSpeed);
            timePassed = 0;

            GameObject.Find("btnImport").GetComponent<Button>().interactable = false;
            GameObject.Find("btnStart").GetComponent<Button>().interactable = false;
            GameObject.Find("SliderSpeed").GetComponent<Slider>().interactable = false;
            GameObject.Find("inputSeed").GetComponent<InputField>().interactable = false;
        }
    }

    /// <summary>
    /// Remove the json path import
    /// </summary>
    public void RemoveJsonPath()
    {
        jsonPath = "";
        JsonPathFile.color = Color.blue;
        JsonPathFile.text = "No file imported";
    }

    /// <summary>
    /// Called when stopping the simulation with the stop button
    /// </summary>
    public void StopSimulation(bool forced)
    {
        if (IsRunning)
        {
            if(createLog)
            {
                Log log = new Log(stats, algorithmSelection.CurrentAlgo, timePassed, forced, simulationSpeed, seed, Path.GetFileName(jsonPath));
                GetComponent<AddObjectToList>().AddItem(log);
            }

            IsRunning = false;
            ShowLeftScreen(true);
            
            DestroyProcesses();
            firstTime = true;

            GameObject.Find("btnImport").GetComponent<Button>().interactable = true;
            GameObject.Find("btnStart").GetComponent<Button>().interactable = true;
            GameObject.Find("SliderSpeed").GetComponent<Slider>().interactable = true;
            GameObject.Find("inputSeed").GetComponent<InputField>().interactable = true;

            PipeColor[] pipes = GameObject.FindObjectsOfType<PipeColor>();
            foreach (PipeColor pipe in pipes)
            {
                pipe.ResetColor();
            }
            createLog = true;
        }
    }

    /// <summary>
    /// Quit and close the application
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
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
