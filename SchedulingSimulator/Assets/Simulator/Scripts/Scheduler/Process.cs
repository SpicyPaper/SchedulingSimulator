using UnityEngine;

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
    }

    public State GetState()
    {
        return state;
    }

    public void Place(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    public void Admit()
    {
        gameObject = Object.Instantiate(prefab) as GameObject;
        gameObject.transform.localScale = new Vector3(0.4f, SIZE_RATIO * Duration, 0.4f);
        gameObject.transform.parent = GameObject.Find("Processes").transform;
        gameObject.name = Name;

        state = State.New;
    }

    public void Arrived()
    {
        timeArrived = Time.time;
    }

    public void WatchOut()
    {
        if (state == State.New &&
                gameObject != null &&
                gameObject.transform.position.y - gameObject.transform.localScale.y / 2 < -2)
        {
            state = State.Ready;
        }
    }

    public void Consume(float time)
    {
        if (state == State.Ready)
        {
            state = State.Running;
            TimeWaited += Time.time - timeArrived;
        }

        if (state == State.Running)
        {
            Progress -= time;
            if (Progress <= 0)
            {
                Object.Destroy(gameObject);
                state = State.Terminated;
            }
            else
            {
                gameObject.transform.localScale = new Vector3(0.4f, SIZE_RATIO * Progress, 0.4f);
            }
        }
    }

    public void Reset()
    {
        state = State.New;
        timeArrived = Time.time;
    }

    
}
