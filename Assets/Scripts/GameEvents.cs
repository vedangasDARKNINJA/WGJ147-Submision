using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public event Action<int, bool> onSwitchStateChanged;
    public void SwitchStateChanged(int id,bool state)
    {
        onSwitchStateChanged(id,state);
    }

    public event Action<float> onReloadValueChanged;
    public void ReloadValueChanged(float value)
    {
        onReloadValueChanged(value);
    }

}
