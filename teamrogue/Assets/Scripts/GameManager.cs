using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuBoon;
    [SerializeField] GameObject chestMenu;

    [SerializeField] public GameObject rewardChest1;
    [SerializeField] public GameObject rewardChest2;
    [SerializeField] public GameObject healButton;
    [SerializeField] public GameObject boonButton;
    public bool room1Clear = false, room2Clear = false;

    [SerializeField] public Collider door1Col, door2Col, door3Col;

    [Header("Ammo Settings")]
    [SerializeField] int maxReserveAmmo = 30; // Munición máxima en reserva
    [SerializeField] int currentReserveAmmo;
    [SerializeField] float reloadTime = 2f;
    bool isReloading = false;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI ammoText;

    private Staffs currentStaff;

    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] Image meleeCooldownUI;

    public List<Staffs> allStaffs = new List<Staffs>();

    public Image playerHealthBar;


    public GameObject player;
    public Player playerScript;

    public GameObject plane;
    public NavMeshBaker navMeshBakerScript;

    public bool isPaused;
    int enemyCount;
    public int boonCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();

        plane = GameObject.FindWithTag("Plane");
        navMeshBakerScript = plane.GetComponent<NavMeshBaker>();


        foreach (Staffs staffs in allStaffs)
        {
            staffs.InitializeStaffValues();
        }
        currentReserveAmmo = maxReserveAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePaused();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }

            else if (menuActive == menuPause)
            {
                stateUnpaused();

            }
            
        }
    }

    public void statePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void stateUnpaused() 
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }

    public void updateGoal(int amount)
    {
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");

        if (enemyCount <= 0)
        {
            rewardChest1.SetActive(true);
            room1Clear = true;
        }
        else if (enemyCount <= 0 && room1Clear) 
        {
            rewardChest2.SetActive(true);
            room2Clear = true;
        }
        else if (enemyCount <= 0 && room2Clear)
        {
            youWin();
        }
    }
    public void youWin()
    {
        statePaused();
        menuActive = menuWin;
        menuActive.SetActive(isPaused);
    }
    public void youLose()
    {
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }
    public void boonSelection()
    {
        statePaused();
        BoonManager.instance.randomizeList();
        menuActive = menuBoon;
        menuActive.SetActive(isPaused);
        boonCount++;
        if (playerScript.staffList.Count == 3 && boonCount == 1)
            door1Col.enabled = true;

    }
    public void rewardMenu()
    {
        statePaused();
        menuActive = chestMenu;
        menuActive.SetActive(isPaused);
    }
    public void UpdateMeleeCooldownUI(float cooldownRemaining)
    {
        if (meleeCooldownUI != null)
        {
            meleeCooldownUI.fillAmount = cooldownRemaining;
        }
    }

    public void SetCurrentStaff(Staffs staff)
    {
        // Cambiar al nuevo staff
        currentStaff = staff;
        UpdateAmmoUI(); // Asegurarnos de actualizar la UI cuando cambiamos el staff
    }

    public bool HasAmmoInClip()
    {
        return currentStaff != null && currentStaff.currentAmmoInClip > 0;
    }

    public void UseAmmo()
    {
        if (currentStaff != null && currentStaff.currentAmmoInClip > 0)
        {
            currentStaff.currentAmmoInClip--;
            UpdateAmmoUI();
        }
    }

    public void Reload()
    {
        if (!isReloading && currentReserveAmmo > 0 && currentStaff != null)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        int ammoToReload = currentStaff.maxAmmoInClip - currentStaff.currentAmmoInClip;
        if (currentReserveAmmo >= ammoToReload)
        {
            currentStaff.currentAmmoInClip += ammoToReload;
            currentReserveAmmo -= ammoToReload;
        }
        else
        {
            currentStaff.currentAmmoInClip += currentReserveAmmo;
            currentReserveAmmo = 0;
        }
        isReloading = false;
        UpdateAmmoUI();
    }

    public int GetCurrentAmmoInClip()
    {
        return currentStaff != null ? currentStaff.currentAmmoInClip : 0;
    }

    public int GetCurrentReserveAmmo()
    {
        return currentReserveAmmo;
    }

    public void AddAmmo(int amount)
    {
        currentReserveAmmo = Mathf.Min(currentReserveAmmo + amount, maxReserveAmmo);
        UpdateAmmoUI();
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null && currentStaff != null)
        {
            ammoText.text = $"{currentStaff.currentAmmoInClip}/{currentReserveAmmo}";
        }
    }


}
