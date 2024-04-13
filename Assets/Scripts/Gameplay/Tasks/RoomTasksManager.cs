using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomTasksManager : MonoSingleton<RoomTasksManager>
{

    public List<Task> currentRoomTasks;
    private int currentTaskCount;

    delegate void OnTaskComplete();
    OnTaskComplete onTaskComplete;

    delegate void OnRoomComplete(List<Task> tasks);
    OnRoomComplete onRoomComplete;

    public TMP_Text taskCounterText;
    public GameObject roomCompleteUI;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Task task in currentRoomTasks)
        {
            if(task.items != null && task.items.Count > 0)
            {
                foreach(TaskItem item in task.items)
                {
                    //Instantiate(item.obj, item.position);
                }
            }
        }
      

        OnNewRoom();
    }

    public void OnTaskWasCompleted()
    {
        onTaskComplete?.Invoke();
        currentTaskCount += 1;
        UpdateUI();

        if(currentTaskCount == currentRoomTasks.Count)
        {
            roomCompleteUI.SetActive(true);
            onRoomComplete?.Invoke(currentRoomTasks);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Task> loadRoomTasks()
    {

        List<Task> testTasks = new List<Task>();
        testTasks.Add(new Task());
        testTasks.Add(new Task());
        return testTasks;
    }

    void OnNewRoom()
    {
        currentTaskCount = 0;
        currentRoomTasks = loadRoomTasks();
        roomCompleteUI.SetActive(false);
        UpdateUI();
    }

    void UpdateUI()
    {
        taskCounterText.text = string.Format("Tasks: {0}/{1}", currentTaskCount, currentRoomTasks.Count);
    }
}
