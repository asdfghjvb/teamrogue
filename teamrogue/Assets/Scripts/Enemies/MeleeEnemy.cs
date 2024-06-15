using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAI, IDamage
{
    bool isAttacking = false;

    void Start()
    {
        GameManager.instance.updateGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

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
