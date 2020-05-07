using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadSlider : MonoBehaviour
{
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        GameEvents.current.onReloadValueChanged += OnReloadValueChanged;
    }

    void OnReloadValueChanged(float value)
    {
        slider.value = value;
    }

}
