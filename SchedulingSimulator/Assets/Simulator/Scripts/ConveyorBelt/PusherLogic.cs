using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherLogic : MonoBehaviour
{
    public float Speed;

    private Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos += transform.forward * (Speed * Time.deltaTime);

        transform.position = currentPos;
    }
}
