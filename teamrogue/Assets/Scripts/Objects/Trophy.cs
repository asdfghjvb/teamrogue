using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Trophy : MonoBehaviour
{
    [SerializeField] GameObject trophyMenu;
    [SerializeField] TMP_Text trophyName;
    [SerializeField] TMP_Text trophyDescription;
    [SerializeField] Image menuBox;

    
    void Update()
    {
        TrophyData data = GameManager.instance.playerScript.objectView();
        if (Input.GetButtonDown("e") && data != null)
        {
            GameManager.instance.statePaused();
            GameManager.instance.menuActive = trophyMenu;
            GameManager.instance.menuActive.SetActive(GameManager.instance.isPaused);
            trophyName.text = data.trophyName;
            trophyDescription.text = data.trophyDescription;
            menuBox.color = data.color;
        }
    }
}
