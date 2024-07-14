using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPost : MonoBehaviour
{
    [SerializeField] public SignScript signScript;

    // Update is called once per frame
    void Update()
    {
        ScriptableObject data = GameManager.instance.playerScript.objectView();
        if (Input.GetKey("e") && data != null && data is SignScript && !GameManager.instance.isPaused)
        {
            GameManager.instance.statePaused();
            GameManager.instance.menuActive = GameManager.instance.signUI;
            GameManager.instance.menuActive.SetActive(GameManager.instance.isPaused);
        }
    }
}
