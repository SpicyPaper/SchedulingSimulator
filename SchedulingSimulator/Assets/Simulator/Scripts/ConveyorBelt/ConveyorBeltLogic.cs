using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the conveyor belt logic
/// </summary>
public class ConveyorBeltLogic : MonoBehaviour
{
    float scrollSpeed = 1.3f;
    Renderer rend;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    
    void Update()
    {
        float offset = Time.time * scrollSpeed * GameHandler.SimulationSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }
}
