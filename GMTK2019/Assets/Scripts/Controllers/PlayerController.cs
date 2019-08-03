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

    [SerializeField] Transform topRightRaycastOrigin;
    [SerializeField] Transform midRightRaycastOrigin;
    [SerializeField] Transform bottomRightRaycastOrigin;
    [SerializeField] Transform topLeftRaycastOrigin;
    [SerializeField] Transform midLeftRaycastOrigin;
    [SerializeField] Transform bottomLeftRaycastOrigin;

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
        Debug.DrawRay(topRightRaycastOrigin.position, wallRideRaycastVector, Color.red);
        Debug.DrawRay(midRightRaycastOrigin.position, wallRideRaycastVector, Color.red);
        Debug.DrawRay(bottomRightRaycastOrigin.position, wallRideRaycastVector, Color.red);
    }

    void FixedUpdate()
    {
        if (playerState == PlayerState.Running)
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
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
        //print("you died");
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        var scale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        transform.localScale = scale;
    }

    void DrawRaycasts()
    {
        // Debug raycasts
        
        //Debug.DrawRay(topLeftRaycastOrigin.position, -wallRideRaycastVector, Color.red);
        //Debug.DrawRay(midLeftRaycastOrigin.position, -wallRideRaycastVector, Color.red);
        //Debug.DrawRay(bottomLeftRaycastOrigin.position, -wallRideRaycastVector, Color.red);

        var topRightHit = Physics2D.Raycast(topRightRaycastOrigin.position, wallRideRaycastVector, wallRideRaycastVector.x);
        var midRightHit = Physics2D.Raycast(midRightRaycastOrigin.position, wallRideRaycastVector, wallRideRaycastVector.x);
        var bottomRightHit = Physics2D.Raycast(bottomRightRaycastOrigin.position, wallRideRaycastVector, wallRideRaycastVector.x);

        var topLeftHit = Physics2D.Raycast(topLeftRaycastOrigin.position, -wallRideRaycastVector, -wallRideRaycastVector.x);
        var midLeftHit = Physics2D.Raycast(midLeftRaycastOrigin.position, -wallRideRaycastVector, -wallRideRaycastVector.x);
        var bottomLeftHit = Physics2D.Raycast(bottomLeftRaycastOrigin.position, -wallRideRaycastVector, -wallRideRaycastVector.x);

        if ((topRightHit || midRightHit || bottomRightHit)
            && (IsValidWallRideRaycast(topRightHit)
                || IsValidWallRideRaycast(midRightHit)
                || IsValidWallRideRaycast(bottomRightHit)))
        {

            OnAttatchToWall(WallAttatchedState.Right);
        }
        else if ((topLeftHit || midLeftHit || bottomLeftHit)
            && (IsValidWallRideRaycast(topLeftHit)
                || IsValidWallRideRaycast(midLeftHit)
                || IsValidWallRideRaycast(bottomLeftHit)))
        {
            OnAttatchToWall(WallAttatchedState.Left);
        }
    }

    void OnAttatchToWall(WallAttatchedState wall)
    {
        canJump = true;
        ChangeWallAttatchedState(wall);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsJumping", false);
    }

    bool IsValidWallRideRaycast(RaycastHit2D ray)
    {
        //return false;
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
