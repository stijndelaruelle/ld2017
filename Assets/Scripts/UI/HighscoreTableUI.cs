using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTableUI : MonoBehaviour
{
    [SerializeField]
    private HighscoreSystem m_HighscoreSystem;

    [SerializeField]
    private Text m_TextInfo;
    private HighscoreLabelUI m_HighscoreLabelUIPrefab;
    private List<HighscoreLabelUI> m_HighscoreLabels;

    private void Awake()
    {
        m_HighscoreLabels = new List<HighscoreLabelUI>();

        //There should be only 1 child, and this is the one we're going to use as a "prefab"
        if (transform.childCount < 0)
        {
            Debug.LogWarning("The highscore table should have 1 child. This will be the score label template.", this);
        }

        m_HighscoreLabelUIPrefab = transform.GetChild(0).GetComponent<HighscoreLabelUI>();

        if (m_HighscoreLabelUIPrefab == null)
        {
            Debug.LogWarning("No 'HighscoreLabelUI' found on label template!", this);
        }
        else
        {

            m_HighscoreLabels.Add(m_HighscoreLabelUIPrefab);
        }

        HideInfo();
        HideScores();
    }

    private void Start()
    {
        if (m_HighscoreSystem == null)
        {
            Debug.LogError("HighscoreTableUI doesn't have access to the highscore system!", this);
            return;
        }

        m_HighscoreSystem.HighscoreUpdateEvent += OnHighscoresUpdated;
        m_HighscoreSystem.HighscoreUpdateFailedEvent += OnHighscoreUpdateFailed;
    }

    private void OnDestroy()
    {
        if (m_HighscoreSystem != null)
        {
            m_HighscoreSystem.HighscoreUpdateEvent -= OnHighscoresUpdated;
            m_HighscoreSystem.HighscoreUpdateFailedEvent -= OnHighscoreUpdateFailed;
        }
    }

    private void OnEnable()
    {
        RefreshScores();
    }


    public void RefreshScores()
    {
        m_HighscoreSystem.UpdateHighscores();

        m_TextInfo.text = "Fetching highscores";
        ShowInfo();
    }


    //Callbacks
    private void OnHighscoresUpdated()
    {
        int numberOfHighscores = m_HighscoreSystem.GetNumberOfHighscores;

        for (int i = 0; i < numberOfHighscores; ++i)
        {
            if (i >= m_HighscoreLabels.Count)
            {
                HighscoreLabelUI newLabel = Instantiate<HighscoreLabelUI>(m_HighscoreLabelUIPrefab, transform);
                m_HighscoreLabels.Add(newLabel);
            }

            m_HighscoreLabels[i].UpdateScore(m_HighscoreSystem.GetHighscore(i));
        }

        HideInfo();
        ShowScores();
    }

    private void OnHighscoreUpdateFailed()
    {
        m_TextInfo.text = "Error.";
        ShowInfo();
    }


    //Show & Hide info
    private void HideInfo()
    {
        ShowInfo(false);
    }

    private void ShowInfo()
    {
        ShowInfo(true);
    }

    private void ShowInfo(bool state)
    {
        m_TextInfo.gameObject.SetActive(state);
    }


    //Show & Hide scoress
    private void HideScores()
    {
        ShowScores(false);
    }

    private void ShowScores()
    {
        ShowScores(true);
    }

    private void ShowScores(bool state)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(state);
        }
    }
}
