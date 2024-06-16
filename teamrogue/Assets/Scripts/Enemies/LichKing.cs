using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichKing : RangedEnemy
{
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
