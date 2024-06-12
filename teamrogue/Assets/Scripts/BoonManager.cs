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
    [SerializeField] GameObject boonMenu;

    [SerializeField] TMP_Text option1;
    [SerializeField] TMP_Text option2;
    [SerializeField] TMP_Text option3;

    List<(int, string)> boonlist = new List<(int, string)>{
        (1,"Increase Health"),
        (2,"Increase Fire Rate"),
        (3,"Increase Speed"),
        (4,"Extra Jump"),
        (5,"Increase Damage"),
        (6,"Add Armor"),
        (7,"Increased Sprint"),
        (8,"Increase Range") };
    //(9,"Decrease Enemy Speed"),
    //(10,"Decrease Enemy Health"),
    List<(int, string)> randList;

    // Start is called before the first frame update
    void Start()
    {
        randList = randomizeList(boonlist);
        option1.text = randList[0].Item2;
        option2.text = randList[1].Item2;
        option3.text = randList[2].Item2;


    }

    // Update is called once per frame
    void Update()
    {

    }
    List<(int, string)> randomizeList(List<(int, string)> list)
    {
        System.Random rand = new System.Random();
        return list.OrderBy(x => rand.Next()).ToList();

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
        }
    }

    public void boonOption1()
    {
        ApplyBoon(randList[0].Item1);
        boonMenu.SetActive(false);
        GameManager.instance.stateUnpaused();
    }
    public void boonOption2()
    {
        ApplyBoon(randList[1].Item1);
        boonMenu.SetActive(false);
        GameManager.instance.stateUnpaused();
    }
    public void boonOption3()
    {
        ApplyBoon(randList[2].Item1);
        boonMenu.SetActive(false);
        GameManager.instance.stateUnpaused();
    }
}
