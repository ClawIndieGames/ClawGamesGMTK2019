using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour
{
    Transform target;
    public Rigidbody2D rb;
    public float speed;
    public float rotationSpeed;
    bool canChase;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    public PolygonCollider2D collider;

    bool isChasing;
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;        
    }

    
    void FixedUpdate()
    {
        if (!canChase)
            return;

        var direction = (Vector2)target.position - rb.position;
        direction.Normalize();
                
        float rotateAmmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmmount * rotationSpeed;
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            canChase = true;
            isChasing = true;
            collider.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")
            || collision.transform.CompareTag("MousePlatform"))
        {
            if (isChasing)
            {
                audioSource.Play();
                spriteRenderer.enabled = false;
                collider.enabled = false;
                Destroy(gameObject, 0.7f);
            }
        }
    }
}
