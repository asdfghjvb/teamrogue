using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClick;

    public void resume()
    {
        GameManager.instance.stateUnpaused();
    }

    public void restart()
    {
        StartCoroutine(restartDelay());
        GameManager.instance.stateUnpaused();

    }

    public void quit()
    {
       
        foreach (Staffs staff in GameManager.instance.playerScript.staffList)
        {
            staff.ResetStaffStats();
        }
        playClickSound();
        SceneManager.LoadScene("Main Menu");
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
    public void boonReward()
    {
        playClickSound();
        GameManager.instance.stateUnpaused();
        GameManager.instance.boonManager.randomizeList();
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

    public void playClickSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }
   
    IEnumerator restartDelay()
    {
        playClickSound();
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    
}
