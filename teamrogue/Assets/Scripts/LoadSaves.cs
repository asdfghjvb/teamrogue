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
        saveFile.load();
        Debug.Log("save loaded");
        SceneManager.LoadScene("SampleScene");
    }

    public void ClearSave()
    {
        saveFile.ClearSaveFile();
    }
   
}
