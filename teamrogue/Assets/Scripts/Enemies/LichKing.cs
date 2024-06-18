using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichKing : RangedEnemy
{
    [Space(5)]
    [Header("Minion Spawning")]

    [Tooltip("The enemy that will be summoned by the lich king")]
    [SerializeField] GameObject minion;

    [Tooltip("How often the lich king will spawn more minions")]
    [SerializeField] int spawnRate;

    [Tooltip("The amount of minions that will be spawned at once")]
    [SerializeField] int spawnQuantity;

    [Tooltip("The radius around the lich king in which minions will be spawned")]
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
        Instantiate(projectile, shootPos.position, transform.rotation);

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
