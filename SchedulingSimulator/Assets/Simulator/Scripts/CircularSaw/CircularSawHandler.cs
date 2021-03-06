﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the circular saw object.
/// </summary>
public class CircularSawHandler : MonoBehaviour
{
    public int SlotID;

    public List<GameObject> CircularSaws { get; set; }

    private const float ROTATION_SPEED = 5;

    private bool isSawRotationEnable;

    public bool IsSawTurning { get; private set; }
    
    /// <summary>
    /// Find all children in the general circular saw.
    /// Init variables
    /// </summary>
    void Start()
    {
        isSawRotationEnable = false;

        CircularSaws = new List<GameObject>();

        foreach (Transform childSide in transform)
        {
            foreach (Transform childCenter in childSide)
            {
                foreach (Transform childSaw in childCenter)
                {
                    CircularSaws.Add(childSaw.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Check if a bloc (process) is on the circular saw
    /// </summary>
    /// <returns></returns>
    private bool CheckIfCorrectBlocIsOn()
    {
        RaycastHit rayHit;
        Collider collider = GetComponent<Collider>();
        float rayCastSize = 0.5f;

        bool res = Physics.Raycast(
            collider.bounds.center,
            Vector3.up,
            out rayHit,
            collider.bounds.extents.y + rayCastSize);

        // The characters can pass through some items, they are listed here.
        if (res && rayHit.transform.tag == "FactoryCube")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Check if a bloc (process) is on the circular saw and if it's ready to be processed.
    /// </summary>
    void Update()
    {
        isSawRotationEnable = CheckIfCorrectBlocIsOn();

        if(isSawRotationEnable &&
            Scheduler.SlotID == SlotID)
        {
            IsSawTurning = true;
            for (int i = 0; i < CircularSaws.Count; i++)
            {
                if(i > CircularSaws.Count / 2 - 1)
                {
                    CircularSaws[i].transform.Rotate(Vector3.right * ROTATION_SPEED);
                }
                else
                {
                    CircularSaws[i].transform.Rotate(Vector3.right * -ROTATION_SPEED);
                }
            }
        }
        else
        {
            IsSawTurning = false;
        }
    }
}
