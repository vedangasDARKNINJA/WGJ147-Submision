using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegenerateHealth : MonoBehaviour
{
    public float regenRate;
    public float hitanimationTime = 0.5f;
    float prevValue;
    Slider slider;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        anim = GetComponentInChildren<Animator>();
        prevValue = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value<prevValue)
        {
            prevValue =slider.value;
            StartCoroutine(HitAnimation());
        }

        if (slider.value <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        if (slider.value <1)
        {
            slider.value += regenRate * Time.deltaTime;
        }

    }

    IEnumerator HitAnimation()
    {
        anim.SetBool("Hit", true);
        yield return new WaitForSeconds(hitanimationTime);
        anim.SetBool("Hit", false);
    }
}
