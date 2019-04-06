using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics
{
    private readonly int nbProcesses;
    private float startTime;

    public Statistics(int nbProcesses)
    {
        this.nbProcesses = nbProcesses;
        startTime = Time.time;
    }

    public float Results()
    {
        float duration = Time.time - startTime;
        return duration / nbProcesses;
    }
}
