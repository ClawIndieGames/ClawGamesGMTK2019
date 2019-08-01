using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    #region Fields	
    [HideInInspector]
    public AudioSource audioSource;

    public string name;

    public AudioClip clip;

    [Range(0,1)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    [Range(0.1f, 5f)]
    public float fadeTime;
    
    public bool loop;

    public bool playOnAwake;

    public AudioMixerGroup audioMixerGroup;
    #endregion

    #region Properties	
    #endregion

    #region Public methods	
    #endregion

    #region Private methods	
    void Awake()
    {        
    }

    void Start () 
	{
		
	}	

	void Update () 
	{
		
	}
	#endregion	
}
