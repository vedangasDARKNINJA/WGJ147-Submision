using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public Transform crossHair;
    public GameObject bulletPrefab;
    public int bullets;
    public float reloadTime;
    bool reloading;

    private new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(!reloading)
        {

        }
    }

    protected override void Attack()
    {
        anim.SetTrigger("Attack");

    }
}
