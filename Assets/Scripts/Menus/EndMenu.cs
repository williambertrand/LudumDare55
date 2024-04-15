using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{

    public TMP_Text titleText;
    public TMP_Text totalText;


    void Start()
    {
        titleText.text = getTitleTextForGameState();
        totalText.text = "" + GameStats.Instance.totalTasks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string getTitleTextForGameState()
    {
        if(GameStats.Instance.didWin)
        {
            return "Great Job!";
        } else
        {
            return "You ran out of time!";
        }
    }

    private string getDescriptionTextForGameState()
    {
        if (GameStats.Instance.didWin)
        {
            return "Great Job!";
        }
        else
        {
            return "You ran out of time!";
        }
    }

    public void OnPlayAgain()
    {
        SceneManager.LoadScene(SCENES.GAMEPLAY);
    }

    public void OnResume()
    {
        GameStats.Instance.didResume = true;
        SceneManager.LoadScene(SCENES.GAMEPLAY);
    }
    public void OnMenu()
    {
        SceneManager.LoadScene(SCENES.MENU);
    }
}
