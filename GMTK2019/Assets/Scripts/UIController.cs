using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region Fields
    [SerializeField] Transform healthBar;
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
    }

    void UnSubscribeEvents()
    {
        PlayerController.OnPlayerLosingHealth -= OnPlayerLostHealth;
        PlayerController.OnPlayerHealing -= OnPlayerHealed;

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
    #endregion
}
