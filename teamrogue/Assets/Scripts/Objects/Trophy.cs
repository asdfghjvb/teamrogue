using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : MonoBehaviour
{
    [SerializeField] GameObject trophyMenu;

    
    void Update()
    {
        if(GameManager.instance.playerScript.objectView() && Input.GetButtonDown("e"))
        {
            GameManager.instance.menuActive = trophyMenu;
            GameManager.instance.menuActive.SetActive(true);
        }
    }
}
