using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;

public class BoonManager : MonoBehaviour
{
    

    [SerializeField] GameObject boonMenu;
    [SerializeField] GameObject startingBoon;

    [SerializeField] TMP_Text option1;
    [SerializeField] TMP_Text option2;
    [SerializeField] TMP_Text option3;

    [SerializeField] Saves changingSave;

    List<(int, string)> newList;


    List<(int, string)> boonList = new List<(int, string)>{
        (1,"Increase Health"),
        (2,"Increase Fire Rate"),
        (3,"Increase Speed"),
        (4,"Extra Jump"),
        (5,"Increase Damage"),
        (6,"Add Armor"),
        (7,"Increased Sprint"),
        (8,"Increase Range"),
        (9,"Increase Melee Attack"),
        (10,"Reduce Melee Cooldown"), };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void randomizeList()
    {
        System.Random rand = new System.Random();
        newList = boonList.OrderBy(x => rand.Next()).ToList();
        option1.text = newList[0].Item2;
        option2.text = newList[1].Item2;
        option3.text = newList[2].Item2;

    }

    public void ApplyBoon(int ID)
    {
        switch (ID)
        {
            case 1:
                GameManager.instance.playerScript.fullHealth += 5;
                GameManager.instance.playerScript.health += 5;
                changingSave.health += 5;
                break;
            case 2:
                GameManager.instance.playerScript.innateShootRate *= 0.75f;
                changingSave.shootRate *= 0.75f;
                break;
            case 3:
                GameManager.instance.playerScript.speed += 5;
                GameManager.instance.achievementManager.UpdateCount(5);
                changingSave.speed += 5;
                break;
            case 4:
                GameManager.instance.playerScript.jumpMax += 1;
                changingSave.jumpMax += 1;
                break;
            case 5:
                GameManager.instance.playerScript.innateShootDamage += 3;
                changingSave.shootDamage += 3;
                break;
            case 6:
                GameManager.instance.playerScript.armorMod *= 0.9f;
                changingSave.armorMod *= 0.9f;
                break;
            case 7:
                GameManager.instance.playerScript.sprintMod *= 1.2f;
                changingSave.sprintMod *= 1.2f;
                break;
            case 8:
                GameManager.instance.playerScript.innateShootDist += 10;
                changingSave.shootRange += 10;
                break;
            case 9:
                GameManager.instance.playerScript.meleeDamage += 3;
                changingSave.meleeDamage += 3;
                break;
            case 10:
                GameManager.instance.playerScript.meleeCooldown *= 0.8f;
                changingSave.meleeCooldown *= 0.8f;
                break;
        }
        GameManager.instance.achievementManager.UpdateCount(6);
    }

    public void boonOption1()
    {
        ApplyBoon(newList[0].Item1);
        boonMenu.SetActive(false);
        GameManager.instance.stateUnpaused();
        if (LoadDungeon.initialBoon)
        {
            SceneManager.LoadScene("Dungeon");
            LoadDungeon.initialBoon = false;
        }
    }
    public void boonOption2()
    {
        ApplyBoon(newList[1].Item1);
        boonMenu.SetActive(false);
        GameManager.instance.stateUnpaused();
        if (LoadDungeon.initialBoon)
        {
            SceneManager.LoadScene("Dungeon");
            LoadDungeon.initialBoon = false;
        }
    }
    public void boonOption3()
    {
        ApplyBoon(newList[2].Item1);
        boonMenu.SetActive(false);
        GameManager.instance.stateUnpaused();
        if (LoadDungeon.initialBoon)
        {
            SceneManager.LoadScene("Dungeon");
            LoadDungeon.initialBoon = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.boonCount < 1 && other.CompareTag("Player"))
        {
            GameManager.instance.boonSelection();
            startingBoon.SetActive(false);
        }
        
        
        
    }
}
