using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Process
{
    public enum State { New, Ready, Running, Waiting, Terminated };

    public string Name { get; set; }
    public float Arrival { get; set; }
    public float Duration { get; set; }
    public float Progress { get; set; }

    private State state;
    private GameObject gameObject;
    private readonly GameObject prefab;
    private const float SIZE_RATIO = 0.2f;

    public Process(GameObject prefab, string name, float arrival, float duration)
    {
        this.prefab = prefab;
        Name = name;
        Arrival = arrival;
        Duration = duration;
        Progress = duration;
        state = State.New;
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
        state = State.Ready;
    }

    public void Start()
    {
        state = State.Running;
    }

    public float Consume(float time)
    {
        Progress -= time;
        if (Progress <= 0)
        {
            Object.Destroy(gameObject);
            state = State.Terminated;
            return -Duration;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(0.4f, SIZE_RATIO * Progress, 0.4f);
            return 0f;
        }
    }

    public void Interrupt()
    {
        state = State.Waiting;
    }

    
}
