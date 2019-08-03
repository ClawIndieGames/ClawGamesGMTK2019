using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObjectController : MonoBehaviour
{
    #region Fields
    Vector2 screenToWorld;
    public Rigidbody2D rb;
    public float moveSpeed;
    
    #endregion

    #region Private methods

    void Start()
    {
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        screenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var x = Vector3.MoveTowards(transform.position, screenToWorld, 200 * Time.fixedDeltaTime);

        rb.MovePosition(x);
        //UpdatePosition();

    }

    private void UpdatePosition()
    {
        screenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 mousePosition = MouseManager.Instance.GetMousePositionInWorld();

        Vector2 targetPostion = Vector2.MoveTowards(this.transform.position,
                                                    screenToWorld,
                                                    moveSpeed * Time.deltaTime);

        /* 
         * (Using this instead of setting this.transform.position causes 
         *  less glitchiness) 
         */
        // Set the position of this object based on the mouse's world position
        rb.MovePosition(targetPostion);
    }
    #endregion    
}
