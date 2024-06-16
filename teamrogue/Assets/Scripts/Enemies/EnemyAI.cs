using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] protected Renderer model;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform dropSpawn;
    [SerializeField] protected GameObject healthDrop;

    [SerializeField] protected int HP;
    [SerializeField] protected float attackRate;

    Vector3 playerDir;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameManager.instance.updateGoal(1);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Movement();
    }

    public void Movement()
    {
        playerDir = GameManager.instance.player.transform.position - transform.position;
        float agentSpeed = agent.velocity.normalized.magnitude;
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), agentSpeed, Time.deltaTime));

        if (agent.remainingDistance < agent.stoppingDistance)
        {
            faceTarget();
        }

        agent.SetDestination(GameManager.instance.player.transform.position);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            GameManager.instance.updateGoal(-1);
            Destroy(gameObject);
            Instantiate(healthDrop, dropSpawn.position, transform.rotation);
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 3);
    }
}
