using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorDetector : MonoBehaviour
{
    public bool IsStartDetector;
    public GameObject PusherModel;
    public GameObject Parent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ConveyorPusher")
        {
            if(!IsStartDetector)
            {
                Destroy(other.gameObject);
            }
        }

        if(other.tag == "FactoryCube")
        {
            if(IsStartDetector)
            {
                GameObject pusher = Instantiate(PusherModel);
                pusher.transform.position = transform.position + transform.forward * -0.5f;
                pusher.transform.parent = Parent.transform;
            }
        }
    }
}
