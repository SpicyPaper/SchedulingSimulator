using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to display (deploy) or hide (undeploy) a screen
/// </summary>
public class ScreenDisplay : MonoBehaviour
{
    public int Screen;
    public Transform ScreenPosition;
    public Transform DeployedPosition;
    private Vector3 startPos;
    private Vector3 endPos;

    public bool IsDisplayed { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        startPos = ScreenPosition.position;
        endPos = DeployedPosition.position;

    }

    /// <summary>
    /// Deploy the screen (make it visible)
    /// </summary>
    public void Deploy()
    {
        if (!IsDisplayed)
        {
            StartCoroutine(MoveTo(startPos, endPos));
            IsDisplayed = true;
        }
    }

    /// <summary>
    /// Undeploy the screen (make it invisible)
    /// </summary>
    public void Undeploy()
    {
        if (IsDisplayed)
        {
            StartCoroutine(MoveTo(endPos, startPos));
            IsDisplayed = false;
        }
    }
    
    /// <summary>
    /// Move the screen from one place to another
    /// </summary>
    /// <param name="startPos">Start position of the screen</param>
    /// <param name="endPos">End position of the screen</param>
    /// <returns></returns>
    IEnumerator MoveTo(Vector3 startPos, Vector3 endPos)
    {
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 1)
        { // until one second passed
            ScreenPosition.position = Vector3.Lerp(startPos, endPos, Time.time - startTime); // lerp from A to B in one second
            yield return 1; // wait for next frame
        }
    }
}
