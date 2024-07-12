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

        menuButtons.SetActive(true);
        menuSettings.SetActive(false);
      
    }

    public void NewGame()
    {
        playClickSound(); 
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadGame()
    {
        playClickSound();
        //will load like new game until save system is set up
        SceneManager.LoadScene("SampleScene");
    }

    public void Settings()
    {
        //close main menu and open settings
        StartCoroutine(OpenSettingsDelay());

        //if (menuSettings.activeInHierarchy)
        //{
        //    Debug.LogWarning("Menu did not open");
        //}
       //playClickSound();
       // yield return new WaitForSeconds(0.1f);
        menuSettings.SetActive(true);
        menuButtons.SetActive(false);

    }

    private IEnumerator OpenSettingsDelay()
    {
        playClickSound();
        yield return new WaitForSeconds(0.5f);
        //menuSettings.SetActive(true);
        //menuButtons.SetActive(false);
    }

    public void QuitGame()
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
    {
        playClickSound();
        yield return new WaitForSeconds(1f);

    }
}
