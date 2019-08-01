using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region Fields	
    public static SettingsManager instance;

    public AudioMixer audioMixer;
    #endregion

    #region Properties	    
    #endregion

    #region Public methods	
    public void SetMasterVolume(float volume)
    {
        SaveSoundPreference("Master", volume);
        audioMixer.SetFloat("Master", volume);
    }

    public void SetMusicVolume(float volume)
    {
        SaveSoundPreference("Music", volume);
        audioMixer.SetFloat("Music", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        SaveSoundPreference("Effects", volume);
        audioMixer.SetFloat("Effects", volume);
    }

    public void LoadMasterSound()
    {
        var masterVolume = GetSoundPreference("Master");
        audioMixer.SetFloat("Master", masterVolume);
        var sliderMaster = GameObject.Find("SliderMaster");

        if (sliderMaster != null)
        {
            sliderMaster.GetComponent<Slider>().value = masterVolume;
        }
    }

    public void LoadMusicSound()
    {
        var musicVolume = GetSoundPreference("Music");
        audioMixer.SetFloat("Music", musicVolume);
        var sliderMusic = GameObject.Find("SliderMusic");

        if (sliderMusic != null)
        {
            sliderMusic.GetComponent<Slider>().value = musicVolume;
        }
    }

    public void LoadEffectsSound()
    {
        var effectsVolume = GetSoundPreference("Effects");
        audioMixer.SetFloat("Effects", effectsVolume);
        var sliderEffects = GameObject.Find("SliderEffects");

        if (sliderEffects != null)
        {
            sliderEffects.GetComponent<Slider>().value = effectsVolume;
        }
    }

    #endregion

    #region Private methods	
    private void SaveSoundPreference(string soundKey, float volume)
    {
        PlayerPrefs.SetFloat(soundKey, volume);
    }

    private float GetSoundPreference(string soundKey)
    {
        return PlayerPrefs.GetFloat(soundKey);
    }

    private void UpdateSliders()
    {
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
	{
        LoadMasterSound();
        LoadMusicSound();
        LoadEffectsSound();
    }	

	void Update () 
	{
		
	}

    private void OnLevelWasLoaded(int level)
    {   
    }

    #endregion
}
