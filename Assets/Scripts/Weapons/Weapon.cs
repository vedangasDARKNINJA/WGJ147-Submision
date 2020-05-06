using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon:MonoBehaviour
{
    protected Animator anim;
    protected abstract void Attack();

    protected void Start()
    {
        anim = GetComponent<Animator>();
    }

}