using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity;
    public float smooothingTime;
    Vector3 smoothingVelocity;

    float input_h;


    Rigidbody2D rb;
    Animator anim;
    public Transform crosshair;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        input_h = Input.GetAxisRaw("Horizontal");
        Animate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 targetVelocity = new Vector2(input_h * velocity*Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref smoothingVelocity, smooothingTime);
    }

    void Animate()
    {
        if(input_h<0)
        {
            transform.localScale = -2 * Vector3.right + Vector3.one;
        }
        else if(input_h>0)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(crosshair.position.x - transform.position.x);
            transform.localScale = scale;
        }
        anim.SetFloat("Velocity_X", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Velocity_Y", rb.velocity.y);
    }


    public bool CanFire()
    {
        return Mathf.Sign(crosshair.position.x - transform.position.x) == transform.localScale.x;
    }
}
