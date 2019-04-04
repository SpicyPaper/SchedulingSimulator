using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddObjectToList : MonoBehaviour
{
    public GameObject Content;
    public GameObject ButtonPrefab;

    public void AddItem()
    {
        GameObject button = Instantiate(ButtonPrefab);
        button.transform.SetParent(Content.transform);
        button.transform.localPosition = Vector3.zero;
        button.transform.localRotation = Quaternion.Euler(Vector3.zero);
        

        button.GetComponentInChildren<Text>().text = "test";
    }
}
