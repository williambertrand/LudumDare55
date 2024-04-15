using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "ScriptableObjects/RoomScriptableObject", order = 1)]
public class Room : ScriptableObject
{
    public string roomName;
    public string dialogue;
    public string tutorial;
    public string tutorial2;
    public string transitionText;
    public int numTasks;
    public float roomTimer;
    public System.Collections.Generic.List<Task> tasks;
}
