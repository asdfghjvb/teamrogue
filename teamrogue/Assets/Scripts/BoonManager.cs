using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using TMPro;


public class BoonManager : MonoBehaviour
{
    public static BoonManager instance;

    [SerializeField] GameObject boonMenu;
    [SerializeField] Collider boonCol;

    [SerializeField] TMP_Text option1;
    [SerializeField] TMP_Text option2;
    [SerializeField] TMP_Text option3;

    bool boonSelected = false;

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
        //instance = this;
        randomizeList();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.playerScript.staffList.Count > 0 && boonSelected == false)
        {
            boonCol.enabled = true;
            boonSelected = true;
        }
    }
    public void randomizeList()
    {
        System.Random rand = new System.Random();
        boonList.OrderBy(x => rand.Next()).ToList();
        option1.text = boonList[0].Item2;
        option2.text = boonList[1].Item2;
        option3.text = boonList[2].Item2;

    }

    public void ApplyBoon(int ID)
    {
        switch (ID)
        {
            case 1:
                GameManager.instance.playerScript.fullHealth += 5;
                GameManager.instance.playerScript.health += 5;
                break;
            case 2:
                GameManager.instance.playerScript.shootRate *= 0.75f;
                break;
            case 3:
                GameManager.instance.playerScript.speed += 5;
                break;
            case 4:
                GameManager.instance.playerScript.jumpMax += 1;
                break;
            case 5:
                GameManager.instance.playerScript.shootDamage += 3;
                break;
            case 6:
                GameManager.instance.playerScript.armorMod *= 0.9f;
                break;
            case 7:
                GameManager.instance.playerScript.sprintMod *= 1.2f;
                break;
            case 8:
                GameManager.instance.playerScript.shootDist += 10;
                break;
            case 9:
                GameManager.instance.playerScript.meleeDamage += 3;
                break;
            case 10:
                GameManager.instance.playerScript.meleeCooldown *= 0.8f;
                break;
        }
    }

    public void boonOption1()
    {
        GameManager.instance.applyBoon(boonList[0].Item1);
        GameManager.instance.boonCount += 1;
        boonMenu.SetActive(false);
        GameManager.instance.stateUnpaused();
    }
    public void boonOption2()
    {
        GameManager.instance.applyBoon(boonList[1].Item1);
        GameManager.instance.boonCount += 1;
        boonMenu.SetActive(false);
        GameManager.instance.stateUnpaused();
    }
    public void boonOption3()
    {
        GameManager.instance.applyBoon(boonList[2].Item1);
        GameManager.instance.boonCount += 1;
        boonMenu.SetActive(false);
        GameManager.instance.stateUnpaused();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.boonCount < 1)
        {
            boonCol.enabled = false;
            GameManager.instance.boonSelection();
        }
        
        
        
    }
}
