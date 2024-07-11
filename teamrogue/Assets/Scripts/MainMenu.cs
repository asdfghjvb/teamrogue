using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuButtons;
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
        //close main menu and open settings
        StartCoroutine(OpenSettingsDelay());
        
    }

    private IEnumerator OpenSettingsDelay()
    {
        playClickSound();
        yield return new WaitForSeconds(0.1f);
        menuSettings.SetActive(true);
        menuButtons.SetActive(false);
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
