using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] public UpgradeScriptOb menu;
    [SerializeField] UnityEngine.UI.Image menuBox;

    UpgradeScriptOb anvilData;

    // Update is called once per frame
    void Update()
    {
        ScriptableObject data = GameManager.instance.playerScript.objectView();
        if (Input.GetKey("e") && data != null && data is UpgradeScriptOb && !GameManager.instance.isPaused)
        {
            GameManager.instance.statePaused();
            GameManager.instance.menuActive = GameManager.instance.upgradeUI;
            GameManager.instance.menuActive.SetActive(GameManager.instance.isPaused);
            anvilData = (UpgradeScriptOb)data;
            title.text = anvilData.title;
            menuBox.color = anvilData.color;
        }
    }
}
