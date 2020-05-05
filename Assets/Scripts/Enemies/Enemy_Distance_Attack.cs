using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Distance_Attack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    [SerializeField] float xVelocity = 3f;

    Rigidbody2D enemyRB;
    Animator Enemy_Animator;

    bool running = false;
    bool attacking = false;
    float flipSide;
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        Enemy_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }
    private void CheckDistance()
    {
        Vector2 this_enemy = new Vector2(transform.position.x, transform.position.y);
        Vector2 the_player = new Vector2(player.transform.position.x, player.transform.position.y);
        float distance = Vector2.Distance(this_enemy, the_player);
        float distance2 = player.transform.position.x - transform.position.x;
        flipSide = Mathf.Sign(distance2);
        if (distance < 10)
        {
            
            Vector2 Enemy_velocity = enemyRB.velocity;
            Enemy_velocity.x = flipSide*xVelocity;
            enemyRB.velocity = Enemy_velocity;
            Enemy_Animator.SetBool("Player_Detected", true);
            
            if(distance <3)
            {
                Enemy_velocity.x = 0;
                enemyRB.velocity = Enemy_velocity;
                Enemy_Animator.SetBool("Attacking", true);
                StartCoroutine(Attacking_Loop());
            }
            
        }
        else
        {
            Enemy_Animator.SetBool("Player_Detected", false);
            Enemy_Animator.SetBool("Attacking", false);
        }
    }
    IEnumerator Attacking_Loop()
    {
        Enemy_Animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(4);
        Enemy_Animator.SetBool("Attacking", true);
    }
}
