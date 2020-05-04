using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity;

    float input_h;


    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spriteRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        input_h = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Move();
        Animate();
    }

    void Move()
    {
        Vector2 vel = rb.velocity;
        vel.x = velocity * input_h;
        rb.velocity = vel;
    }

    void Animate()
    {
        if(input_h<0)
        {
            spriteRenderer.flipX = true;
        }
        else if(input_h>0)
        {
            spriteRenderer.flipX = false;
        }
        anim.SetFloat("Velocity_X", Mathf.Abs(input_h));
        anim.SetFloat("Velocity_Y", rb.velocity.y);
    }
}
