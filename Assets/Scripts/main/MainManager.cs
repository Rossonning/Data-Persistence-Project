using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using File = System.IO.File;
using Input = UnityEngine.Input;
using Random = UnityEngine.Random;


public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public GameObject GameOverText;
    public Text playerInfo;
    public Text bestPlayerInfo;
    public Button exitButton;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private SaveData _saveData;

    private void Awake()
    {
        LoadPlayInfo();
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        playerInfo.text = "Score: " + m_Points + ", Name: " + MenuManager.Instance.playerName;
        bestPlayerInfo.text = GetBestScoreInfo();

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    public void Exit()
    {
        SavePlayInfo();

        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        playerInfo.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestScoreOwner;
    }

    public void SavePlayInfo()
    {
        if (_saveData == null || m_Points > _saveData.bestScore)
        {
            SaveData data = new SaveData();
        
            data.bestScore = m_Points;
            data.bestScoreOwner = MenuManager.Instance.playerName;

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }

    public void LoadPlayInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            _saveData = JsonUtility.FromJson<SaveData>(json);
        }
    }

    private string GetBestScoreInfo()
    {
        if (_saveData != null)
        {
            return "Best Score: " + _saveData.bestScore + ", Owner: " + _saveData.bestScoreOwner;
        }

        return "No record";
    }
}