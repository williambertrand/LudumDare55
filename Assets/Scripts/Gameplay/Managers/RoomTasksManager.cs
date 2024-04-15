using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RoomTasksManager : MonoSingleton<RoomTasksManager>
{

    public List<Task> currentRoomTasks;
    private int currentTaskCount;
    private int roomTaskCount;

    public delegate void OnTaskComplete();
    OnTaskComplete onTaskComplete;

    public delegate void OnRoomComplete(List<Task> tasks);
    public OnRoomComplete onRoomComplete;

    public TMP_Text taskCounterText;
    public CanvasGroup roomCompleteUI;

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
      
    }

    public void OnTaskWasCompleted()
    {
        onTaskComplete?.Invoke();
        currentTaskCount += 1;
        UpdateUI();

        if(currentTaskCount == roomTaskCount)
        {
            roomCompleteUI.DOFade(1, 0.5f);
            onRoomComplete?.Invoke(currentRoomTasks);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNewRoom(Room r)
    {
        currentTaskCount = 0;
        currentRoomTasks = r.tasks;
        roomTaskCount = r.numTasks;
        roomCompleteUI.DOFade(0, 0.5f);
        UpdateUI();
    }

    void UpdateUI()
    {
        if(taskCounterText != null)
            taskCounterText.text = string.Format("Tasks Complete: {0}/{1}", currentTaskCount, roomTaskCount);
    }
}
