using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    Image image;
    IEnumerator current;
    public float fadeTime = 1f;
    float timer;

    Color transparent = new Color(0,0,0,0);
    Color Solid = new Color(0,0,0,1);

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        GameEvents.current.onFadeStateChange += OnFadeChange;
    }

    void OnFadeChange(bool fadeIn)
    {
        Debug.Log("Fade Change");
        if(current!=null)
        {
            StopCoroutine(current);
        }
        timer = 0;
        image.enabled = true;
        current = Fade(fadeIn?Solid:transparent);
        StartCoroutine(current);
    }

    IEnumerator Fade(Color target)
    {
        while (image.color != target)
        {
            timer += Time.deltaTime;
            image.color = Color.Lerp(image.color, target, timer / fadeTime);
            yield return null;
        }
        if (target.a == 0)
        {
            image.enabled = false;
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onFadeStateChange -= OnFadeChange;
    }
}
