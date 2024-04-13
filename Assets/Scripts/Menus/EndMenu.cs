using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour
{

    public TMP_Text titleText;
    public TMP_Text totalText;


    void Start()
    {
        titleText.text = getTitleTextForGameState();
        totalText.text = "" + GameplayManager.Instance.totalTasks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string getTitleTextForGameState()
    {
        if(GameplayManager.Instance.didWin)
        {
            return "Great Job!";
        } else
        {
            return "You ran out of time!";
        }
    }

    private string getDescriptionTextForGameState()
    {
        if (GameplayManager.Instance.didWin)
        {
            return "Great Job!";
        }
        else
        {
            return "You ran out of time!";
        }
    }
}
