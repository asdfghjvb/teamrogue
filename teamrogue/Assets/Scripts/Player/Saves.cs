using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Saves : ScriptableObject
{
    [Header("Player Stats")]
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float sprintMod;
    [SerializeField] private float armorMod;
    [SerializeField] private int jumpMax; // this may be modified to dodgeMax or dodgeDist in the future
    [SerializeField] private int meleeDamage;
    [SerializeField] private int shootDamage;
    [SerializeField] private float shootRate;
    [SerializeField] private int shootRange;
    [SerializeField] private float meleeCooldown;

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

        GameManager.instance.gold.SetActive(goldAch);
        GameManager.instance.silver1.SetActive(silverAch1);
        GameManager.instance.silver2.SetActive(silverAch2);
        GameManager.instance.bronze1.SetActive(bronzeAch1);
        GameManager.instance.bronze2.SetActive(bronzeAch2);
        GameManager.instance.bronze3.SetActive(bronzeAch3);
        GameManager.instance.bronze4.SetActive(bronzeAch4);

    }

}
