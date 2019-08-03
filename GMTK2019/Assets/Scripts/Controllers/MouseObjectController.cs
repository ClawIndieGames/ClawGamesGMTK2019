using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObjectController : MonoBehaviour
{
    #region Fields
    Vector2 screenToWorld;
    
    #endregion

    #region Private methods

    void Start()
    {
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        screenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        transform.position = Vector3.MoveTowards(transform.position, screenToWorld, 200 * Time.fixedDeltaTime);
    }

    #endregion    
}
