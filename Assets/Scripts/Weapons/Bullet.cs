using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5;

    Rigidbody2D rb;
    TrailRenderer trail;
    [HideInInspector]
    public Vector2 velocity;

    public float invisibleTime=5f;
    public GameObject currentHit;
    DissolveController currentDissolveController;

    SpriteRenderer spriteRenderer;
    Collider2D rb_collider;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb_collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();
        trail.emitting = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("PickableItem") || other.transform.CompareTag("Enemy"))
        {
            trail.enabled = false;
            spriteRenderer.enabled = false;
            rb_collider.enabled = false;
            Debug.Log("pickable item");
            currentHit = other.gameObject;
            currentDissolveController = currentHit.GetComponent<DissolveController>();
            if(currentDissolveController!=null)
            {
                currentDissolveController.Dissolve();
                StartCoroutine(Invisible());
            }
            velocity = Vector2.zero;
        }
        else if(other.transform.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Invisible()
    {
        yield return new WaitForSeconds(invisibleTime);
        currentDissolveController.Dissolve(true);
        Destroy(gameObject);
    }
}
