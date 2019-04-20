using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLogScreen : MonoBehaviour
{
    private Text algorithmText;
    private Text dateText;
    private Text durationText;
    private Text stateText;
    private Text averageText;
    private Text averageScoreText;
    private Text maxText;
    private Text maxScoreText;
    private Text finalScoreText;

    public void Start()
    {
        algorithmText = GameObject.Find("AlgorithmText").GetComponent<Text>();
        dateText = GameObject.Find("DateText").GetComponent<Text>();
        durationText = GameObject.Find("DurationText").GetComponent<Text>();
        stateText = GameObject.Find("StateText").GetComponent<Text>();
        averageText = GameObject.Find("AverageText").GetComponent<Text>();
        averageScoreText = GameObject.Find("AverageScoreText").GetComponent<Text>();
        maxText = GameObject.Find("MaxText").GetComponent<Text>();
        maxScoreText = GameObject.Find("MaxScoreText").GetComponent<Text>();
        finalScoreText = GameObject.Find("FinalScoreText").GetComponent<Text>();
    }

    /// <summary>
    /// Set the log
    /// </summary>
    /// <param name="log">The log to be set</param>
    public void SetLog(Log log)
    {
        algorithmText.text = log.GetSchedulingName();
        dateText.text = log.GetDateAndTime();
        durationText.text = log.GetDuration();
        stateText.text = log.GetEndState();
        averageText.text = log.GetAverage();
        maxText.text = log.GetMax();

        averageScoreText.text = log.GetAverageScore();
        maxScoreText.text = log.GetMaxScore();
        finalScoreText.text = log.GetFinalScore();
    }
}
