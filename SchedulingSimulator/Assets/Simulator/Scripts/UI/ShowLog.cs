using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to show a specific log on the screen
/// </summary>
public class ShowLog : MonoBehaviour
{
    public Log Log;

    private GameHandler gameHandler;

    private void Start()
    {
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
    }

    /// <summary>
    /// Display the log
    /// </summary>
    public void DisplayLog()
    {
        gameHandler.DisplayLog(Log);
    }
}
