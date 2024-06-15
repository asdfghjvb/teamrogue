using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyAI
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (isShooting == false)
            StartCoroutine(shoot());
    }

   IEnumerator shoot()
   {
       isShooting = true;
       Instantiate(bullet, shootPos.position, transform.rotation);

       yield return new WaitForSeconds(attackRate);
       isShooting = false;
   }
}
