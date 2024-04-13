using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoSingleton<GameplayManager>
{

    // Data required between rooms:
    public int currentRoom;
    public List<Room> gameRooms;
    public GameObject roomHolder;

    [Header("Transitioning Between rooms")]
    public float transitionTime;
    public float transitionHoldTime;
    public GameObject roomTransitionUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        RoomTasksManager.Instance.onRoomComplete += OnRoomCompleted;

        CheckRoomSetup();

        OnNextRoom();
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

    private void UpdatePlayerPos(Vector3 pos)
    {
        Debug.Log("Updating player pos to new room...");
        PlayerController.Instance.gameObject.transform.position = pos;
    }

    public void OnRoomCompleted(List<Task> tasks)
    {

        Debug.Log("Game mgr ROOM COMPLETE!!");

        // Update score
        currentRoom += 1;

        // Show transition ui, update player pos, and then hide transition ui
        StartCoroutine(HandleTransitionAndNextRoom());
    }


    public void OnNextRoom()
    {
        Room newRoom = gameRooms[currentRoom];
        GameObject roomGameObjSpawn = GameObject.Find(newRoom.roomName + "/RoomBasics/PlayerSpawn");
        if (roomGameObjSpawn == null)
        {
            Debug.LogWarningFormat("Did not find room spawn: {0}", newRoom.roomName);
        }
        RoomTasksManager.Instance.OnNewRoom(newRoom);
        UpdatePlayerPos(roomGameObjSpawn.transform.position);
    }

    private void CheckRoomSetup()
    {
        for(int i = 0; i > gameRooms.Count; i++)
        {
            Room roomDef = gameRooms[i];
            GameObject roomGameObj = GameObject.Find(roomDef.roomName);

            if (roomGameObj == null)
            {
                Debug.LogWarningFormat("Did not find room: {0}", roomDef.roomName);
            }
        }
    }

    public IEnumerator HandleTransitionAndNextRoom()
    {
        AnimateRoomTransition(true);
        yield return new WaitForSeconds(transitionTime);

        OnNextRoom();

        yield return new WaitForSeconds(transitionHoldTime);

        AnimateRoomTransition(false);

    }

    private void AnimateRoomTransition(bool shown)
    {
        roomTransitionUI.SetActive(shown);
    }
}
