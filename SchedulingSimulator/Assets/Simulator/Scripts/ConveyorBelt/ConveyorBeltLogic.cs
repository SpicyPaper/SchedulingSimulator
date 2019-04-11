using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltLogic : MonoBehaviour
{
    float scrollSpeed = 1.3f;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Time.time * scrollSpeed * GameHandler.SimulationSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }
}
