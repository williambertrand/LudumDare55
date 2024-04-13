using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "ScriptableObjects/RoomScriptableObject", order = 1)]
public class Room : ScriptableObject
{
    public string roomName;
    public int numTasks;
    public System.Collections.Generic.List<Task> tasks;
}
