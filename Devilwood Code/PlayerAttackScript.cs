using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public Animator anim;
    public bool isAttack;
    public float playerAttackDelaySeconds;
    public PlayerMovementPrime movement;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask bossLayer;
    public int playerAttackValue;

    private void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovementPrime>();
    }

    void Update()
    {
        
        if(Input.GetKeyDown("a") || Input.GetKeyDown("d") || Input.GetButton("Jump"))
        {
            isAttack = false;
            anim.SetBool("isAttack", isAttack);
            // override deactivate attack here
        }
        else if (Input.GetMouseButtonDown(0) && !isAttack && movement.isGrounded)
        {
            // Override Movement to be 0 on player attack
            movement.rb.velocity = new Vector2(0, 0);
            isAttack = true;
            Attack();
        }
    }

    void Attack()
    {
        //play attack anim
        anim.SetBool("isAttack", isAttack);
        StartCoroutine(AttackDelay());
        
        // activate attack here
        IEnumerator AttackDelay()
        {
            yield return new WaitForSeconds(playerAttackDelaySeconds);
            // deactivate attack here
            isAttack = false;
            anim.SetBool("isAttack", isAttack);
        }

        Collider2D[]  hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, bossLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(playerAttackValue);
        }
        /* 
         Line 27 is updating the animator. Line 19 is making isAttack to true. We need to update the animation after we set isAttack to false. 
         We need to setup a corotine that allows for an update for the animation after its delay. In that corotine, at its start, we need to create a
        collider which is its attack area. Once the corotine ends, that collider needs to be destoryed. If the collider already exist that means it is currently attacking
        so we need to stop the player from attacking again during that time. 
         */

        // Detect Enemies in range of the attack

        //apply damage
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return; 
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
