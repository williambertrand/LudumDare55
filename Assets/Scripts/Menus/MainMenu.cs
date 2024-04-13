using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SCENES
{
    public static int MENU = 0;
    public static int GAMEPLAY = 1;
    public static int END = 2;
}

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlay()
    {
        SceneManager.LoadScene(SCENES.GAMEPLAY);
    }
}
