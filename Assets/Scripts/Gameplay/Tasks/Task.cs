using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TaskType
{
    TRASH,
    MOP,
    SWEEP,
    TIDY_ITEM
}

[Serializable]
public class Task 
{
    public TaskType type;
    public string description;
    public List<TaskItem> items;
}
