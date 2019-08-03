using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObjectController : MonoBehaviour
{
    #region Fields
    Vector2 screenToWorld;
    public Rigidbody2D rb;    
    
    #endregion

    #region Private methods

    void Start()
    {
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        screenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var positionToMove = Vector3.MoveTowards(transform.position, screenToWorld, 100 * Time.fixedDeltaTime);

        rb.MovePosition(positionToMove);
    }
   
    #endregion    
}
