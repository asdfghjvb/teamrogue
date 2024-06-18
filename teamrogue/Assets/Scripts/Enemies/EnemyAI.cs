using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] protected Renderer model;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform dropSpawn;
    [SerializeField] protected GameObject healthDrop;
    [Space(5)]

    [Header("Stats")]
    [SerializeField] protected int HP;
    [SerializeField] protected float attackRate;
    [Space(5)]

    [Header("Misc")]
    [Tooltip("How long in seconds the body will last after the death animation before being destroyed")]
    [SerializeField] protected float deathAnimationDuration = 5.0f;

    Vector3 playerDir;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameManager.instance.updateGoal(1);
        agent.SetDestination(GameManager.instance.player.transform.position); //keeps enemies from attacking on game start, even when out of range
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
           
            Collider collider = GetComponent<Collider>();
            collider.enabled = false; //disable collider

            agent.enabled = false; //disable nav mesh
            
            enabled = false; //disable movement
            
            StartCoroutine(OnDeath());
        }
    }

    protected virtual IEnumerator OnDeath()
    {
        animator.SetTrigger("Death");
        GameManager.instance.updateGoal(-1);
        Instantiate(healthDrop, dropSpawn.position, transform.rotation);

        yield return new WaitForSeconds(deathAnimationDuration);

        Destroy(gameObject);
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
