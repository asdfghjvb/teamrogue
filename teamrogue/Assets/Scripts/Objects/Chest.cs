using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IDamage
{
    public void takeDamage(int amount)
    {
        GameManager.instance.rewardMenu();
        Destroy(gameObject);
    }



}
