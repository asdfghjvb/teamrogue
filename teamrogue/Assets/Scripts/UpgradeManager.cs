using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
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

    void UpdateCosts()
    {
        healthButton.SetText(healthCost.ToString());
        armorButton.SetText(armorCost.ToString());
        speedButton.SetText(speedCost.ToString());
        sprintButton.SetText(sprintCost.ToString());
        jumpButton.SetText(jumpCost.ToString());
        rangeButton.SetText(rangeCost.ToString());
        sDamageButton.SetText(sDamageCost.ToString());
        sRateButton.SetText(sRateCost.ToString());
        mDamageButton.SetText(mDamageCost.ToString());
        mRateButton.SetText(mRateCost.ToString());
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
    }

    void UpgradeHealth()
    {
        // if currency >= cost
        healthCost *= costMod;
        // remove healthCost from currency
        GameManager.instance.playerScript.fullHealth += healthUpgrade;
        GameManager.instance.playerScript.health += healthUpgrade;
        UpdateCosts();
    }
    void UpgradeArmor()
    {
        // if currency >= cost
        armorCost *= costMod;
        // remove armorCost from currency
        GameManager.instance.playerScript.armorMod *= armorUpgrade;
        UpdateCosts();
    }
    void UpgradeSpeed()
    {
        // if currency >= cost
        speedCost *= costMod;
        // remove speedCost from currency
        GameManager.instance.playerScript.speed += speedUpgrade;
        UpdateCosts();
    }
    void UpgradeSprint()
    {
        // if currency >= cost
        sprintCost *= costMod;
        // remove sprintCost from currency
        GameManager.instance.playerScript.sprintMod *= sprintUpgrade;
        UpdateCosts();
    }
    void UpgradeJump()
    {
        // if currency >= cost
        jumpCost *= costMod;
        // remove jumpCost from currency
        GameManager.instance.playerScript.jumpMax += jumpUpgrade;
        UpdateCosts();
    }
    void UpgradeRange()
    {
        // if currency >= cost
        rangeCost *= costMod;
        // remove rangeCost from currency
        GameManager.instance.playerScript.innateShootDist += rangeUpgrade;
        UpdateCosts();
    }
    void UpgradeShootDamage()
    {
        // if currency >= cost
        sDamageCost *= costMod;
        // remove sDamageCost from currency
        GameManager.instance.playerScript.jumpMax += sDamageUpgrade;
        UpdateCosts();
    }
    void UpgradeShootRate()
    {
        // if currency >= cost
        sRateCost *= costMod;
        // remove sRateCost from currency
        GameManager.instance.playerScript.innateShootRate *= sRateUpgrade;
        UpdateCosts();
    }
    void UpgradeMeleeDamage()
    {
        // if currency >= cost
        mDamageCost *= costMod;
        // remove mDamageCost from currency
        GameManager.instance.playerScript.meleeDamage += mDamageUpgrade;
        UpdateCosts();
    }
    void UpgradeMeleeRate()
    {
        // if currency >= cost
        mRateCost *= costMod;
        // remove mRateCost from currency
        GameManager.instance.playerScript.meleeCooldown *= mRateUpgrade;
        UpdateCosts();
    }
}
