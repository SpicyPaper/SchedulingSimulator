using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleScreenDisplay : MonoBehaviour
{
    public Transform MiddleScreenPosition;
    public Transform DeployedPosition;
    private Vector3 startPos;
    private Vector3 endPos;

    public bool IsDisplayed { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        startPos = MiddleScreenPosition.position;
        endPos = DeployedPosition.position;

    }

    public void Deploy()
    {
        StartCoroutine(MoveTo(startPos, endPos));
        IsDisplayed = true;
    }

    public void Undeploy()
    {
        StartCoroutine(MoveTo(endPos, startPos));
        IsDisplayed = false;
    }

    IEnumerator MoveTo(Vector3 startPos, Vector3 endPos)
    {
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 1)
        { // until one second passed
            MiddleScreenPosition.position = Vector3.Lerp(startPos, endPos, Time.time - startTime); // lerp from A to B in one second
            yield return 1; // wait for next frame
        }
    }
}
