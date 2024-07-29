using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
   
{
    [SerializeField] public ChestOb chestOb;

    public Chest chest;

    private void Start()
    {
        chest = this;
    }
    private void Update()
    {
        ScriptableObject data = GameManager.instance.playerScript.objectView();
        if (Input.GetKey("e") && data != null && data is ChestOb && !GameManager.instance.isPaused)
        {
            GameManager.instance.rewardMenu();
        }
    }
    public void boonreward()
    {
        ButtonFunctions.instance.playClickSound();
        GameManager.instance.healButton.SetActive(true);
        GameManager.instance.boonButton.SetActive(false);
        GameManager.instance.stateUnpaused();
        GameManager.instance.boonManager.randomizeList();
        GameManager.instance.boonSelection();
        Destroy(gameObject);
    }
}



