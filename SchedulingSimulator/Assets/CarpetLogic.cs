using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetLogic : MonoBehaviour
{
    float Speed = 3;

    private void OnTriggerStay(Collider other)
    {
        other.attachedRigidbody.velocity = -transform.forward * Speed * GameHandler.SimulationSpeed;
    }

    private void OnTriggerExit(Collider other)
    {
        other.attachedRigidbody.velocity = Vector3.zero;
    }
}
