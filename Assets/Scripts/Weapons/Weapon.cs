using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon:MonoBehaviour
{
    
    protected Animator anim;
    public bool reloading;
    public float reloadTime;
    public Transform crossHair;
    public abstract void Attack();

    protected void Start()
    {
        anim = GetComponent<Animator>();
    }

}