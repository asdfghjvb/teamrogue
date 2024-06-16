using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyAI, IDamage
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] bool spawner = false;

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
       if (spawner)
        {
            GameManager.instance.updateGoal(1);
        }
       yield return new WaitForSeconds(attackRate);
       isShooting = false;
   }

}
