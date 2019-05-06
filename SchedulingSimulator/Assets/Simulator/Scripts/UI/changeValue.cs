using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Change the value of a text depending of a slider
/// </summary>
public class ChangeValue : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    /// <summary>
    /// Set the value of the text
    /// </summary>
    /// <param name="slider">Slider to take the value from</param>
    public void SetValue(Slider slider)
    {
        text.text = slider.value.ToString();
    }
}
