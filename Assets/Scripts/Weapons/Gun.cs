using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : Weapon
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    [SerializeField]
    float timer;
    
    public override void Attack()
    {
        anim.SetTrigger("Attack");
        Vector2 dir = crossHair.position - transform.position;

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();
        b.velocity = dir.normalized;
        timer = 0;
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        reloading = true;
        while (timer < reloadTime)
        {
            timer += Time.deltaTime;
            GameEvents.current.ReloadValueChanged(SLIDER_IDS.RELOAD_BAR,Mathf.Clamp01(timer/reloadTime));
            yield return null;
        }
        reloading = false;
    }
}
