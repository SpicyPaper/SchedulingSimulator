using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalSparksHandler : MonoBehaviour
{
    [SerializeField]
    private CircularSawHandler sawHandler;

    // Update is called once per frame
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
