using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuSaves;
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
        StartCoroutine(clickDelay());
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadGame()
    {
        StartCoroutine(OpenSettingsDelay());

        menuSaves.SetActive(true);
        menuButtons.SetActive(false);
    }

    public void Settings()
    {
        //close main menu and open settings
        StartCoroutine(OpenSettingsDelay());

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
        StartCoroutine(clickDelayAndQuit());
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
      
        yield return new WaitForSeconds(1f);

    }
    IEnumerator clickDelayAndQuit()
    {

        yield return new WaitForSeconds(.2f);
    }
}
