using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLogScreen : MonoBehaviour
{
    public Text Name;
    public Text SchedulingAlgorithm;

    private Log log;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetLog(Log log)
    {
        this.log = log;
        Name.text = log.Name;
        SchedulingAlgorithm.text = log.SchedulingType.ToString();
    }
}
