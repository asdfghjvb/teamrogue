using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class Saves : ScriptableObject
{
    [Header("Player Stats")]
    [SerializeField] public float health;
    [SerializeField] public float speed;
    [SerializeField] public float sprintMod;
    [SerializeField] public float armorMod;
    [SerializeField] public int jumpMax; 
    [SerializeField] public int meleeDamage;
    [SerializeField] public int shootDamage;
    [SerializeField] public float shootRate;
    [SerializeField] public int shootRange;
    [SerializeField] public float meleeCooldown;
    [SerializeField] public int gold;

    [Header("Achievements")]
    public bool bronzeAch1;
    public bool bronzeAch2;
    public bool bronzeAch3;
    public bool bronzeAch4;
    public bool silverAch1;
    public bool silverAch2;
    public bool goldAch;

    public void save()
    {
        health = GameManager.instance.playerScript.health;
        speed = GameManager.instance.playerScript.speed;
        sprintMod = GameManager.instance.playerScript.sprintMod;
        armorMod = GameManager.instance.playerScript.armorMod;
        jumpMax = GameManager.instance.playerScript.jumpMax;
        meleeDamage = GameManager.instance.playerScript.meleeDamage;
        meleeCooldown = GameManager.instance.playerScript.meleeCooldown;
        shootDamage = GameManager.instance.playerScript.innateShootDamage;
        shootRate = GameManager.instance.playerScript.innateShootRate;
        shootRange = GameManager.instance.playerScript.innateShootDist;
        gold = GameManager.instance.playerScript.currentGold;

        goldAch = GameManager.instance.gold.activeSelf;
        silverAch1 = GameManager.instance.silver1.activeSelf;
        silverAch2 = GameManager.instance.silver2.activeSelf;
        bronzeAch1 = GameManager.instance.bronze1.activeSelf;
        bronzeAch2 = GameManager.instance.bronze2.activeSelf;
        bronzeAch3 = GameManager.instance.bronze3.activeSelf;
        bronzeAch4 = GameManager.instance.bronze4.activeSelf;
    }
    public void load()
    {
        GameManager.instance.playerScript.health = health;
        GameManager.instance.playerScript.speed = speed;
        GameManager.instance.playerScript.sprintMod = sprintMod;
        GameManager.instance.playerScript.armorMod = armorMod;
        GameManager.instance.playerScript.jumpMax = jumpMax;
        GameManager.instance.playerScript.meleeDamage = meleeDamage;
        GameManager.instance.playerScript.innateShootDamage = shootDamage;
        GameManager.instance.playerScript.innateShootDist = shootRange;
        GameManager.instance.playerScript.innateShootRate = shootRate;
        GameManager.instance.playerScript.currentGold = gold;

        GameManager.instance.gold.SetActive(goldAch);
        GameManager.instance.goldEarned = goldAch;
        GameManager.instance.silver1.SetActive(silverAch1);
        GameManager.instance.silver1Earned =silverAch1;
        GameManager.instance.silver2.SetActive(silverAch2);
        GameManager.instance.silver2Earned = silverAch2;
        GameManager.instance.bronze1.SetActive(bronzeAch1);
        GameManager.instance.bronze1Earned = bronzeAch1;
        GameManager.instance.bronze2.SetActive(bronzeAch2);
        GameManager.instance.bronze2Earned = bronzeAch2;
        GameManager.instance.bronze3.SetActive(bronzeAch3);
        GameManager.instance.bronze3Earned = bronzeAch3;
        GameManager.instance.bronze4.SetActive(bronzeAch4);
        GameManager.instance.bronze4Earned = bronzeAch4;

       // SceneManager.LoadScene("Hub");

    }

    public void Start()
    {
        Debug.Log("start called");
        if (GameManager.instance.IsInScene("Hub"))
        {
            GameManager.instance.goldEarned = goldAch;
            GameManager.instance.silver1Earned = silverAch1;
            GameManager.instance.silver2Earned = silverAch2;
            GameManager.instance.bronze1Earned = bronzeAch1;
            GameManager.instance.bronze2Earned = bronzeAch2;
            GameManager.instance.bronze3Earned = bronzeAch3;
            GameManager.instance.bronze4Earned = bronzeAch4;
        }
        
    }
    public void ClearSaveFile()
    {
        health = 100;
        speed = 5;
        sprintMod = 2;
        armorMod = 1;
        jumpMax = 1; 
        meleeDamage = 2;
        shootDamage = 10;
        shootRate = 0.5f;
        shootRange = 10;
        meleeCooldown = 0.5f;
        gold = 0;

        bronzeAch1 = false;
        bronzeAch2 = false;
        bronzeAch3 = false;
        bronzeAch4 = false;
        silverAch1 = false;
        silverAch2 = false;
        goldAch = false;
}

}
