
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour 
{
    #region	Fields
    public static GameManager instance = null;

    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    bool canPlayLevelTheme;

    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        Loading
    }

    private GameState gameStateValue;
    #endregion

    
    #region Public Methods
    /// <summary>
    /// Toggles the time, paused or normal.
    /// </summary>
    /// <param name="isToStopTime">Flag to indicate if it is to pause the game.</param>    
    public void ToggleStopTime(bool isToStopTime)
    {
        Time.timeScale = isToStopTime ? 0 : 1;
        gameStateValue = isToStopTime ? GameState.Paused : GameState.Playing;
    }

    /// <summary>
    /// Indicates if the gamestate is "playing"
    /// </summary>
    /// <returns>True if is playing.</returns>
    public bool IsGameStatePlaying()
    {
        return gameStateValue == GameState.Playing;
    }

    /// <summary>
    /// Indicates if the gamestate is "paused"
    /// </summary>
    /// <returns>True if is paused.</returns>
    public bool IsGameStatePaused()
    {
        return gameStateValue == GameState.Paused;
    }
       
    public bool IsGameStateGameOver()
    {
        return gameStateValue == GameState.GameOver;
    }


    public void SetState(GameState stateToSet)
    {
        gameStateValue = stateToSet;
    }

    public GameState GetState()
    {
        return gameStateValue;
    }

    /// <summary>
    /// Resets the level
    /// </summary>
    /// <param name="levelName">In case there is a specific level to reset.</param>
    public void ResetLevel(string levelName = "")
    {
        SetState(GameState.Playing);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion


    #region Private Methods
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
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            audioSource.volume = 0.115f;
            PlaySound("MainMenuTheme");
        } 
    }

    IEnumerator LoadLevelAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
    }

    void PlaySound(string name)
    {
        audioSource.Stop();
        audioSource.clip = audioClips.Where(m => m.name == name).First();
        audioSource.Play();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1"
            || scene.name == "Level2"
            || scene.name == "Level3")
        {
            if (canPlayLevelTheme)
            {
                audioSource.volume = 0.03f;
                PlaySound("LevelTheme");
                canPlayLevelTheme = false;
            }
        }

        if (scene.name == "MainMenu" || scene.name == "Intro")
        {
            canPlayLevelTheme = true;
        }
    }
    
    #endregion

}
