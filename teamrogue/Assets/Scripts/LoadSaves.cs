using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSaves : MonoBehaviour
{
    [SerializeField] Saves saveFile;
    [SerializeField] Saves changingSave;

    public void SaveGame()
    {
        saveFile.save();
    }

    public void LoadSave()
    {
        CopySave();
       
        changingSave.load();
       
        
       
        SceneManager.LoadScene("Hub");
    }

    public void CopySave()
    {
        changingSave.health = saveFile.health;
        changingSave.speed = saveFile.speed;
        changingSave.sprintMod = saveFile.sprintMod;
        changingSave.armorMod = saveFile.armorMod;
        changingSave.jumpMax = saveFile.jumpMax;
        changingSave.meleeDamage = saveFile.meleeDamage;
        changingSave.meleeCooldown = saveFile.meleeCooldown;
        changingSave.shootRange = saveFile.shootRange;
        changingSave.shootDamage = saveFile.shootDamage;
        changingSave.shootRate = saveFile.shootRate;
        
        changingSave.bronzeAch1 = saveFile.bronzeAch1;
        changingSave.bronzeAch2 = saveFile.bronzeAch2;
        changingSave.bronzeAch3 = saveFile.bronzeAch3;
        changingSave.bronzeAch4 = saveFile.bronzeAch4;
        changingSave.silverAch1 = saveFile.silverAch1;
        changingSave.silverAch2 = saveFile.silverAch2;
        changingSave.goldAch = saveFile.goldAch;

    }

    public void Start()
    {
        Debug.Log("start called");
        if (GameManager.instance.IsInScene("Hub"))
        {
            GameManager.instance.goldEarned = changingSave.goldAch;
            GameManager.instance.silver1Earned = changingSave.silverAch1;
            GameManager.instance.silver2Earned = changingSave.silverAch2;
            GameManager.instance.bronze1Earned = changingSave.bronzeAch1;
            GameManager.instance.bronze2Earned = changingSave.bronzeAch2;
            GameManager.instance.bronze3Earned = changingSave.bronzeAch3;
            GameManager.instance.bronze4Earned = changingSave.bronzeAch4;
        }
        changingSave.load();
    }

    public void ClearSave()
    {
        saveFile.ClearSaveFile();
    }
   
    public void NewGameSelected()
    {
        changingSave.ClearSaveFile();
    }
}
