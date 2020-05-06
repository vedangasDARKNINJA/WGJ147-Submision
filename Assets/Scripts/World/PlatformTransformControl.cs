using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTransformControl : MonoBehaviour
{
    static Transform player;
    BoxCollider2D boxCollider;

    bool playerOnMe;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!playerOnMe)
        {
            if (transform.position.y < player.position.y)
            {
                boxCollider.enabled = true;
            }
            else
            {
                boxCollider.enabled = false;
            }
        }
        else
        {
            boxCollider.enabled = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.parent = transform;
            playerOnMe = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.parent = null;
            playerOnMe = false;
        }
    }
}
