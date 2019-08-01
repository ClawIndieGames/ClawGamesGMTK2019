using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLoader : MonoBehaviour 
{
    public GameManager gameManager;    

    public AudioManager audioManager;

    public SettingsManager settingsManager;

    private void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
     
        if (AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }

        if (SettingsManager.instance == null)
        {
            Instantiate(settingsManager);
        }
    }
}
