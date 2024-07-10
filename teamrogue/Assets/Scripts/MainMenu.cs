using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void newGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void loadGame()
    {
        //will load like new game until save system is set up
        SceneManager.LoadScene("SampleScene");
    }

    public void settings()
    {
        GameManager.instance.settingsMenu();
    }

    public void quitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
