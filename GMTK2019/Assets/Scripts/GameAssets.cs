using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }

            return instance;
        }
    }


    #region Fields
    public Transform damageIndicator;
    #endregion

    #region Public methods
    #endregion

    #region Private methods

    #endregion
}
