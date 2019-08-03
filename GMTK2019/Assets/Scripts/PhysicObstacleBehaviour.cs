using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObstacleBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    public float forceToApply;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("MousePlatform"))
        {            
            rb.AddForce(Vector2.right * forceToApply, ForceMode2D.Impulse);
        }
    }
}
