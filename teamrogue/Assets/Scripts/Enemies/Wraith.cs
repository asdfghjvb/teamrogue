using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wraith : MeleeEnemy, IDamage
{
    [Space(5)]
    [Header("Teleport")]

    [Tooltip("How often the wraith will teleport closer to the player")]
    [SerializeField] int teleportRate;

    [Tooltip("The radius around the player in which the wraith with teleport")]
    [SerializeField] int teleportRadius;

    [Tooltip("The particle effect left behind when teleport ability is used")]
    [SerializeField] ParticleSystem teleportEffect;

    bool canTeleport = true;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (canTeleport && agent.remainingDistance > agent.stoppingDistance && playerInView())
            StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        canTeleport = false;

        Vector3 targetPos;

        Transform playerTransform = GameManager.instance.player.transform;
        Vector3 randomDirection = Random.insideUnitSphere * teleportRadius;
        randomDirection += playerTransform.position;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, teleportRadius, NavMesh.AllAreas))
        {// if random pos is on nav mesh, teleport there. Other wise the teleport fails and nothing happens
            targetPos = navHit.position;

            float destroyTime = teleportEffect.main.startLifetime.constantMax;

            ParticleSystem effect1 = Instantiate(teleportEffect, transform.position, Quaternion.identity);
            Destroy(effect1.gameObject, destroyTime);

            transform.position = targetPos;

            ParticleSystem effect2 = Instantiate(teleportEffect, transform.position, Quaternion.identity);
            Destroy(effect2.gameObject, destroyTime);
        }

        yield return new WaitForSeconds(teleportRate);
    
        canTeleport = true;
    }
}
