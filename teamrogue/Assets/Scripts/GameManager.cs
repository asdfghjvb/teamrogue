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

    [SerializeField] GameObject rewardChest;
    [SerializeField] public GameObject healButton;
    [SerializeField] public GameObject boonButton;

    [SerializeField] public Collider beginDoorCol;
    [SerializeField] public Collider room1DoorCol;

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
            
            rewardChest.SetActive(true);
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
        menuActive = menuBoon;
        menuActive.SetActive(isPaused);
        boonCount++;
        if (playerScript.staffList.Count == 3 && boonCount == 1)
            beginDoorCol.enabled = true;

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

    public void applyBoon(int boonID)
    {
       
        switch (boonID)
        {
            case 1:
                GameManager.instance.playerScript.fullHealth += 5;
                GameManager.instance.playerScript.health += 5;
                break;
            case 2:
                foreach (Staffs staff in GameManager.instance.playerScript.staffList)
                {
                    staff.staffSpeed -= 0.75f;
                }
                break;
            case 3:
                GameManager.instance.playerScript.speed += 5;
                break;
            case 4:
                GameManager.instance.playerScript.jumpMax += 1;
                break;
            case 5:
                foreach (Staffs staff in GameManager.instance.playerScript.staffList)
                {
                    staff.staffDamage += 3;
                }
                break;
            case 6:
                GameManager.instance.playerScript.armorMod *= 0.9f;
                break;
            case 7:
                GameManager.instance.playerScript.sprintMod *= 1.2f;
                break;
            case 8:
                foreach (Staffs staff in GameManager.instance.playerScript.staffList)
                {
                   staff.staffDistance += 10;
                }
                break;
            case 9:
                GameManager.instance.playerScript.meleeDamage += 3;
                break;
            case 10:
                GameManager.instance.playerScript.meleeCooldown *= 0.8f;
                break;
        }
        
    }
}
