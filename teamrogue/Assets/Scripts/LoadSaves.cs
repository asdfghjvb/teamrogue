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

    public void ClearSave()
    {
        saveFile.ClearSaveFile();
    }
   
}
