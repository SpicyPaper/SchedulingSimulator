using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        button.GetComponent<ShowLog>().Log = log;

        Text buttonText = button.GetComponentInChildren<Text>();
        buttonText.text = log.GetSchedulingName();
        buttonText.fontSize = 1;
    }
}
