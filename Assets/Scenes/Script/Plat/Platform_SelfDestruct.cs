using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_SelfDestruct : Platform
{

    [SerializeField] float FallingDelay;
    [SerializeField] float DelayRespawn;
    

    private Rigidbody2D Rb;
    private bool IsGonnaFall;
    private Vector2 StartPosition;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        Rb = GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Touche");
        if (collision.gameObject.CompareTag("Player") && !IsGonnaFall) 
        {
            print("dedans");
            IsGonnaFall = true; //is gonna fall
            Invoke("Fall", FallingDelay);
        }

    }

    private void Fall()
    {
        ColPlatform.enabled = false;
        Rb.bodyType = RigidbodyType2D.Dynamic;
        Invoke("Respawn", DelayRespawn);
    }

    /*private*/ void Respawn()
    {
        IsGonnaFall = false;
        Rb.velocity = Vector2.zero;
        Rb.angularVelocity = 0;
        Rb.bodyType = RigidbodyType2D.Static;
        ColPlatform.enabled = true;
        transform.position = StartPosition; 
    }

}


