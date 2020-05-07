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
        
        timer = toOne ? 0f : dissolveTime;
        if(currentCoroutine!=null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = toOne ? UnDissolve() : Dissolve();
        StartCoroutine(currentCoroutine);
    }


    IEnumerator Dissolve()
    {
        while(Mathf.Clamp01(timer / dissolveTime) > 0)
        {
            Debug.Log("in dissolve");
            timer -= Time.fixedDeltaTime;
            timer = Mathf.Clamp(timer, 0, float.MaxValue);
            spriteRenderer.material.SetFloat("_Fade", Mathf.Clamp01(timer / dissolveTime));
            yield return null;
        }
        
    }

    IEnumerator UnDissolve()
    {
        myCollider.enabled = true;
        while (timer <= dissolveTime)
        {
            Debug.Log("in undissolve");
            timer += Time.fixedDeltaTime;
            timer = Mathf.Clamp(timer, 0, float.MaxValue);
            spriteRenderer.material.SetFloat("_Fade", Mathf.Clamp01(timer / dissolveTime));
            yield return null;
        }
    }
}
