using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int manaAmount = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.mana += manaAmount;

            if (GameManager.instance.playerScript.mana > GameManager.instance.playerScript.fullMana)
            {
                GameManager.instance.playerScript.mana = GameManager.instance.playerScript.fullMana;
            }
            Destroy(gameObject);
            GameManager.instance.playerScript.updatePlayerUI();
        }
    }
}
