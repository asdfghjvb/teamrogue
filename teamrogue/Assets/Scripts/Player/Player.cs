using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController playerController;
    [SerializeField] public float health;
    [SerializeField] public float mana;
    [SerializeField] public float manaCost;
    [SerializeField] public float speed;
    [SerializeField] public float sprintMod;
    [SerializeField] public float armorMod;
    [SerializeField] public int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [SerializeField] public int meleeDamage;
    [SerializeField] float meleeRange;
    [SerializeField] float meleeRate;
    [SerializeField] public float meleeCooldown;
    [SerializeField] public float knockbackForce;
    [SerializeField] public Animator staff;

    [SerializeField] public int shootDamage;
    [SerializeField] public float shootRate;
    [SerializeField] public int shootDist;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip shootEffect;
    [SerializeField] AudioClip meleeEffect;
    [SerializeField] float shootVol;
    [SerializeField] float meleeVol;

    public int innateShootDamage;
    public float innateShootRate;
    public int innateShootDist;
    public int currentGold;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject staffModel;
    [SerializeField] public List<Staffs> staffList = new List<Staffs>();

    int currentStaffIndex = -1; // Intialize to -1 to indicate that there is no staff equipped
    bool isShooting;
    bool isMeleeAttacking;
    float lastMeleeTime;
    int jumpCount;
    public float fullHealth;
    public float fullMana;

    Vector3 moveDir;
    Vector3 playerVel;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameObject sfxAudioSource = GameObject.FindWithTag("SFX Audio Source");
        if(sfxAudioSource != null)
            aud = sfxAudioSource.GetComponent<AudioSource>();

        if (GameManager.instance.playerScript == null || GameManager.instance.player == null)
        {//bc the gm is made before the player, the player does not exist yet in dungeons
            GameManager.instance.playerScript = this;
            GameManager.instance.player = gameObject;
        }

        fullHealth = health;
        fullMana = mana;
        updatePlayerUI();

        if (staffList.Count > 0)
        {
            EquipStaff(0); // Equipar el primer staff por defecto
        }

        if(GameManager.instance.IsInScene("Hub") && LoadDungeon.hasValue)
        {
            staffList = LoadDungeon.staffList;
            health = LoadDungeon.health;
            speed = LoadDungeon.speed;
            sprintMod = LoadDungeon.sprintMod;
            armorMod = LoadDungeon.armorMod;
            jumpMax = LoadDungeon.jumpMax;
            meleeDamage = LoadDungeon.meleeDamage;
            shootDamage = LoadDungeon.shootDamage;
            shootRate = LoadDungeon.shootRate;
            shootDist = LoadDungeon.shootRange;
            meleeCooldown = LoadDungeon.meleeCooldown;
            currentGold = LoadDungeon.gold;
        }
    }

    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
            Movement();
            Sprint();
            if (Input.GetButton("Fire1") && !isShooting && mana > manaCost && staffList.Count > 0)
            {
                StartCoroutine(shoot());
                updatePlayerUI();

                GameManager.instance.ModifySeed(17);
            }
            selectStaff();

            if (Input.GetButtonDown("Fire2") && !isMeleeAttacking && Time.time >= lastMeleeTime + meleeCooldown)
            {
                StartCoroutine(melee());
                GameManager.instance.ModifySeed(41);
            }
        }
        
    }

    void Movement()
    {
        if (playerController.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        playerController.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
            GameManager.instance.ModifySeed(71);
        }
        playerVel.y -= gravity * Time.deltaTime;
        playerController.Move(playerVel * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            GameManager.instance.ModifySeed(59);
        }
        else if (Input.GetButtonUp("Sprint"))
            speed /= sprintMod;
    }

    IEnumerator shoot()
    {
        if (!GameManager.instance.isPaused)
        {
            isShooting = true;
            mana -= manaCost;
            
            aud.PlayOneShot(shootEffect, shootVol);
            Instantiate(bullet, shootPos.position, shootPos.rotation);
            

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    IEnumerator melee()
    {
        isMeleeAttacking = true;
        aud.PlayOneShot(meleeEffect, meleeVol);
        lastMeleeTime = Time.time;
        staff.Play("Staffswing", 0, 0f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * meleeRange, meleeRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == gameObject)
                continue;

            IDamage dmg = hitCollider.GetComponent<IDamage>();
            if (dmg != null)
            {
                mana += 2;
                updatePlayerUI();
                dmg.takeDamage(meleeDamage);
                Vector3 knockbackDirection = (hitCollider.transform.position - transform.position).normalized;
                dmg.knockback(knockbackDirection, knockbackForce);
            }
        }
        yield return new WaitForSeconds(meleeRate);
        isMeleeAttacking = false;
        staff.Play("Idle", 0, 0f);
        GameManager.instance.UpdateMeleeCooldownUI(meleeCooldown, lastMeleeTime); // Update the ui from the game manager (translated)
    }

    public void updatePlayerUI()
    {
        GameManager.instance.playerHealthBar.fillAmount = (float)health / fullHealth;
        GameManager.instance.playerManaBar.fillAmount =(float)mana / fullMana;
    }

    public void takeDamage(int amount)
    {
        health -= amount * armorMod;
        updatePlayerUI();

        if (health <= 0)
        {
            GameManager.instance.youLose();
        }
    }

    public void getStaff(Staffs staff)
    {
        staffList.Add(staff);
        EquipStaff(staffList.Count - 1);
    }

    void selectStaff()
    {
        if (!GameManager.instance.isPaused)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentStaffIndex < staffList.Count - 1)
            {
                EquipStaff(currentStaffIndex + 1);
                GameManager.instance.ModifySeed(2);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentStaffIndex > 0)
            {
                EquipStaff(currentStaffIndex - 1);
                GameManager.instance.ModifySeed(3);
            }
        }
    }

    void EquipStaff(int index)
    {
        currentStaffIndex = index;
        Staffs newStaff = staffList[index];

        shootDamage = newStaff.staffDamage + innateShootDamage;
        shootDist = newStaff.staffDistance + innateShootDist;
        shootRate = newStaff.staffSpeed * innateShootRate;
        manaCost = newStaff.staffManaCost;
        bullet = newStaff.bullet;
        shootEffect = newStaff.staffShootEffect;
        shootVol = newStaff.staffShootVol;

        staffModel.GetComponent<MeshFilter>().sharedMesh = newStaff.staffModel.GetComponent<MeshFilter>().sharedMesh;
        staffModel.GetComponent<MeshRenderer>().sharedMaterial = newStaff.staffModel.GetComponent<MeshRenderer>().sharedMaterial;

        GameManager.instance.SetCurrentStaff(newStaff);
        
    }
    public ScriptableObject objectView(float viewingRange = 30f)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, viewingRange))
        {
            if (hit.collider.CompareTag("Trophy"))
            {
                GameManager.instance.inputHint.SetActive(true);
                return hit.collider.GetComponent<TrophyObject>().trophy;
            }
            else if (hit.collider.CompareTag("Anvil"))
            {
                GameManager.instance.inputHint.SetActive(true);
                return hit.collider.GetComponent<Anvil>().menu;
            }
            else if (hit.collider.CompareTag("Sign"))
            {
                GameManager.instance.inputHint.SetActive(true);
                return hit.collider.GetComponent<SignPost>().signScript;
            }
            else if (hit.collider.CompareTag("Door"))
            {
                GameManager.instance.inputHint.SetActive(true);
                return null;
            }
            else if (hit.collider.CompareTag("Portal"))
            {
                GameManager.instance.inputHint.SetActive(true);
                return null;
            }
            else if (hit.collider.CompareTag("Chest"))
            {
                GameManager.instance.inputHint.SetActive(true);
                return hit.collider.GetComponent<Chest>().chestOb;
            }
            else
            {
                GameManager.instance.inputHint.SetActive(false);
            }
        }
        return null;
    }
    public void knockback(Vector3 dir, float force)
    {

    }
}

