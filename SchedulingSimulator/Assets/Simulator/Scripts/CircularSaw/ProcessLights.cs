using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Change the light under the saw depending if the saw is active or not
/// </summary>
public class ProcessLights : MonoBehaviour
{
    public Color OnColor = Color.green;
    public Color OffColor = Color.red;

    [SerializeField]
    private CircularSawHandler sawHandler;
    

    // Update is called once per frame
    void Update()
    {
        if (sawHandler.IsSawTurning)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", OnColor);
        }
        else
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", OffColor);
        }
    }
}
