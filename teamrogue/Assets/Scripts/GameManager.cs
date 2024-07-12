using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

public class GameManager : MonoBehaviour
{
    //test change 

    public static GameManager instance;

    [Header("Menus")]
    [SerializeField] public GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuBoon;
    [SerializeField] GameObject chestMenu;
    [SerializeField] GameObject menuSettings;

    [SerializeField] public GameObject inputHint;

    [SerializeField] public GameObject rewardChest1;
    [SerializeField] public GameObject rewardChest2;
    [SerializeField] public GameObject healButton;
    [SerializeField] public GameObject boonButton;
    public bool room1Clear = false, room2Clear = false;

    [SerializeField] public Collider door1Col, door2Col, door3Col;

    [Header("Player Settings")]
    public float sensitivity = 0.5f;
    public bool invertY = false;

    [Header("UI Elements")]
  
    [SerializeField] Image meleeCooldownUI;
    [SerializeField] TMP_Text enemyCountText;

    [Header("Trophies")]
    [SerializeField] public GameObject gold;
    [SerializeField] public GameObject silver1;
    [SerializeField] public GameObject silver2;
    [SerializeField] public GameObject bronze1;
    [SerializeField] public GameObject bronze2;
    [SerializeField] public GameObject bronze3;
    [SerializeField] public GameObject bronze4;

    public Staffs currentStaff;

    

    public List<Staffs> allStaffs = new List<Staffs>();

    public Image playerHealthBar;
    public Image playerManaBar;


    public GameObject player;
    public Player playerScript;

    public BoonManager boonManager;
    public cameraController cameraController;

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

        boonManager = GetComponent<BoonManager>();
        cameraController = GetComponent<cameraController>();
        foreach (Staffs staffs in allStaffs)
        {
            staffs.InitializeStaffValues();
        }

        //get settings from settings menu to use in game
        sensitivity = PlayerPrefs.GetFloat("sensValue", 0.5f);
        invertY = PlayerPrefs.GetInt("invertY", 0) == 1;
       
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
            navMeshBakerScript.rebakeNavMesh();
            room1Clear = true;
        }
        else if (enemyCount <= 0 && room1Clear) 
        {
            rewardChest2.SetActive(true);
            navMeshBakerScript.rebakeNavMesh();
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
        menuActive = menuLose;
        menuActive.SetActive(true);
        statePaused();
       
    }
    public void boonSelection()
    {
        statePaused();
        boonManager.randomizeList();
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

    public void settingsMenu()
    {
       
        menuActive = menuSettings;
        menuActive.SetActive(true);
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
        
    }

    public void UpdateMeleeCooldownUI(float meleeCooldown, float lastMeleeTime)
    {
        StartCoroutine(MeleeCooldownCoroutine(meleeCooldown, lastMeleeTime));
    }

    IEnumerator MeleeCooldownCoroutine(float meleeCooldown, float lastMeleeTime)
    {
        while (Time.time < lastMeleeTime + meleeCooldown)
        {
            float cooldownRemaining = Mathf.Clamp01((Time.time - lastMeleeTime) / meleeCooldown);
            if (meleeCooldownUI != null)
            {
                meleeCooldownUI.fillAmount = cooldownRemaining;
            }
            yield return null;
        }
    }


}
