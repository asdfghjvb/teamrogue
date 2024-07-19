using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Trophy : MonoBehaviour
{
    [SerializeField] TMP_Text trophyName;
    [SerializeField] TMP_Text trophyDescription;
    [SerializeField] UnityEngine.UI.Image menuBox;

    TrophyData trophyInfo;
    
    void Update()
    {
        /*
        ScriptableObject data = GameManager.instance.playerScript.objectView();
        if (Input.GetKey("e") && data != null && data is TrophyData && !GameManager.instance.isPaused)
        {
            GameManager.instance.statePaused();
            GameManager.instance.menuActive = GameManager.instance.trophyMenu;
            GameManager.instance.menuActive.SetActive(GameManager.instance.isPaused);
            trophyInfo = (TrophyData)data;
            trophyName.text = trophyInfo.trophyName;
            trophyDescription.text = trophyInfo.trophyDescription;
            menuBox.color = trophyInfo.color;
        }
        */
    }
}
