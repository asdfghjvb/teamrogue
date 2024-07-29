using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Saves changingSave;

    [SerializeField] GameObject upgradeMenu;
    [SerializeField] TMP_Text healthButton;
    [SerializeField] TMP_Text armorButton;
    [SerializeField] TMP_Text speedButton;
    [SerializeField] TMP_Text sprintButton;
    [SerializeField] TMP_Text jumpButton;
    [SerializeField] TMP_Text rangeButton;
    [SerializeField] TMP_Text sDamageButton;
    [SerializeField] TMP_Text sRateButton;
    [SerializeField] TMP_Text mDamageButton;
    [SerializeField] TMP_Text mRateButton;

    [SerializeField] TMP_Text goldCount;

    [SerializeField] float healthUpgrade;
    [SerializeField] float armorUpgrade;
    [SerializeField] float speedUpgrade;
    [SerializeField] float sprintUpgrade;
    [SerializeField] int jumpUpgrade;
    [SerializeField] int rangeUpgrade;
    [SerializeField] int sDamageUpgrade;
    [SerializeField] float sRateUpgrade;
    [SerializeField] int mDamageUpgrade;
    [SerializeField] float mRateUpgrade;

    [SerializeField] int costMod;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateCosts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCosts(int ID = 0)
    {
        if (ID == 0)
        {
            if (changingSave.healthCost < 10)
            {
                changingSave.healthCost = 10;
            }
            if (changingSave.armorCost < 10)
            {
                changingSave.armorCost = 10;
            }
            if (changingSave.speedCost < 10)
            {
                changingSave.speedCost = 10;
            }
            if (changingSave.sprintCost < 10)
            {
                changingSave.sprintCost = 10;
            }
            if (changingSave.jumpCost < 10)
            {
                changingSave.jumpCost = 10;
            }
            if (changingSave.rangeCost < 10)
            {
                changingSave.rangeCost = 10;
            }
            if (changingSave.sDamageCost < 10)
            {
                changingSave.sDamageCost = 10;
            }
            if (changingSave.sRateCost < 10)
            {
                changingSave.sRateCost = 10;
            }
            if (changingSave.mDamageCost < 10)
            {
                changingSave.mDamageCost = 10;
            }
            if (changingSave.mRateCost < 10)
            {
                changingSave.mRateCost = 10;
            }
        }
        if (ID == 0 || ID == 1)
        {
            healthButton.SetText(changingSave.healthCost.ToString());
        }
        if (ID == 0 || ID == 2)
        {
            armorButton.SetText(changingSave.armorCost.ToString());
        }
        if (ID == 0 || ID == 3)
        {
            speedButton.SetText(changingSave.speedCost.ToString());
        }
        if (ID == 0 || ID == 4)
        {
            sprintButton.SetText(changingSave.sprintCost.ToString());
        }
        if (ID == 0 || ID == 5)
        {
            jumpButton.SetText(changingSave.jumpCost.ToString());
        }
        if (ID == 0 || ID == 6)
        {
            rangeButton.SetText(changingSave.rangeCost.ToString());
        }
        if (ID == 0 || ID == 7)
        {
            sDamageButton.SetText(changingSave.sDamageCost.ToString());
        }
        if (ID == 0 || ID == 8)
        {
            sRateButton.SetText(changingSave.sRateCost.ToString());
        }
        if (ID == 0 || ID == 9)
        {
            mDamageButton.SetText(changingSave.mDamageCost.ToString());
        }
        if (ID == 0 || ID == 10)
        {
            mRateButton.SetText(changingSave.mRateCost.ToString());
        }

        UpdateGold();
    }

    public void UpdateGold()
    {
        goldCount.SetText(GameManager.instance.playerScript.currentGold.ToString());
        changingSave.gold = GameManager.instance.playerScript.currentGold;
    }

    public void Upgrade(int ID)
    {
        switch (ID)
        {
            case 0:
                break;
            case 1:
                UpgradeHealth();
                break;
            case 2:
                UpgradeArmor();
                break;
            case 3:
                UpgradeSpeed();
                break;
            case 4:
                UpgradeSprint();
                break;
            case 5:
                UpgradeJump();
                break;
            case 6:
                UpgradeRange();
                break;
            case 7:
                UpgradeShootDamage();
                break;
            case 8:
                UpgradeShootRate();
                break;
            case 9:
                UpgradeMeleeDamage();
                break;
            case 10:
                UpgradeMeleeRate();
                break;
        }
        GameManager.instance.achievementManager.UpdateCount(7);
    }

    void UpgradeHealth()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.healthCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.healthCost;
            changingSave.healthCost *= costMod;
            GameManager.instance.playerScript.fullHealth += healthUpgrade;
            GameManager.instance.playerScript.health += healthUpgrade;
            changingSave.health += healthUpgrade;
            UpdateCosts(1);
        }
    }
    void UpgradeArmor()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.armorCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.armorCost;
            changingSave.armorCost *= costMod;
            GameManager.instance.playerScript.armorMod *= armorUpgrade;
            changingSave.armorMod *= armorUpgrade;
            UpdateCosts(2);
        }
    }
    void UpgradeSpeed()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.speedCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.speedCost;
            changingSave.speedCost *= costMod;
            GameManager.instance.playerScript.speed += speedUpgrade;
            changingSave.speed += speedUpgrade;
            UpdateCosts(3);
            GameManager.instance.achievementManager.UpdateCount(5);
        }
    }
    void UpgradeSprint()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.sprintCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.sprintCost;
            changingSave.sprintCost *= costMod;
            GameManager.instance.playerScript.sprintMod *= sprintUpgrade;
            changingSave.sprintMod *= sprintUpgrade;
            UpdateCosts(4);
        }
    }
    void UpgradeJump()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.jumpCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.jumpCost;
            changingSave.jumpCost *= costMod;
            GameManager.instance.playerScript.jumpMax += jumpUpgrade;
            changingSave.jumpMax += jumpUpgrade;
            UpdateCosts(5);
        }
    }
    void UpgradeRange()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.rangeCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.rangeCost;
            changingSave.rangeCost *= costMod;
            GameManager.instance.playerScript.innateShootDist += rangeUpgrade;
            changingSave.shootRange += rangeUpgrade;
            UpdateCosts(6);
        }
    }
    void UpgradeShootDamage()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.sDamageCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.sDamageCost;
            changingSave.sDamageCost *= costMod;
            GameManager.instance.playerScript.innateShootDamage += sDamageUpgrade;
            changingSave.shootDamage += sDamageUpgrade;
            UpdateCosts(7);
        }
    }
    void UpgradeShootRate()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.sRateCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.sRateCost;
            changingSave.sRateCost *= costMod;
            GameManager.instance.playerScript.innateShootRate *= sRateUpgrade;
            changingSave.shootRate *= sRateUpgrade;
            UpdateCosts(8);
        }
    }
    void UpgradeMeleeDamage()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.mDamageCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.mDamageCost;
            changingSave.mDamageCost *= costMod;
            GameManager.instance.playerScript.meleeDamage += mDamageUpgrade;
            changingSave.meleeDamage += mDamageUpgrade;
            UpdateCosts(9);
        }
    }
    void UpgradeMeleeRate()
    {
        if (GameManager.instance.playerScript.currentGold >= changingSave.mRateCost)
        {
            GameManager.instance.playerScript.currentGold -= changingSave.mRateCost;
            changingSave.mRateCost *= costMod;
            GameManager.instance.playerScript.meleeCooldown *= mRateUpgrade;
            changingSave.meleeCooldown *= mRateUpgrade;
            UpdateCosts(10);
        }
    }
}
