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

    [Header("Drop Settings")]
    [SerializeField] protected Transform dropSpawn;
    [SerializeField] protected GameObject healthDrop;
    [SerializeField] protected GameObject ammoDropPrefab;
    [SerializeField] protected GameObject chest;
    [SerializeField] protected int goldAmount;
    [Range(0, 1)]
    [SerializeField] protected float healthDropChance;
    [Range(0, 1)]
    [SerializeField] protected float ammoDropChance;
    [Range(0, 1)]
    [SerializeField] protected float chestDropChance;
    [Space(5)]

    [Header("Stats")]
    [SerializeField] protected int HP;
    [SerializeField] protected float attackRate;
    [Space(5)]

    [Header("Misc")]
    [Tooltip("How long in seconds the body will last after the death animation before being destroyed")]
    [SerializeField] protected float deathAnimationDuration = 5.0f;
    [Tooltip("Any objects that already exist in the scene that should become acive upon death")]
    [SerializeField] List<GameObject> deathActivatedObjects;


    public Vector3 knockbackTarget;
    private bool isKnockedback;
    [SerializeField] float knockbackSpeed;

    Vector3 playerDirection;
    float angleToPlayer;

    protected virtual void Start()
    {
        GameManager.instance.updateGoal(1);
    }

    protected virtual void Update()
    {
        Movement();
        if (isKnockedback)
        {
            transform.position = Vector3.Lerp(transform.position, knockbackTarget, Time.deltaTime * knockbackSpeed);
            if (Vector3.Distance(transform.position, knockbackTarget) < 0.1f)
            {
                isKnockedback = false;
            }
        }
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

        if (playerInView())
        {
            agent.SetDestination(GameManager.instance.player.transform.position);
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());
        Movement();

        if (HP <= 0)
        {
            StartCoroutine(OnDeath());
        }
    }

    protected virtual IEnumerator OnDeath()
    {
        if(TryGetComponent<Collider>(out var collider))
            collider.enabled = false;

        agent.enabled = false;
        enabled = false;

        animator.SetTrigger("Death");
        GameManager.instance.updateGoal(-1);
        GameManager.instance.UpdatePlayerCurrency(goldAmount);

        // Spawn either health or ammo drop based on their respective chances
        float dropRoll = Random.Range(0f,1f);

        if (dropRoll < healthDropChance)
        {
            Instantiate(healthDrop, dropSpawn.position, Quaternion.identity);
        }
        else if (dropRoll < healthDropChance + ammoDropChance)
        {
            Instantiate(ammoDropPrefab, dropSpawn.position, Quaternion.identity);
        }
        else if (dropRoll <= healthDropChance + ammoDropChance + chestDropChance)
        {
            Instantiate(chest, dropSpawn.position, Quaternion.identity);
        }

        ToggleDeathActivatedObjects(true);

        yield return new WaitForSeconds(deathAnimationDuration);

        Destroy(gameObject);

        GameManager.instance.achievementManager.UpdateCount(4);
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
        playerDirection = GameManager.instance.player.transform.position - new Vector3(0, .5f, 0) - pov.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

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
    public void knockback(Vector3 direction, float force)
    {
        knockbackTarget = transform.position + direction * force;
        isKnockedback = true;
    }

    public void ToggleDeathActivatedObjects(bool isActive)
    {
        if (deathActivatedObjects == null)
            return;

        foreach(GameObject obj in deathActivatedObjects)
        {
            obj.SetActive(isActive);
        }
    }

    public void AddDeathActivatedObject(GameObject obj)
    {
        deathActivatedObjects.Add(obj);
    }

}
