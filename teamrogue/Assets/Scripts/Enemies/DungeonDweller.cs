using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDweller : MeleeEnemy
{
    [SerializeField] int hornAttackRate;

    float maxNavMeshAgentSpeed;
    bool isHornAttacking = false;

    protected override void Start()
    {
        base.Start();

        maxNavMeshAgentSpeed = agent.speed;
    }

    protected override void Update()
    {
        base.Update();

        if (agent.remainingDistance <= (agent.stoppingDistance * 2) && !isHornAttacking)
            StartCoroutine(HornAttack());
    }

    public void ToggleHalted()
    {
        agent.isStopped = !agent.isStopped;
    }

    protected virtual IEnumerator HornAttack()
    {
        isHornAttacking = true;
        
        animator.SetTrigger("Horn Attack");
        yield return new WaitForSeconds(hornAttackRate);

        isHornAttacking = false;
    }

    protected override IEnumerator OnDeath()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>(true);

        foreach(Collider col in colliders)
            col.enabled = false;

        return base.OnDeath();
    }
}
