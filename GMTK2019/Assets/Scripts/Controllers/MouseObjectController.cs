using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObjectController : MonoBehaviour
{
    #region Fields
    Vector2 mousePosition;
    Vector2 screenToWorld;
    public float offsetX;
    #endregion

    #region Private methods

    void Start()
    {
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        mousePosition = Input.mousePosition;
        screenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = new Vector3(screenToWorld.x - offsetX, screenToWorld.y, 0);

    }

    #endregion
}
