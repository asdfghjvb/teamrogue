using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject menuButtons;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuSaves;
    [SerializeField] GameObject menuSaveSelect;
    [SerializeField] GameObject credits;
    //list of current settings to adjust
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider sensSlider;
    public Toggle invertToggle;

    public AudioMixer audioMixer;

    public MainMenu menu;
    public GameObject musicSource;
    public GameObject sfxSource;
    public AudioSource previewSource;
    public AudioClip sfxPreview;

    private Coroutine previewCoroutine;


   
    
    // Start is called before the first frame update
    void Start()
    {
        //initialize sliders to default or saved values
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.75f);
        sensSlider.value = PlayerPrefs.GetFloat("sensValue", 1.0f);
        invertToggle.isOn = PlayerPrefs.GetInt("invertY", 0) == 1;
        //set the initial values
        SetMusicVol(musicSlider.value);
        SetSFXVol(sfxSlider.value);
        SetSens(sensSlider.value);
        toggleY(invertToggle.isOn);
        //listen for slider changes
        musicSlider.onValueChanged.AddListener(SetMusicVol);
        sfxSlider.onValueChanged.AddListener(SetSFXVolWithPreview);
        sensSlider.onValueChanged.AddListener(SetSens);
        invertToggle.onValueChanged.AddListener(toggleY);
    }

    public void SetMusicVol(float volume)
    {
        //set the volume in the mixer and save it as player preference
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVol(float volume)
    {
        //set the volume in the mixer and save it as player preference
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        
    }
    public void SetSFXVolWithPreview(float volume)
    {
        //set the volume in the mixer and save it as player preference
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        
        if (previewCoroutine != null)
        {
            StopCoroutine(previewCoroutine);
        }
        previewCoroutine = StartCoroutine(PreviewDelay());
    }

    private IEnumerator PreviewDelay()
    {
        yield return new WaitForSeconds(0.2f);
        previewSource.PlayOneShot(sfxPreview);
    }
    public void SetSens(float sens)
    {
        GameManager.instance.sensitivity = sens;
        PlayerPrefs.SetFloat("sensValue", sens);

    }

    public void toggleY(bool isOn)
    {
        GameManager.instance.invertY = isOn;
        PlayerPrefs.SetInt("invertY", isOn ? 1 : 0);
    }
    
    //close settings menu and return to main menu
    public void ReturnButton()
    {
       
        menuSettings.SetActive(false);
        menuSaves.SetActive(false);
        menuSaveSelect.SetActive(false);
        menuButtons.SetActive(true);
        menu.playClickSound();
    }

    public void OpenSaveMenu()
    {
        GameManager.instance.saveMenuActive = true;
        GameManager.instance.menuActive.SetActive(false);
        menuSaves.SetActive(true);
    }
    public void SaveMenuReturnButton()
    {
        menuSaves.SetActive(false);
        GameManager.instance.saveMenuActive = false;
        
        GameManager.instance.menuActive.SetActive(true);

    }
    public void OpenCredits()
    {
        menuButtons.SetActive(false);
        credits.SetActive(true);
    }
   
}
