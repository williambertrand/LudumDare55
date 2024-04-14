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

        OnNextRoom();
        OnNewRoomStarted();
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
        Room newRoom = gameRooms[currentRoom];
        transitionTitle.text = newRoom.transitionText || DEFAULT_TRANSITION_TEXT; 
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
        if (newRoom.dialogue != null)
        {
            dialogueText.text = newRoom.dialogue;
            ShowDialogue();
            yield return new WaitForSeconds(3.0f);
        }

        if (newRoom.tutorial != null)
        {
            dialogueText.text = newRoom.tutorial;
            //ShowDialogueText(newRoom.tutorial);
            yield return new WaitForSeconds(3.0f);
        }

        HideDialogue();
    }

    private void ShowDialogue()
    {
        dialogueUIPanel.transform.DOMoveY(75, 0.75f);
    }

    private void HideDialogue()
    {
        dialogueUIPanel.transform.DOMoveY(-130, 0.75f);
    }
}
