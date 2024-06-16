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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void healReward()
    {
        if (GameManager.instance.playerScript.health <= GameManager.instance.playerScript.fullHealth / 2)
            GameManager.instance.playerScript.health += GameManager.instance.playerScript.fullHealth / 2;
        else
            GameManager.instance.playerScript.health = GameManager.instance.playerScript.fullHealth;
        GameManager.instance.healButton.SetActive(false);
    }
    public void boonReward()
    {
        BoonManager.instance.randomizeList();
        GameManager.instance.boonSelection();
        GameManager.instance.boonButton.SetActive(false);
    }
    public void closeChest()
    {
        resume();
        GameManager.instance.door1.transform.position = new Vector3(2, 0, 0);
        GameManager.instance.door2.transform.position = new Vector3(-2, 0, 0);
    }
    

}