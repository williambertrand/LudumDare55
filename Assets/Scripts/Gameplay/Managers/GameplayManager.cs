using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

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
    public TMP_Text transitionTitle;
    public CanvasGroup roomTransitionUI;

    public CanvasGroup dialogueUIPanel;
    public TMP_Text dialogueText;

    private string DEFAULT_TRANSITION_TEXT = "You have been summoned!";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        RoomTasksManager.Instance.onRoomComplete += OnRoomCompleted;
        totalTasks = 0;
        CheckRoomSetup();
    }

    private void OnDestroy()
    {
        RoomTasksManager.Instance.onRoomComplete -= OnRoomCompleted;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.Instance.waiting = true;
        OnNextRoom();
        OnNewRoomStarted();
        Timer.Instance.Pause();
        didWin = false;
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
        PlayerController.Instance.gameObject.transform.position = pos;
    }

    public void OnRoomCompleted(List<Task> tasks)
    {
        PlayerMovement.Instance.waiting = true;
        totalTasks += tasks.Count;
        Timer.Instance.Pause();

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
            roomGameObjSpawn = GameObject.Find(newRoom.roomName + "/RoomBasicsVariant/PlayerSpawn");
        }
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

            if (!roomDef.roomName.StartsWith("Room"))
            {
                Debug.LogWarningFormat("Room name not set for room: {0}", i);
            }

            GameObject roomGameObj = GameObject.Find(roomDef.roomName);

            if (roomGameObj == null)
            {
                Debug.LogWarningFormat("Did not find room: {0}", roomDef.roomName);
            }
        }
    }

    public IEnumerator HandleTransitionAndNextRoom()
    {
        Room newRoom = gameRooms[currentRoom];
        transitionTitle.text = newRoom.transitionText.Equals("") ? DEFAULT_TRANSITION_TEXT: newRoom.transitionText; 
        yield return new WaitForSeconds(delayTransitionTime);

        AnimateRoomTransition(true);
        yield return new WaitForSeconds(transitionTime);

        OnNextRoom();

        yield return new WaitForSeconds(transitionHoldTime);

        AnimateRoomTransition(false);

        OnNewRoomStarted();

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

    // Handle dialogue or tutorial
    private void OnNewRoomStarted()
    {
        StartCoroutine(ShowRoomDialogueAndTutorial());
    }

    private IEnumerator ShowRoomDialogueAndTutorial()
    {
        Room newRoom = gameRooms[currentRoom];
        Timer.Instance.SetTime(newRoom.roomTimer);
        if (newRoom.dialogue != null)
        {
            dialogueText.text = newRoom.dialogue;
            ShowDialogue();
            yield return new WaitForSeconds(3.0f);
        }

        if (!newRoom.tutorial.Equals(""))
        {
            dialogueText.text = newRoom.tutorial;
            //ShowDialogueText(newRoom.tutorial);
            yield return new WaitForSeconds(3.0f);
        }

        if (!newRoom.tutorial2.Equals(""))
        {
            dialogueText.text = newRoom.tutorial2;
            //ShowDialogueText(newRoom.tutorial);
            yield return new WaitForSeconds(3.0f);
        }

        PlayerMovement.Instance.waiting = false;
        Timer.Instance.Resume();

        HideDialogue();
    }

    private void ShowDialogue()
    {
        dialogueUIPanel.transform.DOMoveY(100, 0.75f);
    }

    private void HideDialogue()
    {
        dialogueUIPanel.transform.DOMoveY(-130, 0.75f);
    }
}
