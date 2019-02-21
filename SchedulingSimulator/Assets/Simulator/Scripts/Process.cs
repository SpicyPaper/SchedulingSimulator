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
    private GameObject prefab;

    public Process(GameObject prefab, string name, float arrival, float duration)
    {
        this.prefab = prefab;
        Name = name;
        Arrival = arrival;
        Duration = duration;
        Progress = 0;
        state = State.New;
    }

    public void Place(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    public void Admit()
    {
        gameObject = Object.Instantiate(prefab) as GameObject;
        gameObject.transform.localScale = new Vector3(1f, 0.2f * Duration, 1f);
        gameObject.transform.parent = GameObject.Find("Processes").transform;
        gameObject.name = Name;
        state = State.Ready;
    }

    public void Interrupt()
    {
        state = State.Waiting;
    }

    
}
