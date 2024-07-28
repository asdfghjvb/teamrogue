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
    
    [SerializeField] int healthCost;
    [SerializeField] int armorCost;
    [SerializeField] int speedCost;
    [SerializeField] int sprintCost;
    [SerializeField] int jumpCost;
    [SerializeField] int rangeCost;
    [SerializeField] int sDamageCost;
    [SerializeField] int sRateCost;
    [SerializeField] int mDamageCost;
    [SerializeField] int mRateCost;
    
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
        if (ID == 0 || ID == 1)
        {
            healthButton.SetText(healthCost.ToString());
        }
        if (ID == 0 || ID == 2)
        {
            armorButton.SetText(armorCost.ToString());
        }
        if (ID == 0 || ID == 3)
        {
            speedButton.SetText(speedCost.ToString());
        }
        if (ID == 0 || ID == 4)
        {
            sprintButton.SetText(sprintCost.ToString());
        }
        if (ID == 0 || ID == 5)
        {
            jumpButton.SetText(jumpCost.ToString());
        }
        if (ID == 0 || ID == 6)
        {
            rangeButton.SetText(rangeCost.ToString());
        }
        if (ID == 0 || ID == 7)
        {
            sDamageButton.SetText(sDamageCost.ToString());
        }
        if (ID == 0 || ID == 8)
        {
            sRateButton.SetText(sRateCost.ToString());
        }
        if (ID == 0 || ID == 9)
        {
            mDamageButton.SetText(mDamageCost.ToString());
        }
        if (ID == 0 || ID == 10)
        {
            mRateButton.SetText(mRateCost.ToString());
        }

        UpdateGold();
    }

    public void UpdateGold()
    {
        goldCount.SetText(GameManager.instance.playerScript.currentGold.ToString());
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
        if (GameManager.instance.playerScript.currentGold >= healthCost)
        {
            GameManager.instance.playerScript.currentGold -= healthCost;
            healthCost *= costMod;
            GameManager.instance.playerScript.fullHealth += healthUpgrade;
            GameManager.instance.playerScript.health += healthUpgrade;
            changingSave.health += healthUpgrade;
            UpdateCosts(1);
        }
    }
    void UpgradeArmor()
    {
        if (GameManager.instance.playerScript.currentGold >= armorCost)
        {
            GameManager.instance.playerScript.currentGold -= armorCost;
            armorCost *= costMod;
            GameManager.instance.playerScript.armorMod *= armorUpgrade;
            changingSave.armorMod *= armorUpgrade;
            UpdateCosts(2);
        }
    }
    void UpgradeSpeed()
    {
        if (GameManager.instance.playerScript.currentGold >= speedCost)
        {
            GameManager.instance.playerScript.currentGold -= speedCost;
            speedCost *= costMod;
            GameManager.instance.playerScript.speed += speedUpgrade;
            changingSave.speed += speedUpgrade;
            UpdateCosts(3);
            GameManager.instance.achievementManager.UpdateCount(5);
        }
    }
    void UpgradeSprint()
    {
        if (GameManager.instance.playerScript.currentGold >= sprintCost)
        {
            GameManager.instance.playerScript.currentGold -= sprintCost;
            sprintCost *= costMod;
            GameManager.instance.playerScript.sprintMod *= sprintUpgrade;
            changingSave.sprintMod *= sprintUpgrade;
            UpdateCosts(4);
        }
    }
    void UpgradeJump()
    {
        if (GameManager.instance.playerScript.currentGold >= jumpCost)
        {
            GameManager.instance.playerScript.currentGold -= jumpCost;
            jumpCost *= costMod;
            GameManager.instance.playerScript.jumpMax += jumpUpgrade;
            changingSave.jumpMax += jumpUpgrade;
            UpdateCosts(5);
        }
    }
    void UpgradeRange()
    {
        if (GameManager.instance.playerScript.currentGold >= rangeCost)
        {
            GameManager.instance.playerScript.currentGold -= rangeCost;
            rangeCost *= costMod;
            GameManager.instance.playerScript.innateShootDist += rangeUpgrade;
            changingSave.shootRange += rangeUpgrade;
            UpdateCosts(6);
        }
    }
    void UpgradeShootDamage()
    {
        if (GameManager.instance.playerScript.currentGold >= sDamageCost)
        {
            GameManager.instance.playerScript.currentGold -= sDamageCost;
            sDamageCost *= costMod;
            GameManager.instance.playerScript.innateShootDamage += sDamageUpgrade;
            changingSave.shootDamage += sDamageUpgrade;
            UpdateCosts(7);
        }
    }
    void UpgradeShootRate()
    {
        if (GameManager.instance.playerScript.currentGold >= sRateCost)
        {
            GameManager.instance.playerScript.currentGold -= sRateCost;
            sRateCost *= costMod;
            GameManager.instance.playerScript.innateShootRate *= sRateUpgrade;
            changingSave.shootRate *= sRateUpgrade;
            UpdateCosts(8);
        }
    }
    void UpgradeMeleeDamage()
    {
        if (GameManager.instance.playerScript.currentGold >= mDamageCost)
        {
            GameManager.instance.playerScript.currentGold -= mDamageCost;
            mDamageCost *= costMod;
            GameManager.instance.playerScript.meleeDamage += mDamageUpgrade;
            changingSave.meleeDamage += mDamageUpgrade;
            UpdateCosts(9);
        }
    }
    void UpgradeMeleeRate()
    {
        if (GameManager.instance.playerScript.currentGold >= mRateCost)
        {
            GameManager.instance.playerScript.currentGold -= mRateCost;
            mRateCost *= costMod;
            GameManager.instance.playerScript.meleeCooldown *= mRateUpgrade;
            changingSave.meleeCooldown *= mRateUpgrade;
            UpdateCosts(10);
        }
    }
}
