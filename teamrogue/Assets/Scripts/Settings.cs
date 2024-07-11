using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    //list of current settings to adjust
    public Dropdown resDropdown;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider sensSlider;
    public Toggle invertToggle;

    public AudioMixer audioMixer;

    public GameObject musicSource;
    public GameObject sfxSource;

    //array of different resolutions to swap to
    private Resolution[] resolutions;
    
    // Start is called before the first frame update
    void Start()
    {
        //initialize sliders to default or saved values
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.75f);
        //set the initial volumes
        SetMusicVol(musicSlider.value);
        SetSFXVol(sfxSlider.value);
        //listen for slider changes
        musicSlider.onValueChanged.AddListener(SetMusicVol);
        sfxSlider.onValueChanged.AddListener(SetSFXVol);
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
}
