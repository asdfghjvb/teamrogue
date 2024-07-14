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

        //if(agent.remainingDistance <= agent.stoppingDistance && !isAttacking && playerInView())
        //{
        //    StartCoroutine(melee());
        //}

        if (agent.remainingDistance <= agent.stoppingDistance && !isAttacking)
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
        Collider collider = GetComponent<Collider>();
        WeaponColOff();
        collider.enabled = false;
        agent.enabled = false;
        enabled = false;

        animator.SetTrigger("Death");
        GameManager.instance.updateGoal(-1);

        // Spawn either health or ammo drop based on their respective chances
        float dropRoll = Random.value;

        if (dropRoll < healthDropChance)
        {
            Debug.Log("Dropping Health");
            Instantiate(healthDrop, dropSpawn.position, Quaternion.identity);
        }
        else if (dropRoll < healthDropChance + ammoDropChance)
        {
            Debug.Log("Dropping Ammo");
            Instantiate(ammoDropPrefab, dropSpawn.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(deathAnimationDuration);

        Destroy(gameObject);
    }

}
