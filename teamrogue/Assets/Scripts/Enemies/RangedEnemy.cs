using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyAI, IDamage
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform shootPos;
    

    protected bool isShooting;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isShooting == false)
            StartCoroutine(shoot());
    }

    public virtual IEnumerator shoot()
    {
        isShooting = true;

        animator.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.position, transform.rotation);
        
        yield return new WaitForSeconds(attackRate);
        
        isShooting = false;
    }

}
