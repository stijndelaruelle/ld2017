using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreSubmitButtonUI : MonoBehaviour
{
    [SerializeField]
    private HighscoreSystem m_HighscoreSystem;

    [SerializeField]
    private InputField m_NameInputField;

    [SerializeField]
    private InputField m_ScoreInputField;

    public void SubmitHighscore()
    {
        int intScore = 0;
        bool success = int.TryParse(m_ScoreInputField.text, out intScore);

        if (success)
        {
            m_HighscoreSystem.PostHighscore(m_NameInputField.text, intScore);
        }
        else
        {
            Debug.LogError("The score you wanted to submit is not an integer", this);
        }
    }
}
