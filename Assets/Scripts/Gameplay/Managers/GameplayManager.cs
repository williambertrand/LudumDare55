using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoSingleton<GameplayManager>
{

    // Data required between rooms:
    public int currentRoom;
    public int totalTasks;
    public bool didWin;
    public List<Room> gameRooms;
    public GameObject roomHolder;

    [Header("Transitioning Between rooms")]
    public float delayTransitionTime;
    public float transitionTime;
    public float transitionHoldTime;
    public CanvasGroup roomTransitionUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        RoomTasksManager.Instance.onRoomComplete += OnRoomCompleted;
        totalTasks = 0;
        CheckRoomSetup();

        OnNextRoom();
        didWin = false;
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

        totalTasks += tasks.Count;

        if (currentRoom == gameRooms.Count - 1)
        {
            // On game completed!
            didWin = true;
            StartCoroutine(HandleTransitionToGameOver());
            return;
        }

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
        yield return new WaitForSeconds(delayTransitionTime);

        AnimateRoomTransition(true);
        yield return new WaitForSeconds(transitionTime);

        OnNextRoom();

        yield return new WaitForSeconds(transitionHoldTime);

        AnimateRoomTransition(false);

    }

    private void AnimateRoomTransition(bool shown)
    {
        
        if(shown)
        {
            roomTransitionUI.alpha = 0;
            roomTransitionUI.DOFade(1, transitionTime);
        } else
        {
            roomTransitionUI.alpha = 1;
            roomTransitionUI.DOFade(0, transitionTime);
        }
    }

    public IEnumerator HandleTransitionToGameOver()
    {
        yield return new WaitForSeconds(delayTransitionTime);

        SceneManager.LoadScene(SCENES.END);
    }
}
