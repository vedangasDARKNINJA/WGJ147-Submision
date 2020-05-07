using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    
    public GameObject[] toActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Activate Objects");
            foreach(GameObject g in toActivate)
            {
                g.SetActive(true);
            }
        }
    }
}
