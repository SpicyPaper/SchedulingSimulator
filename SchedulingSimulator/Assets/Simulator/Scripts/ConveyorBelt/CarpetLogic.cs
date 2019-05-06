using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to move a block on a the conveyor belt
/// </summary>
public class CarpetLogic : MonoBehaviour
{
    readonly float Speed = 3;

    /// <summary>
    /// Set the velocity of the block on the carpet
    /// </summary>
    /// <param name="other">The object on the carpet</param>
    private void OnTriggerStay(Collider other)
    {
        other.attachedRigidbody.velocity = -transform.forward * Speed * GameHandler.SimulationSpeed;
    }

    /// <summary>
    /// When the block has exited the carpet, nullify its velocity
    /// </summary>
    /// <param name="other">The object exiting the carpet</param>
    private void OnTriggerExit(Collider other)
    {
        other.attachedRigidbody.velocity = Vector3.zero;
    }
}
