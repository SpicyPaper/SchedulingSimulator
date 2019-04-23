using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeColor : MonoBehaviour
{
    public Color OnColor = Color.green;
    public Color OffColor = Color.red;

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Renderer>().material.SetColor("_Color", OnColor);
    }

    private void OnTriggerExit(Collider other)
    {
        ResetColor();
    }

    public void ResetColor()
    {
        GetComponent<Renderer>().material.SetColor("_Color", OffColor);
    }
}
