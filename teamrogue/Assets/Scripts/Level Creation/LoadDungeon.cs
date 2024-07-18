using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDungeon : MonoBehaviour
{
    public static bool initialBoon;

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
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.boonSelection();
        initialBoon = true;

        //save preboon values, reset in
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
}
