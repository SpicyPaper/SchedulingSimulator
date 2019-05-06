using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add an item (a button) to the content of a scrollview
/// </summary>
public class AddObjectToList : MonoBehaviour
{
    public GameObject Content;
    public GameObject ButtonPrefab;

    /// <summary>
    /// Add a button on the left screen holding the log information
    /// </summary>
    /// <param name="log">Log of the last run</param>
    public void AddItem(Log log)
    {
        GameObject button = Instantiate(ButtonPrefab);
        button.transform.SetParent(Content.transform);
        button.transform.localPosition = Vector3.zero;
        button.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
        Text buttonText = button.GetComponentInChildren<Text>();
        buttonText.text = log.GetSchedulingName() + "\n" + log.GetTime();
        buttonText.color = Color.black;
        buttonText.fontSize = 1;

        button.GetComponent<ShowLog>().Log = log;
    }
}
