using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSawHandler : MonoBehaviour
{
    public List<GameObject> CircularSaws { get; set; }

    private const float ROTATION_SPEED = 5;

    private bool isSawRotationEnable;

    // Start is called before the first frame update
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

    private bool CheckIfBlocIsOn()
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
            return true;

        return !res;
    }

    // Update is called once per frame
    void Update()
    {
        isSawRotationEnable = CheckIfBlocIsOn();

        if(isSawRotationEnable)
        {
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
    }
}
