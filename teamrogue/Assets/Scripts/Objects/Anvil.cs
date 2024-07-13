using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    [SerializeField] GameObject upgradeUI;
    [SerializeField] TMP_Text title;
    [SerializeField] UpgradeScriptOb menu;
    [SerializeField] UnityEngine.UI.Image menuBox;

    // Update is called once per frame
    void Update()
    {
        UpgradeScriptOb data = (UpgradeScriptOb)GameManager.instance.playerScript.objectView();
        if (Input.GetButtonDown("Interact") && data != null)
        {
            GameManager.instance.statePaused();
            GameManager.instance.menuActive = upgradeUI;
            GameManager.instance.menuActive.SetActive(GameManager.instance.isPaused);
            title.text = data.title;
            menuBox.color = data.color;
        }
    }
}
