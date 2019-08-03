using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region fields
    public enum PlayerState
    {
        Running,
        Jumping,
        WallRide
    }

    PlayerState playerState;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] float jumpForce;
    [SerializeField] float movementSpeed;

    int maxHealth = 100;
    float currentHealth;
    [SerializeField] float damageDoneByPlatform;
    [SerializeField] float passiveHealing;

    bool canJump;
    bool isLosingHealth;

    public static event Action<float> OnPlayerLosingHealth = delegate { };
    public static event Action<float> OnPlayerHealing = delegate { };

    #endregion

    #region PrivateMethods

    void Start()
    {
        currentHealth = maxHealth;   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            
        }

        if (isLosingHealth)
        {
            if (currentHealth > 0)
            {
                currentHealth -= damageDoneByPlatform;
                OnPlayerLosingHealth(damageDoneByPlatform);
            }
        }
        else
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += passiveHealing;
                OnPlayerHealing(passiveHealing);
            }
        }
        if (currentHealth <= 0)
        {
            Die();
        }

    }

    void FixedUpdate()
    {
        if (playerState == PlayerState.Running)
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);            
        }
    }

    void Jump()
    {
        if (!canJump)
            return;
        
        canJump = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void Die()
    {
        print("you died");
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            isLosingHealth = true;
        }        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
            isLosingHealth = false;
        }
    }
}
