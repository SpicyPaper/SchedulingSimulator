﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLog : MonoBehaviour
{
    public Log Log;

    private GameHandler gameHandler;

    private void Start()
    {
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
    }

    public void DisplayLog()
    {
        gameHandler.DisplayLog(Log);
    }
}
