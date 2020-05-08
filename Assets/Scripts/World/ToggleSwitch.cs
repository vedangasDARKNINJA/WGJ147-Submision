using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField]
    int id;
    
    bool switchActive;

    public bool initialStateActive;
    public bool facingRight;


    bool prevState;
    Animator anim;

    bool flipX = false;
    bool once = true;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        flipX = Equation2(facingRight, initialStateActive);
        Vector3 scale = transform.localScale;
        scale.x = flipX ? -1 : 1;
        transform.localScale = scale;

        anim.SetBool("Left", switchActive);
        if (once)
        {
            switchActive = initialStateActive;
            prevState = switchActive;
            GameEvents.current.SwitchStateChanged(id, switchActive);
            once = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            bool touchingRight;
            if (collision.transform.position.x < transform.position.x)
            {
                touchingRight = false;
            }
            else
            {
                touchingRight = true;
            }
            switchActive = Equation1(facingRight, initialStateActive, touchingRight);
            if (prevState != switchActive)
            {
                prevState = switchActive;
                GameEvents.current.SwitchStateChanged(id, switchActive);
            }
        }
    }

    bool Equation1(bool facingRight, bool active, bool touchingRight)
    {
        bool t1 = ((!facingRight) && (!active) && (!touchingRight));
        bool t2 = ((!facingRight) && active && touchingRight);
        bool t3 = (facingRight && (!active) && touchingRight);
        bool t4 = (facingRight && active && (!touchingRight));
        return (t1 || t2 || t3 || t4);
    }

    bool Equation2(bool facingRight,bool output)
    {
        return (!facingRight && !output) || (facingRight && output);
    }
}
