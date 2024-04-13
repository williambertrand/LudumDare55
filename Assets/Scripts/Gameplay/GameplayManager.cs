using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoSingleton<GameplayManager>
{

    // Data required between rooms:
    public int currentRoom;


    private void Awake()
    {
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

    public void OnTaskCompleted()
    {

    }

    public void OnRoomCompleted()
    {

    }


    public void OnNextRoom()
    {
        
    }
}
