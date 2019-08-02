
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    #region	Fields
    public static GameManager instance = null;
    public float varJ;
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
    }

    private void Start()
    {
      // If the game has Google integration the  PlayGamesManager LOGIN should be called here
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

    private void OnApplicationQuit()
    {
        // If the game has Google integration the  PlayGamesManager LOGOUT should be called here
    }
    #endregion

}
