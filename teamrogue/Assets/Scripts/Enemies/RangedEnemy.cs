using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyAI, IDamage
{

    [Space(5)]
    [Header("Ranged Weapon")]
    
    [Tooltip("The object that will be created when this enemy fires")]
    [SerializeField] protected GameObject projectile;
    
    [Tooltip("The transform the projectile when be created at")]
    [SerializeField] protected Transform shootPos;
    

    protected bool isShooting;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isShooting == false && playerInView())
            StartCoroutine(shoot());
    }

    public virtual IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(projectile, shootPos.position, transform.rotation);
        
        yield return new WaitForSeconds(attackRate);
        
        isShooting = false;
    }

}
