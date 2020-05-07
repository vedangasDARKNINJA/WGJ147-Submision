using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainToggleAnim : MonoBehaviour
{
    AutomatedPlatform ap;
    Animator anim;
    [SerializeField]
    int id;
    // Start is called before the first frame update
    void Start()
    {
        ap = GetComponentInParent<AutomatedPlatform>();
        id = ap.switchId;
        Debug.Log("ID: " + id);
        anim = GetComponent<Animator>();
        GameEvents.current.onSwitchStateChanged += OnSwitchStateChange;
    }

    void OnSwitchStateChange(int id,bool state)
    {

        if(this.id == id)
        {
            anim.SetBool("activated", state);
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onSwitchStateChanged -= OnSwitchStateChange;
    }
}
