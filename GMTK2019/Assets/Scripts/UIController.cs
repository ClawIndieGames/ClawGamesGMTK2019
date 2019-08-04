using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    #region Fields
    [SerializeField] Transform healthBar;
    [SerializeField] GameObject menu;
    #endregion

    #region Private methods


    void Start()
    {
        SubscribeEvents();
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        UnSubscribeEvents();
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    void SubscribeEvents()
    {
        PlayerController.OnPlayerLosingHealth += OnPlayerLostHealth;
        PlayerController.OnPlayerHealing += OnPlayerHealed;
        PlayerController.OnPlayerFinishLevel += OnPlayerFinishLevel;
    }

    void UnSubscribeEvents()
    {
        PlayerController.OnPlayerLosingHealth -= OnPlayerLostHealth;
        PlayerController.OnPlayerHealing -= OnPlayerHealed;
        PlayerController.OnPlayerFinishLevel -= OnPlayerFinishLevel;

    }

    void OnPlayerLostHealth(float damage)
    {
        healthBar.localScale = new Vector2(
            healthBar.localScale.x - (damage / 100f),
            healthBar.localScale.y);
    }

    void OnPlayerHealed(float heal)
    {
        healthBar.localScale = new Vector2(
            healthBar.localScale.x + (heal/ 100f),
            healthBar.localScale.y);
    }

    void OnPlayerFinishLevel()
    {
        Cursor.visible = true;
        menu.SetActive(true);
    }
    #endregion

    #region Public methods
    public void LoadNextLevel()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex +1;
        SceneManager.LoadScene(nextSceneIndex);        
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);        
    }
    #endregion
}
