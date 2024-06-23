using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class Player : MonoBehaviour, IDamage
{
    //public static Player instance;
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
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject staffModel;
    [SerializeField] public List<Staffs> staffList = new List<Staffs>();

    int currentStaff;
    bool isShooting;
    bool isMeleeAttacking;
    float lastMeleeTime;
    int jumpCount;
    public float fullHealth;
   

    Vector3 moveDir;
    Vector3 playerVel;
    // Start is called before the first frame update
    void Start()
    {
        //instance = this;
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        fullHealth = health;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        Movement();
        Sprint();
        UpdateMeleeCooldownUI();
        if (Input.GetButton("Fire1") && !isShooting && staffList.Count > 0)
        {
            StartCoroutine(shoot());
        }
        selectStaff();

        if (Input.GetButtonDown("Fire2") && !isMeleeAttacking && Time.time >= lastMeleeTime + meleeCooldown)
        {
            StartCoroutine(melee());
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
        //isShooting = true;
        //RaycastHit hit;
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
        //{
        //    IDamage dmg = hit.collider.GetComponent<IDamage>();

        //    if (hit.transform != transform && dmg != null)
        //    {
        //        dmg.takeDamage(shootDamage);
        //    }
        //}
        //yield return new WaitForSeconds(shootRate);
        //isShooting = false;
        if (!GameManager.instance.isPaused)
        {
            isShooting = true;
            Instantiate(bullet, shootPos.position, shootPos.rotation);

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
    }

    void UpdateMeleeCooldownUI()
    {
        float cooldownRemaining = Mathf.Clamp01((Time.time - lastMeleeTime) / meleeCooldown);
        GameManager.instance.UpdateMeleeCooldownUI(cooldownRemaining);
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
            GameManager.instance.beginDoorCol.enabled = true;
        currentStaff = staffList.Count - 1;


        shootDamage = staff.staffDamage;
        shootDist = staff.staffDistance;
        shootRate = staff.staffSpeed;
        bullet = staff.bullet;

        staffModel.GetComponent<MeshFilter>().sharedMesh = staff.staffModel.GetComponent<MeshFilter>().sharedMesh;
        staffModel.GetComponent<MeshRenderer>().sharedMaterial = staff.staffModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectStaff()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentStaff < staffList.Count - 1)
        {
            currentStaff++;
            changeStaff();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentStaff > 0)
        {
            currentStaff--;
            changeStaff();
        }
    }

    void changeStaff()
    {
        shootDamage = staffList[currentStaff].staffDamage;
        shootDist = staffList[currentStaff].staffDistance;
        shootRate = staffList[currentStaff].staffSpeed;
        bullet = staffList[currentStaff].bullet;

        staffModel.GetComponent<MeshFilter>().sharedMesh = staffList[currentStaff].staffModel.GetComponent<MeshFilter>().sharedMesh;
        staffModel.GetComponent<MeshRenderer>().sharedMaterial = staffList[currentStaff].staffModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
}

