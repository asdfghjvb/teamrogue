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

    [SerializeField] public int shootDamage;
    [SerializeField] public float shootRate;
    [SerializeField] public int shootDist;

    public int innateShootDamage;
    public float innateShootRate;
    public int innateShootDist;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject staffModel;
    [SerializeField] public List<Staffs> staffList = new List<Staffs>();

    int currentStaffIndex = -1; // Inicializamos con -1 para indicar que no hay staff equipado al inicio
    bool isShooting;
    bool isMeleeAttacking;
    float lastMeleeTime;
    int jumpCount;
    public float fullHealth;

    Vector3 moveDir;
    Vector3 playerVel;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        fullHealth = health;
        updatePlayerUI();

        if (staffList.Count > 0)
        {
            EquipStaff(0); // Equipar el primer staff por defecto
        }
    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        Movement();
        Sprint();
        if (Input.GetButton("Fire1") && !isShooting && GameManager.instance.HasAmmoInClip())
        {
            StartCoroutine(shoot());
        }
        selectStaff();

        if (Input.GetButtonDown("Fire2") && !isMeleeAttacking && Time.time >= lastMeleeTime + meleeCooldown)
        {
            StartCoroutine(melee());
        }

        if (Input.GetButtonDown("Reload"))
        {
            GameManager.instance.Reload();
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
        }
        playerVel.y -= gravity * Time.deltaTime;
        playerController.Move(playerVel * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
            speed *= sprintMod;
        else if (Input.GetButtonUp("Sprint"))
            speed /= sprintMod;
    }

    IEnumerator shoot()
    {
        if (!GameManager.instance.isPaused && GameManager.instance.HasAmmoInClip())
        {
            isShooting = true;
            Instantiate(bullet, shootPos.position, shootPos.rotation);
            GameManager.instance.UseAmmo();

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    IEnumerator melee()
    {
        isMeleeAttacking = true;
        lastMeleeTime = Time.time;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * meleeRange, meleeRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == gameObject)
                continue;

            IDamage dmg = hitCollider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(meleeDamage);
            }
        }
        yield return new WaitForSeconds(meleeRate);
        isMeleeAttacking = false;

        GameManager.instance.UpdateMeleeCooldownUI(meleeCooldown, lastMeleeTime); // Actualizar la UI desde GameManager
    }

    void updatePlayerUI()
    {
        GameManager.instance.playerHealthBar.fillAmount = (float)health / fullHealth;
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
        if (staffList.Count == 3 && GameManager.instance.boonCount >= 0)
            GameManager.instance.door1Col.enabled = true;
        EquipStaff(staffList.Count - 1);
    }

    void selectStaff()
    {
        if (!GameManager.instance.isPaused)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentStaffIndex < staffList.Count - 1)
            {
                EquipStaff(currentStaffIndex + 1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentStaffIndex > 0)
            {
                EquipStaff(currentStaffIndex - 1);
            }
        }
    }

    void EquipStaff(int index)
    {
        currentStaffIndex = index;
        Staffs newStaff = staffList[index];

        shootDamage = newStaff.staffDamage + innateShootDamage;
        shootDist = newStaff.staffDistance + innateShootDist;
        shootRate = newStaff.staffSpeed + innateShootRate;
        bullet = newStaff.bullet;

        staffModel.GetComponent<MeshFilter>().sharedMesh = newStaff.staffModel.GetComponent<MeshFilter>().sharedMesh;
        staffModel.GetComponent<MeshRenderer>().sharedMaterial = newStaff.staffModel.GetComponent<MeshRenderer>().sharedMaterial;

        GameManager.instance.SetCurrentStaff(newStaff);
        GameManager.instance.UpdateAmmoUI(); // Asegurarnos de actualizar la UI después de cambiar el staff
    }
}
