using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichKing : RangedEnemy
{
    [SerializeField] GameObject minion;

    [SerializeField] int spawnRate;
    [SerializeField] int spawnQuantity;
    [SerializeField] int spawnRadius;

    bool spawnCooldown;

    protected override void Update()
    {
        base.Update();

        if(!spawnCooldown)
        {
            StartCoroutine(spawnMinions());
        }
    }

    IEnumerator spawnMinions()
    {
        spawnCooldown = true;

        Quaternion rotation = transform.rotation;
        for (int i = 0; i < spawnQuantity; i++)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = transform.position.y;

            Instantiate(minion, randomPosition, rotation);
        }

        yield return new WaitForSeconds(spawnRate);
        spawnCooldown = false;
    }

    public override IEnumerator shoot()
    {
        isShooting = true;

        yield return StartCoroutine(playShootAnimation());
        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(attackRate);

        isShooting = false;
    }

   public IEnumerator playShootAnimation()
    {
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.47f);
        //0.47 seconds seems to be a sweet spot where it hits a still player about 75% of the time
        //Keep between 0.45 & 0.48 for best results 
    }
}
