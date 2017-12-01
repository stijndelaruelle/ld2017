using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;

public delegate void HighscoreEvent();

public struct Highscore
{
    private int m_Position;
    public int Position
    {
        get { return m_Position; }
    }

    private string m_OwnerName;
    public string Name
    {
        get { return m_OwnerName; }
    }

    //Can also be time in ms
    private int m_Score;
    public int Score
    {
        get { return m_Score; }
    }
    

    public Highscore(int position, string name, int score)
    {
        m_Position = position;
        m_OwnerName = name;
        m_Score = score;
    }
}

public class HighscoreSystem : MonoBehaviour
{
    [SerializeField]
    private string m_AddHighscoreURL;

    [SerializeField]
    private string m_GetHighscoreURL;

    private List<Highscore> m_Highscores;
    public List<Highscore> Highscores
    {
        get { return m_Highscores; }
    }

    public int GetNumberOfHighscores
    {
        get { return m_Highscores.Count; }
    }

    private string m_SecretKey = "asbucpdeerfsgehcirjektlkmenyolpuqdrusmtduavrwex2y0z1a7b"; //supersecretkeyld2017 with salt in between

    public HighscoreEvent HighscorePostEvent;
    public HighscoreEvent HighscorePostFailedEvent;

    public HighscoreEvent HighscoreUpdateEvent;
    public HighscoreEvent HighscoreUpdateFailedEvent;

    private void Start()
    {
        m_Highscores = new List<Highscore>();
    }


    public void PostHighscore(string name, int score)
    {
        StartCoroutine(PostHighscoreRoutine(name, score));
    }

    public void UpdateHighscores()
    {
        StartCoroutine(UpdateHighscoresRoutine());
    }


    public int GetHighscoreID(string name)
    {
        //Change to map?
        for (int i = 0; i < m_Highscores.Count; ++i)
        {
            if (m_Highscores[i].Name == name)
            {
                return i;
            }
        }

        return 0;
    }

    public Highscore GetHighscore(string name)
    {
        //Change to map?
        foreach (Highscore highscore in m_Highscores)
        {
            if (highscore.Name == name)
            {
                return highscore;
            }
        }

        return new Highscore(0, "Empty", 0);
    }

    public Highscore GetHighscore(int highscoreID)
    {
        if (highscoreID >= 0 && highscoreID < m_Highscores.Count)
        {
            return m_Highscores[highscoreID];
        }

        return new Highscore(0, "Empty", 0);
    }


    private IEnumerator PostHighscoreRoutine(string name, int score)
    {
        string hash = Md5Sum(name + score + m_SecretKey);
        string postUrl = m_AddHighscoreURL +
                         "?name=" + WWW.EscapeURL(name) +
                         "&score=" + score +
                         "&hash=" + hash;

        WWW highscorePost = new WWW(postUrl);
        yield return highscorePost;

        if (highscorePost.error != null)
        {
            Debug.Log("There was an error posting the high score: " + highscorePost.error);

            if (HighscorePostFailedEvent != null)
                HighscorePostFailedEvent();
        }
        else
        {
            if (HighscorePostEvent != null)
                HighscorePostEvent();
        }
    }

    private IEnumerator UpdateHighscoresRoutine()
    {
        Debug.Log("Loading Scores");

        WWW highscoreGet = new WWW(m_GetHighscoreURL);
        yield return highscoreGet;

        if (highscoreGet.error != null)
        {
            Debug.Log("There was an error getting the highscores: " + highscoreGet.error);

            if (HighscoreUpdateFailedEvent != null)
                HighscoreUpdateFailedEvent();
        }
        else
        {
            //Split the string (comma separated)
            char[] delimiterChars = { ',' };
            string[] words = highscoreGet.text.Split(delimiterChars);

            m_Highscores.Clear();
            for (int i = 0; i < words.Length; i += 3)
            {
                if (String.IsNullOrEmpty(words[i]))
                    continue;

                //Get the position
                int position = 0;
                bool posParseSuccess = int.TryParse(words[i], out position);

                //Get the name
                string name = words[i + 1];

                //Get the score
                int score = 0;
                bool scoreParseSuccess = int.TryParse(words[i + 2], out score);

                //Add it to the list
                if (posParseSuccess && scoreParseSuccess)
                {
                    Highscore highscore = new Highscore(position, name, score);
                    m_Highscores.Add(highscore);
                }
            }

            if (HighscoreUpdateEvent != null)
                HighscoreUpdateEvent();
        }
    }

    //Taken from: http://wiki.unity3d.com/index.php?title=MD5
    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}
