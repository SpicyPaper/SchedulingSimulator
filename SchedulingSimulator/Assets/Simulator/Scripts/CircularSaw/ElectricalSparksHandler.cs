using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the electrical sparks particle
/// </summary>
public class ElectricalSparksHandler : MonoBehaviour
{
    [SerializeField]
    private CircularSawHandler sawHandler;
    
    /// <summary>
    /// Play or stop the particle system when it's needed
    /// </summary>
    void Update()
    {
        if (sawHandler.IsSawTurning)
        {
            if (!GetComponent<ParticleSystem>().isPlaying)
            {
                GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            GetComponent<ParticleSystem>().Stop();
        }
    }
}
