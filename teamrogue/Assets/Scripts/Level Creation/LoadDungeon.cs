using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDungeon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("Dungeon");
        LevelGenerator.seed = RandomNumberGenerator.GetInt32(100000000);
    }
}
