using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClick;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void newGame()
    {
        playClickSound(); 
        SceneManager.LoadScene("SampleScene");
    }

    public void loadGame()
    {
        playClickSound();
        //will load like new game until save system is set up
        SceneManager.LoadScene("SampleScene");
    }

    public void settings()
    {
        playClickSound();
        GameManager.instance.settingsMenu();
    }

    public void quitGame()
    {
        StartCoroutine(clickDelay());
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void playClickSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }

    IEnumerator clickDelay()
    {playClickSound();
        yield return new WaitForSeconds(0.1f);

    }
}
