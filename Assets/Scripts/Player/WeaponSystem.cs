using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public GameObject[] weaponPrefabs;
    
    List<Weapon> weapons = new List<Weapon>();
    int currWeaponIndex = 0;

    public Transform crossHair;

    PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerController>();
        currWeaponIndex = 0;
        for(int i=0;i<weaponPrefabs.Length;i++)
        {
            GameObject g = Instantiate(weaponPrefabs[i], transform.position, Quaternion.identity, transform);
            weapons.Add(g.GetComponent<Weapon>());
            weapons[i].crossHair = crossHair;
            if (i!=currWeaponIndex)
            {
                g.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!weapons[currWeaponIndex].reloading)
        {
            if (Input.GetMouseButtonDown(0) && pc.CanFire())
            {
                weapons[currWeaponIndex].Attack();
            }
        }
    }
}
