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

    [Header("Options")]
    //options will go here
    bool placeholder1;

    [Header("Achievements")]
    //achievement bools will go here
    bool placeholder2;

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

    }

}
