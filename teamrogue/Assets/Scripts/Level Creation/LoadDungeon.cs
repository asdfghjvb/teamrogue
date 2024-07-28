using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDungeon : MonoBehaviour
{
    public static bool initialBoon;
    public static bool hasValue;

    public static float health;
    public static float speed;
    public static float sprintMod;
    public static float armorMod;
    public static int jumpMax;
    public static int meleeDamage;
    public static int shootDamage;
    public static float shootRate;
    public static int shootRange;
    public static float meleeCooldown;
    public static int gold;

    public static List<Staffs> staffList;


    [SerializeField] Saves changingSave;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.boonSelection();
        initialBoon = true;

        //Save staff list to be moved over between scenes
        staffList = GameManager.instance.playerScript.staffList;

        //save preboon values, reset in
        health = changingSave.health;
        speed = changingSave.speed; 
        sprintMod = changingSave.sprintMod;
        armorMod = changingSave.armorMod;
        jumpMax = changingSave.jumpMax;
        meleeDamage = changingSave.meleeDamage;
        meleeCooldown = changingSave.meleeCooldown;
        shootDamage = changingSave.shootDamage;
        shootRate = changingSave.shootRate;
        shootRange = changingSave.shootRange;
        gold = changingSave.gold;

        hasValue = true;
    }
}
