using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAI, IDamage
{
    bool isAttacking = false;


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(agent.remainingDistance <= agent.stoppingDistance && !isAttacking)
        {
            StartCoroutine(melee());
        }
    }

    IEnumerator melee()
    {
        isAttacking = true;
       
        animator.SetTrigger("Melee");
        yield return new WaitForSeconds(attackRate);

        isAttacking = false;
    }
}
