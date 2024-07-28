using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] Saves changingSave;

    int silver1Progress;
    int silver2Progress;
    int bronze1Progress;
    int bronze2Progress;
    int bronze3Progress;
    int bronze4Progress;
    
    // Start is called before the first frame update
    void Start()
    {
        CheckCompletion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCount(int ID)
    {
        switch (ID)
        {
            case 0:
                break;
            case 2:
                UpdateSilver1();
                break;
            case 3:
                UpdateSilver2();
                break;
            case 4:
                UpdateBronze1();
                break;
            case 5:
                UpdateBronze2();
                break;
            case 6:
                UpdateBronze3();
                break;
            case 7:
                UpdateBronze4();
                break;
        }
    }
    public void CheckCompletion()
    {
        if (silver1Progress > 0 && silver2Progress > 0)
        {
            GameManager.instance.gold.SetActive(true);
            changingSave.goldAch = true;
        }
        if (silver1Progress > 0)
        {
            GameManager.instance.silver1.SetActive(true);
            changingSave.silverAch1 = true;
        }
        if (silver2Progress > 0)
        {
            GameManager.instance.silver2.SetActive(true);
            changingSave.silverAch2 = true;
        }
        if (bronze1Progress >= 10)
        {
            GameManager.instance.bronze1.SetActive(true);
            changingSave.bronzeAch1 = true;
        }
        if (bronze2Progress >= 40)
        {
            GameManager.instance.bronze2.SetActive(true);
            changingSave.bronzeAch2 = true;
        }
        if (bronze3Progress >= 5)
        {
            GameManager.instance.bronze3.SetActive(true);
            changingSave.bronzeAch3 = true;
        }
        if (bronze4Progress >= 5)
        {
            GameManager.instance.bronze4.SetActive(true);
            changingSave.bronzeAch4 = true;
        }
    }

    void UpdateSilver1()
    {
        changingSave.silverProg1++;
        CheckCompletion();
    }
    void UpdateSilver2()
    {
        changingSave.silverProg2++;
        CheckCompletion();
    }
    void UpdateBronze1()
    {
        changingSave.bronzeProg1++;
        CheckCompletion();
    }
    void UpdateBronze2()
    {
        changingSave.bronzeProg2 = (int)GameManager.instance.playerScript.speed;
        CheckCompletion();
    }
    void UpdateBronze3()
    {
        changingSave.bronzeProg3++;
        CheckCompletion();
    }
    void UpdateBronze4()
    {
        changingSave.bronzeProg4++;
        CheckCompletion();
    }
}
