using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using System;
using UnityEngine.SceneManagement;

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
    [SerializeField] public GameObject upgradeUI;
    [SerializeField] public GameObject signUI;
    [SerializeField] public GameObject trophyMenu;

    [SerializeField] public GameObject inputHint;

    [SerializeField] public GameObject rewardChest1;
    [SerializeField] public GameObject rewardChest2;
    [SerializeField] public GameObject healButton;
    [SerializeField] public GameObject boonButton;

    [Header("Player Settings")]
    public float sensitivity = 0.5f;
    public bool invertY = false;

    [Header("UI Elements")]
  
    [SerializeField] Image meleeCooldownUI;
    [SerializeField] TMP_Text enemyCountText;

    [SerializeField] TMP_Text currentMana;
    [SerializeField] TMP_Text fullMana;
    [SerializeField] TMP_Text currentHealth;
    [SerializeField] TMP_Text fullHealth;

    [Header("Trophies")]
    [SerializeField] public GameObject gold;
    public bool goldEarned = false;
    [SerializeField] public GameObject silver1;
    public bool silver1Earned = false;
    [SerializeField] public GameObject silver2;
    public bool silver2Earned = false;
    [SerializeField] public GameObject bronze1;
    public bool bronze1Earned = false;
    [SerializeField] public GameObject bronze2;
    public bool bronze2Earned = false;
    [SerializeField] public GameObject bronze3;
    public bool bronze3Earned = false;
    [SerializeField] public GameObject bronze4;
    public bool bronze4Earned = false;

    [Header("Misc")]
    [SerializeField] public UpgradeManager upgradeManager;


    public Staffs currentStaff;

    

    public List<Staffs> allStaffs = new List<Staffs>();

    public Image playerHealthBar;
    public Image playerManaBar;


    public GameObject player;
    public Player playerScript;

    public BoonManager boonManager;
    public AchievementManager achievementManager;
    public cameraController cameraController;

    public int seed;

    public bool isPaused;
    public bool saveMenuActive = false;
    int enemyCount;
    public int boonCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 30;
        seed = (int)DateTime.Now.Ticks;
        UnityEngine.Random.InitState(seed);

        player = GameObject.FindWithTag("Player");
        if(player!= null)
            playerScript = player.GetComponent<Player>();

        boonManager = GetComponent<BoonManager>();
        achievementManager = GetComponent<AchievementManager>();
        cameraController = GetComponent<cameraController>();
        foreach (Staffs staffs in allStaffs)
        {
            staffs.InitializeStaffValues();
        }

        //get settings from settings menu to use in game
        sensitivity = PlayerPrefs.GetFloat("sensValue", 0.5f);
        invertY = PlayerPrefs.GetInt("invertY", 0) == 1;
       
    }

    private void Start()
    {
        if (IsInScene("Hub"))
        {
            if (goldEarned && silver1Earned && silver2Earned && bronze1Earned && bronze2Earned && bronze3Earned && bronze4Earned)
            {
                StartCoroutine(winMenuDelay());
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (IsInScene("Main Menu"))
            {
                return;
            }
            if (menuActive == null)
            {
                statePaused();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }

            else if (menuActive == menuPause || menuActive == menuSettings || menuActive == signUI || menuActive == upgradeUI || menuActive == trophyMenu && !saveMenuActive)
            {
                stateUnpaused();
            }
        }

        updateBars();
      
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

    }

    public void updateBars()
    {
        currentMana.text = playerScript.mana.ToString("F0");
        fullMana.text = playerScript.fullMana.ToString("F0");
        currentHealth.text = playerScript.health.ToString("F0");
        fullHealth.text = playerScript.fullHealth.ToString("F0");
    }
    public void youWin()
    {
        
        statePaused();
        menuActive = menuWin;
        menuActive.SetActive(isPaused);
    }

    IEnumerator winMenuDelay()
    {
        yield return new WaitForSeconds(0.1f);
        youWin();
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

    public void ModifySeed(int multiplier)
    {
        unchecked
        {
            seed *= multiplier;
        }
    }

    public bool IsInScene(string sceneName)
    {
        return SceneManager.GetActiveScene().name == sceneName;
    }

    public void UpdatePlayerCurrency(int amount)
    {
        playerScript.currentGold += amount;
    }
}
