using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSaves : MonoBehaviour
{
    [SerializeField] Saves saveFile;

    public void SaveGame()
    {
        saveFile.save();
    }

    public void LoadSave()
    {
        Debug.Log("Before Load");
        saveFile.load();
        Debug.Log("save loaded");
        SceneManager.LoadScene("Hub");
    }

    public void Start()
    {
        Debug.Log("start called");
        if (GameManager.instance.IsInScene("Hub"))
        {
            GameManager.instance.goldEarned = saveFile.goldAch;
            GameManager.instance.silver1Earned = saveFile.silverAch1;
            GameManager.instance.silver2Earned = saveFile.silverAch2;
            GameManager.instance.bronze1Earned = saveFile.bronzeAch1;
            GameManager.instance.bronze2Earned = saveFile.bronzeAch2;
            GameManager.instance.bronze3Earned = saveFile.bronzeAch3;
            GameManager.instance.bronze4Earned = saveFile.bronzeAch4;
        }

    }

    public void ClearSave()
    {
        saveFile.ClearSaveFile();
    }
   
}
