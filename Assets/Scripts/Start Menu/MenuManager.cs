using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public int bestScore = 0;
    
    public string playerName;
    
    private void Awake()
    {
        if (null != Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetBestInfo()
    {
        return "Best Score: " + bestScore + ", Player Name: " + playerName;
    }
}
