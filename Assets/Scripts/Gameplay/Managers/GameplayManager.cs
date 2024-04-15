using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayManager : MonoBehaviour
{

    // Data required between rooms:
    public int currentRoom;
    public int totalTasks;
    public List<Room> gameRooms;
    public GameObject roomHolder;

    [Header("Transitioning Between rooms")]
    public float delayTransitionTime;
    public float transitionTime;
    public float transitionHoldTime;
    private TMP_Text transitionTitle;
    private CanvasGroup roomTransitionUI;

    private CanvasGroup dialogueUIPanel;
    private TMP_Text dialogueText;

    private string DEFAULT_TRANSITION_TEXT = "You have been summoned!";

    private void Awake()
    {
        RoomTasksManager.Instance.onRoomComplete += OnRoomCompleted;
        totalTasks = 0;
        CheckRoomSetup();
        transitionTitle = GameObject.Find("TransitionTitle").GetComponent<TMP_Text>();
        roomTransitionUI = GameObject.Find("RoomTransitionUI").GetComponent<CanvasGroup>();
        dialogueUIPanel = GameObject.Find("DialogueUIPanel").GetComponent<CanvasGroup>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();

    }

    private void OnDestroy()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

        if(GameStats.Instance == null)
        {
            GameObject stats = new GameObject();
            stats.AddComponent<GameStats>();
        }

        GameStats.Instance.totalTasks = 0;

        PlayerMovement.Instance.waiting = true;
        Timer.Instance.Pause();
        GameStats.Instance.didWin = false;
        Timer.Instance.onTimerFinished += OnTimerExpire;
        if (GameStats.Instance.didResume)
        {
            GameStats.Instance.didResume = false;
            currentRoom = GameStats.Instance.roomsCleared;
        }
        StartCoroutine(HandleTransitionAndNextRoom(false));
        Room newRoom = gameRooms[currentRoom];
        RoomTasksManager.Instance.OnNewRoom(newRoom);
    }

    public void OnPlayStart()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdatePlayerPos(Vector3 pos)
    {
        PlayerController.Instance.gameObject.transform.position = pos;
    }

    public void OnRoomCompleted(List<Task> tasks)
    {
        PlayerMovement.Instance.waiting = true;
        GameStats.Instance.totalTasks += tasks.Count;
        Timer.Instance.Pause();

        if (currentRoom == gameRooms.Count - 1)
        {
            // On game completed!
            GameStats.Instance.didWin = true;
            StartCoroutine(HandleTransitionToGameOver());
            return;
        }

        // Update score
        currentRoom += 1;

        // Show transition ui, update player pos, and then hide transition ui
        StartCoroutine(HandleTransitionAndNextRoom(true));
    }

    public void OnTimerExpire()
    {
        Debug.Log("TIMER EXPIRED FROM GAMEMANAGER");
        GameStats.Instance.didWin = false;
        GameStats.Instance.roomsCleared = currentRoom;
        StartCoroutine(HandleTransitionToGameOver());
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

    public IEnumerator HandleTransitionAndNextRoom(bool fadeIN)
    {
        Room newRoom = gameRooms[currentRoom];
        transitionTitle.text = newRoom.transitionText.Equals("") ? DEFAULT_TRANSITION_TEXT: newRoom.transitionText;
        if (fadeIN)
            yield return new WaitForSeconds(delayTransitionTime);

        AnimateRoomTransition(true, fadeIN);
        yield return new WaitForSeconds(transitionTime);

        OnNextRoom();

        yield return new WaitForSeconds(transitionHoldTime);

        AnimateRoomTransition(false, false);

        OnNewRoomStarted();

    }

    private void AnimateRoomTransition(bool shown, bool fadeIn)
    {
        
        if(shown)
        {
            roomTransitionUI.alpha = 0;
            roomTransitionUI.DOFade(1, fadeIn ? transitionTime : 0.0f);
        } else
        {
            roomTransitionUI.alpha = 1;
            roomTransitionUI.DOFade(0, transitionTime);
        }
    }

    public IEnumerator HandleTransitionToGameOver()
    {
        yield return new WaitForSeconds(0.5f);
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
            yield return new WaitForSeconds(3.5f);
        }

        if (!newRoom.tutorial.Equals(""))
        {
            dialogueText.text = newRoom.tutorial;
            //ShowDialogueText(newRoom.tutorial);
            yield return new WaitForSeconds(4.0f);
        }

        if (newRoom.tutorial2 != null && !newRoom.tutorial2.Equals(""))
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
        dialogueUIPanel.transform.DOMoveY(130, 0.75f);
    }

    private void HideDialogue()
    {
        dialogueUIPanel.transform.DOMoveY(-130, 0.75f);
    }

    public void Clear()
    {
        totalTasks = 0;
        GameStats.Instance.didWin = false;
    }

    public void Resume()
    {
        GameStats.Instance.didWin = false;
        currentRoom = GameStats.Instance.roomsCleared;
    }
}
