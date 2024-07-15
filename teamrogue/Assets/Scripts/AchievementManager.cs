using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    int goldProgress;
    int silver1Progress;
    int silver2Progress;
    int bronze1Progress;
    int bronze2Progress;
    
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
            case 1:
                UpdateGold();
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
        }
    }
    public void CheckCompletion()
    {
        if (goldProgress > 0)
        {
            GameManager.instance.gold.SetActive(true);
        }
        if (silver1Progress >= 40)
        {
            GameManager.instance.silver1.SetActive(true);
        }
        //silver2
        if (bronze1Progress >= 10)
        {
            GameManager.instance.bronze1.SetActive(true);
        }
    }

    void UpdateGold()
    {
        goldProgress++;
        CheckCompletion();
    }
    void UpdateSilver1()
    {
        silver1Progress = (int)GameManager.instance.playerScript.speed;
        CheckCompletion();
    }
    void UpdateSilver2()
    {
        silver1Progress++;
        CheckCompletion();
    }
    void UpdateBronze1()
    {
        silver1Progress++;
        CheckCompletion();
    }
    void UpdateBronze2()
    {
        silver1Progress++;
        CheckCompletion();
    }
}
