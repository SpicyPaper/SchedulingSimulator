using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmSelection : MonoBehaviour
{
    public Color OnColor = Color.green;
    public Color OffColor = Color.red;

    public List<GameObject> Lights;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisableAllLights()
    {
        for (int i = 0; i < Lights.Count; i++)
            ChangeLight(i, OffColor);
    }

    private void ChangeLight(int id, Color color)
    {
        if (id >= Lights.Count)
            return;

        Lights[id].GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }

    public void SelectAlgo(int id)
    {
        DisableAllLights();
        ChangeLight(id, OnColor);
        // TODO change algo here
        //GameObject.Find("GameHandler").GetComponent<GameHandler>().ChangeScheduler((Scheduler.Scheduling)id);
    }



    
}
