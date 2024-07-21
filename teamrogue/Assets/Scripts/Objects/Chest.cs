using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] public ChestOb chestOb;
    private void Update()
    {
        ScriptableObject data = GameManager.instance.playerScript.objectView();
        if (Input.GetKey("e") && data != null && data is ChestOb && !GameManager.instance.isPaused)
        {
            GameManager.instance.rewardMenu();
        }
    }


}
