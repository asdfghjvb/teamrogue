using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.stateUnpaused();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpaused();
    }

    public void quit()
    {
        foreach(Staffs staff in GameManager.instance.playerScript.staffList)
        {
            staff.ResetStaffStats();
        }

        SceneManager.LoadScene("Main Menu");
    }
    public void healReward()
    {
        if (GameManager.instance.playerScript.health <= GameManager.instance.playerScript.fullHealth / 2)
            GameManager.instance.playerScript.health += GameManager.instance.playerScript.fullHealth / 2;
        else
            GameManager.instance.playerScript.health = GameManager.instance.playerScript.fullHealth;
        GameManager.instance.playerScript.updatePlayerUI();
        GameManager.instance.healButton.SetActive(false);
        GameManager.instance.boonButton.SetActive(true);
    }
    public void boonReward()
    {
        GameManager.instance.stateUnpaused();
        BoonManager.instance.randomizeList();
        GameManager.instance.boonSelection();
        if (GameManager.instance.room1Clear)
        {
            GameManager.instance.door2Col.enabled = true;
            GameManager.instance.rewardChest1.SetActive(false);
        }
        else if (GameManager.instance.room2Clear)
        {
            GameManager.instance.door3Col.enabled = true;
            GameManager.instance.rewardChest2.SetActive(false);
        }
    }



}
