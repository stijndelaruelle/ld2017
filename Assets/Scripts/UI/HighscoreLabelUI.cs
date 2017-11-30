using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreLabelUI : MonoBehaviour
{
    [SerializeField]
    private Text m_NameLabel;

    [SerializeField]
    private Text m_ScoreText;

    [SerializeField]
    private bool m_DisplayScoreAsTime;

    public void UpdateScore(Highscore highScore)
    {
        UpdateScore(highScore.name, highScore.score);
    }

    public void UpdateScore(string name, int score)
    {
        m_NameLabel.text = name;

        if (m_DisplayScoreAsTime)
        {
            string timeString = "";

            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, score);
            int hours = timeSpan.Hours;

            if (hours > 0)
            {
                timeString += hours.ToString("00") + ":";
            }

            timeString += timeSpan.Minutes.ToString("00") + ":";
            timeString += timeSpan.Seconds.ToString("00") + ":";
            timeString += timeSpan.Milliseconds.ToString("00");

            m_ScoreText.text = timeString;
        }
        else
        {
            m_ScoreText.text = score.ToString();
        }
    }
}
