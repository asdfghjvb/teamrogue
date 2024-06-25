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
    [SerializeField] Transform pov;
    [SerializeField] int viewAngle;
    [Space(5)]

    [Header("Health Drop")]
    [SerializeField] protected Transform dropSpawn;
    [SerializeField] protected GameObject healthDrop;

    [Range(0, 1)]
    [SerializeField] protected float dropChance;
    [Space(5)]

    [Header("Stats")]
    [SerializeField] protected int HP;
    [SerializeField] protected float attackRate;
    [Space(5)]

    [Header("Misc")]
    [Tooltip("How long in seconds the body will last after the death animation before being destroyed")]
    [SerializeField] protected float deathAnimationDuration = 5.0f;

    Vector3 playerDirection;
    float angleToPlayer;

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
        playerDirection = GameManager.instance.player.transform.position - transform.position;
        float agentSpeed = agent.velocity.normalized.magnitude;
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), agentSpeed, Time.deltaTime));

        if (agent.remainingDistance < agent.stoppingDistance)
        {
            faceTarget();
        }

        if(playerInView())
        {
            agent.SetDestination(GameManager.instance.player.transform.position);
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());
        Movement(); //if enemy takes dmg, move towards player

        if (HP <= 0)
        {
            StartCoroutine(OnDeath());
        }
    }

    protected virtual IEnumerator OnDeath()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false; //disable collider
        agent.enabled = false; //disable nav mesh
        enabled = false; //disable movement

        animator.SetTrigger("Death");
        GameManager.instance.updateGoal(-1);

        float checkSpawnChance = Random.value;
        if (checkSpawnChance < dropChance)
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
        Quaternion rot = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 3);
    }

    public bool playerInView()
    {
        playerDirection = GameManager.instance.player.transform.position - new Vector3(0,.5f,0) - pov.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(pov.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(pov.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                return true;
            }
        }
        return false;
    }
}
