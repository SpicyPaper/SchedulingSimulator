using UnityEngine;

/// <summary>
/// Process of the scheduling simulator
/// </summary>
public class Process
{
    public enum State { New, Ready, Running, Waiting, Terminated };

    public string Name { get; set; }
    public float Arrival { get; set; }
    public float Duration { get; set; }
    public float Progress { get; set; }
    public float TimeWaited { get; set; }

    private State state;
    private GameObject gameObject;
    private readonly GameObject prefab;
    private float timeArrived;
    private float timeTravel;
    private float timeStartTravel;
    private bool arrived;

    private const float SIZE_RATIO = 0.2f;

    public Process(GameObject prefab, string name, float arrival, float duration)
    {
        this.prefab = prefab;

        Name = name;
        Arrival = arrival;
        Duration = duration;
        Progress = duration;
        state = State.New;
        timeArrived = 0;
        timeTravel = 0;
        timeStartTravel = 0;
        arrived = false;
    }

    public State GetState()
    {
        return state;
    }

    /// <summary>
    /// Place the gameobject of the process in his position 
    /// </summary>
    /// <param name="pos">Vector corresponding to the new process position</param>
    public void Place(Vector3 pos)
    {
        gameObject.transform.position = pos;
        timeStartTravel = Time.time;
    }

    /// <summary>
    /// Create a new process and his gameobject, setting his state to new
    /// </summary>
    public void Admit()
    {
        gameObject = Object.Instantiate(prefab) as GameObject;
        gameObject.transform.localScale = new Vector3(0.4f, SIZE_RATIO * Duration, 0.4f);
        gameObject.transform.parent = GameObject.Find("Processes").transform;
        gameObject.name = Name;

        state = State.New;
    }

    /// <summary>
    /// Register the time where the process is ready to be treated
    /// </summary>
    public void Arrived()
    {
        timeArrived = Time.time;
    }

    /// <summary>
    /// Check if a new process is ready to be treated
    /// </summary>
    public void WatchOut()
    {
        if (state == State.New &&
                gameObject != null &&
                gameObject.transform.position.y - gameObject.transform.localScale.y / 2 < -2)
        {
            state = State.Ready;

            if (!arrived)
            {
                arrived = true;
                timeTravel = Time.time - timeStartTravel;
            }
        }
    }

    /// <summary>
    /// Update the progress of the process
    /// </summary>
    /// <param name="time">Time to increment</param>
    public void Consume(float time)
    {
        // Update the state of the process to Running if it's not already the case
        if (state == State.Ready)
        {
            state = State.Running;
            TimeWaited += Time.time - timeArrived;
        }

        // Increase progress
        if (state == State.Running)
        {
            Progress -= time;
            if (Progress <= 0)
            {
                Object.Destroy(gameObject);
                state = State.Terminated;
                TimeWaited -= timeTravel;
            }
            else
            {
                gameObject.transform.localScale = new Vector3(0.4f, SIZE_RATIO * Progress, 0.4f);
            }
        }
    }

    /// <summary>
    /// Reset a process
    /// </summary>
    public void Reset()
    {
        state = State.New;
        timeArrived = Time.time;
    }

    
}
