using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DissolveController : MonoBehaviour
{
    public float dissolveTime=0.3f;
    float timer;

    SpriteRenderer spriteRenderer;
    Collider2D myCollider;
    IEnumerator currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(spriteRenderer.material);
    }

    public void Dissolve(bool toOne=false)
    {
        myCollider.enabled = false;
        spriteRenderer.material.SetFloat("_Fade", toOne ? 0f : 1f);
        if(currentCoroutine!=null)
        {
            StopCoroutine(currentCoroutine);
        }
        timer = toOne ? 0f : dissolveTime;
        currentCoroutine = toOne ? UnDissolve() : Dissolve();
        StartCoroutine(currentCoroutine);
    }


    IEnumerator Dissolve()
    {
        while(timer>=0)
        {
            timer -= Time.deltaTime;
            spriteRenderer.material.SetFloat("_Fade", timer/dissolveTime);
            yield return null;
        }
        
    }

    IEnumerator UnDissolve()
    {
        myCollider.enabled = true;
        while (timer <dissolveTime)
        {
            timer += Time.deltaTime;
            spriteRenderer.material.SetFloat("_Fade", timer / dissolveTime);
            yield return null;
        }
    }
}
