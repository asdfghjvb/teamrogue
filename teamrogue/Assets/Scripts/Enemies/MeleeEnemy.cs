using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAI, IDamage
{
    [Space(5)]
    [Header("Melee Weapon")]
    [SerializeField] protected Collider weaponCol;

    protected bool isAttacking = false;

    protected override void Start()
    {
        base.Start();

        weaponCol.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if ((agent.remainingDistance <= agent.stoppingDistance + 1 && agent.remainingDistance >= agent.stoppingDistance - 1)
            && !isAttacking)
        {
            StartCoroutine(melee());
        }
    }

    protected virtual IEnumerator melee()
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
    protected override IEnumerator OnDeath()
    {
        WeaponColOff();
        
        StartCoroutine(base.OnDeath());

        yield return new WaitForSeconds(0f); //Does the wait in the base call, no need to wait again
    }

}
