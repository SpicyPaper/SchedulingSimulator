using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to light up the light under the algorithm button
/// </summary>
public class AlgorithmSelection : MonoBehaviour
{
    public Color OnColor = Color.green;
    public Color OffColor = Color.red;

    public List<GameObject> Lights;
    public Scheduler.Scheduling CurrentAlgo { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        SelectAlgo(0);        
    }

    /// <summary>
    /// Disable all lights by turning them red (by default)
    /// </summary>
    private void DisableAllLights()
    {
        for (int i = 0; i < Lights.Count; i++)
            ChangeLight(i, OffColor);
    }

    /// <summary>
    /// Change the specified light color
    /// </summary>
    /// <param name="id">Id of the light that will be changed (from 0 to 3 by default)</param>
    /// <param name="color">The color to put on the light</param>
    private void ChangeLight(int id, Color color)
    {
        if (id >= Lights.Count)
            return;

        Lights[id].GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }

    /// <summary>
    /// Select an algorithm
    /// </summary>
    /// <param name="id">Id of the algorithm to select</param>
    public void SelectAlgo(int id)
    {
        CurrentAlgo = (Scheduler.Scheduling)id;
        DisableAllLights();
        ChangeLight(id, OnColor);
    }
}
