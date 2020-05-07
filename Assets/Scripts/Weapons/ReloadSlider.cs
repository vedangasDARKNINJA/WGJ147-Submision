using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadSlider : MonoBehaviour
{
    public SLIDER_IDS id;

    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        GameEvents.current.onReloadValueChanged += OnReloadValueChanged;
    }

    void OnReloadValueChanged(SLIDER_IDS id,float value,bool subtractFromCurrent=false)
    {
        if (this.id == id)
        {
            if (!subtractFromCurrent)
            {
                slider.value = value;
            }
            else
            {
                slider.value -= value;
            }
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onReloadValueChanged -= OnReloadValueChanged;
    }

}
