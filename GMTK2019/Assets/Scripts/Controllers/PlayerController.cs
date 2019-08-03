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

    public enum WallAttatchedState
    {
        None,
        Left,
        Right
    }

    PlayerState playerState;
    WallAttatchedState wallAttatchedState = WallAttatchedState.None;

    [SerializeField] Rigidbody2D rb;

    int maxHealth = 100;

    float currentHealth;

    [SerializeField] float jumpForce;
    [SerializeField] float movementSpeed;
    [SerializeField] float damageDoneByPlatform;
    [SerializeField] float passiveHealing;

    [SerializeField] Vector2 wallRideRaycastVector;

    bool canJump;
    bool isLosingHealth;
    bool isFacingRight = true;

    [SerializeField] Animator animator;

    [SerializeField] Transform topRaycastOrigin;
    [SerializeField] Transform midRaycastOrigin;
    [SerializeField] Transform bottomRaycastOrigin;    

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
        if (wallAttatchedState == WallAttatchedState.Left)
        {
            if (!isFacingRight)
                Flip();

            animator.SetBool("IsWallRiding", true);
        }
        else if (wallAttatchedState == WallAttatchedState.Right)
        {
            if (isFacingRight)
                Flip();

            animator.SetBool("IsWallRiding", true);
        }
        else if (wallAttatchedState == WallAttatchedState.None)
        {
            animator.SetBool("IsWallRiding", false);
        }

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

        if (playerState == PlayerState.Running)
        {
            animator.SetBool("IsRunning", true);
        }
    }

    void FixedUpdate()
    {
        
        if (playerState == PlayerState.Running)
        {
            if (isFacingRight)
            {
                rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
            }
        }
    }

    void ChangePlayerState(PlayerState stateToApply)
    {
        playerState = stateToApply;
    }

    void ChangeWallAttatchedState(WallAttatchedState wall)
    {
        wallAttatchedState = wall;
    }

    void Jump()
    {
        if (!canJump)
            return;

        animator.SetBool("IsJumping", true);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWallRiding", false);
        ChangePlayerState(PlayerState.Jumping);
        canJump = false;

        var directionToJump = Vector2.zero;
        switch (wallAttatchedState)
        {
            case WallAttatchedState.None:
                directionToJump = new Vector2(rb.velocity.x, jumpForce);
                break;

            case WallAttatchedState.Left:                
                directionToJump = new Vector2(10, jumpForce);
                break;

            case WallAttatchedState.Right:                
                directionToJump = new Vector2(-10, jumpForce);
                break;

            default:
                break;
        }

        rb.velocity = directionToJump;
    }

    void Die()
    {
        print("you died");
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        var scale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        transform.localScale = scale;
    }

    void DrawRaycasts()
    {
        var vectorDirectionToRaycast = isFacingRight ? wallRideRaycastVector : - wallRideRaycastVector;

        var topHit = Physics2D.Raycast(topRaycastOrigin.position, wallRideRaycastVector, vectorDirectionToRaycast.x);
        var midHit = Physics2D.Raycast(midRaycastOrigin.position, wallRideRaycastVector, vectorDirectionToRaycast.x);
        var bottomHit = Physics2D.Raycast(bottomRaycastOrigin.position, wallRideRaycastVector, vectorDirectionToRaycast.x);

        if ((topHit || midHit || bottomHit)
            && (IsValidWallRideRaycast(topHit)
                || IsValidWallRideRaycast(midHit)
                || IsValidWallRideRaycast(bottomHit)))
        {
            if (isFacingRight)
            {
                OnAttatchToWall(WallAttatchedState.Right);
            }
            else
            {
                OnAttatchToWall(WallAttatchedState.Left);
            }
        }        
    }

    void OnAttatchToWall(WallAttatchedState wall)
    {
        canJump = true;
        ChangeWallAttatchedState(wall);
        ChangePlayerState(PlayerState.WallRide);

        animator.SetBool("IsRunning", false);
        animator.SetBool("IsJumping", false);

        if (wall == WallAttatchedState.Right)
        {
            Flip();
        }
        else if (wall == WallAttatchedState.Left)
        {
            Flip();
        }
    }

    bool IsValidWallRideRaycast(RaycastHit2D ray)
    {
        if (ray.transform != null)
        {
            return ray.transform.CompareTag("VerticalWall");
        }

        return false;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            isLosingHealth = true;
            animator.SetBool("IsJumping", false);

            if (wallAttatchedState == WallAttatchedState.None)
            {
                ChangePlayerState(PlayerState.Running);
            }
        }

        if (collision.gameObject.CompareTag("VerticalWall"))
        {
            if (playerState == PlayerState.Running)
            {
                Die();
            }

            DrawRaycasts();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
            isLosingHealth = false;
        }

        if (collision.gameObject.CompareTag("VerticalWall"))
        {            
            ChangeWallAttatchedState(WallAttatchedState.None);
        }
    }
}
