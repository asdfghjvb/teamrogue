using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAI, IDamage
{
    [Space(5)]
    [Header("Melee Weapon")]
    [SerializeField] Collider weaponCol;

    bool isAttacking = false;

    protected override void Start()
    {
        base.Start();

        weaponCol.enabled = false;
    }

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

    public void WeaponColOn()
    {
        weaponCol.enabled = true;
    }

    public void WeaponColOff()
    {
        weaponCol.enabled = false;
    }
}
