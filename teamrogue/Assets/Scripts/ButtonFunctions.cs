using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClick;
    public static ButtonFunctions instance;

    private void Start()
    {
        instance = this;
    }
    public void resume()
    {
        GameManager.instance.stateUnpaused();
    }

    public void restart()
    {
        GameManager.instance.playerScript.health = LoadDungeon.health;
        GameManager.instance.playerScript.speed = LoadDungeon.speed;
        GameManager.instance.playerScript.sprintMod = LoadDungeon.sprintMod;
        GameManager.instance.playerScript.armorMod = LoadDungeon.armorMod;
        GameManager.instance.playerScript.jumpMax = LoadDungeon.jumpMax;
        GameManager.instance.playerScript.meleeDamage = LoadDungeon.meleeDamage;
        GameManager.instance.playerScript.innateShootDamage = LoadDungeon.shootDamage;
        GameManager.instance.playerScript.innateShootDist = LoadDungeon.shootRange;
        GameManager.instance.playerScript.innateShootRate = LoadDungeon.shootRate;
        GameManager.instance.playerScript.staffList = LoadDungeon.staffList;

        StartCoroutine(restartDelay());
        GameManager.instance.stateUnpaused();
        GameManager.instance.boonSelection();

    }
  

    public void quit()
    {
       
        foreach (Staffs staff in GameManager.instance.playerScript.staffList)
        {
            staff.ResetStaffStats();
        }
        StartCoroutine(clickDelay());
        if (GameManager.instance.IsInScene("Hub"))
        {
            SceneManager.LoadScene("Main Menu");
        }
        else if (GameManager.instance.IsInScene("Dungeon"))
        {
            GameManager.instance.playerScript.health = LoadDungeon.health;
            GameManager.instance.playerScript.speed = LoadDungeon.speed;
            GameManager.instance.playerScript.sprintMod = LoadDungeon.sprintMod;
            GameManager.instance.playerScript.armorMod = LoadDungeon.armorMod;
            GameManager.instance.playerScript.jumpMax = LoadDungeon.jumpMax;
            GameManager.instance.playerScript.meleeDamage = LoadDungeon.meleeDamage;
            GameManager.instance.playerScript.innateShootDamage = LoadDungeon.shootDamage;
            GameManager.instance.playerScript.innateShootDist = LoadDungeon.shootRange;
            GameManager.instance.playerScript.innateShootRate = LoadDungeon.shootRate;
            GameManager.instance.playerScript.staffList = LoadDungeon.staffList;
            GameManager.instance.playerHealthBar.rectTransform.sizeDelta = LoadDungeon.size;
            GameManager.instance.playerHealthBarBack.rectTransform.sizeDelta = LoadDungeon.size2;
            SceneManager.LoadScene("Hub");
        }
    }

    IEnumerator clickDelay()
    {
        playClickSound();
        yield return new WaitForSeconds(.1f);
      
    }
   
    
    public void healReward()
    {
        playClickSound();
        if (GameManager.instance.playerScript.health <= GameManager.instance.playerScript.fullHealth / 2)
            GameManager.instance.playerScript.health += GameManager.instance.playerScript.fullHealth / 2;
        else
            GameManager.instance.playerScript.health = GameManager.instance.playerScript.fullHealth;

        if (GameManager.instance.playerScript.mana <= GameManager.instance.playerScript.fullMana / 2)
            GameManager.instance.playerScript.mana += GameManager.instance.playerScript.fullMana / 2;
        else
            GameManager.instance.playerScript.mana = GameManager.instance.playerScript.fullMana;
        GameManager.instance.playerScript.updatePlayerUI();
        GameManager.instance.healButton.SetActive(false);
        GameManager.instance.boonButton.SetActive(true);
    }


    public void playClickSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }
   
    IEnumerator restartDelay()
    {
        playClickSound();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    
}
