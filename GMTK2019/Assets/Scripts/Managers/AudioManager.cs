using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour 
{
    #region Fields	
    public Sound[] sounds;
    public static AudioManager instance;    
    #endregion

    #region Constants
    
    #endregion

    #region Public methods

    public void PlaySound(string soundName)
    {
        if (string.IsNullOrEmpty(soundName) || !sounds.Any(s => s.name == soundName))
        {
            Debug.LogError("** sound: " +soundName+ " invalid **");
            return;
        }

        var soundToPlay = sounds.Where(s => s.name == soundName).FirstOrDefault();
        soundToPlay.audioSource.Play();        
    }

    public IEnumerator FadeOutCoroutine(string soundName)
    {
        var sound = sounds.Where(s => s.name == soundName).FirstOrDefault();

        if (sound == null)
        {
            Debug.LogError("** sound: " + soundName + " invalid **");
            yield return null;
        }

        var startVolume = sound.audioSource.volume;

        while (sound.audioSource.volume > 0)
        {
            sound.audioSource.volume -= startVolume * Time.deltaTime / sound.fadeTime;
            yield return null;
        }

        sound.audioSource.Stop();
        sound.audioSource.volume = startVolume;
    }

    public void StopSound(string soundName)
    {
        var sound = sounds.Where(s => s.name == soundName).FirstOrDefault();

        if (sound == null)
        {
            Debug.LogError("** sound: " + soundName + " invalid **");
            return;
        }

        sound.audioSource.Stop();
    }

    public void StopAllSounds(bool isToFadeOut)
    {
        foreach (var sound in sounds)
        {
            if (isToFadeOut)
            {
                StartCoroutine(FadeOutCoroutine(sound.name));
            }
            else
            {
                sound.audioSource.Stop();
            }
        }
    }
    
    #endregion

    #region Private methods	
    void Awake()
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

        foreach (var sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.playOnAwake = sound.playOnAwake;
            sound.audioSource.outputAudioMixerGroup = sound.audioMixerGroup;

            if (sound.playOnAwake)
            {
                sound.audioSource.Play();
            }
        }
    }

    void Start () 
	{       
    }	

	void Update () 
	{        
	}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
    #endregion
}
