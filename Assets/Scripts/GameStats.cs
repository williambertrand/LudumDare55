using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoSingleton<GameStats>
{

    public int roomsCleared;
    public int totalTasks;
    public bool didWin;
    public bool didResume;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
