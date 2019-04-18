using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLogScreen : MonoBehaviour
{
    public Text Name;
    public Text SchedulingAlgorithm;

    private Log log;

    /// <summary>
    /// Set the log
    /// </summary>
    /// <param name="log">The log to be set</param>
    public void SetLog(Log log)
    {
        this.log = log;
        Name.text = log.Name;
        SchedulingAlgorithm.text = log.SchedulingType.ToString();
    }
}
