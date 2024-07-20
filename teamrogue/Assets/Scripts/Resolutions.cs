using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Resolutions : MonoBehaviour
{

    public TMP_Dropdown resolutionDrop;
    public Resolution currentResolution;
    
    private Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        
        //get all full screen resolutions supported
        resolutions = Screen.resolutions;
        resolutionDrop.ClearOptions();

        //keep track of current index
        int currentResIndex = 0;
        //list to keep track of options
        List<string> options = new List<string>();

        //loop through the array to get each option into the list
        for (int index = 0; index < resolutions.Length; index++)
        {
            string option = resolutions[index].width + "x" + resolutions[index].height; 
            options.Add(option);

            //save the current screen resolution as the current index
            if (resolutions[index].width == Screen.currentResolution.width && resolutions[index].height == Screen.currentResolution.height)
            {
                currentResIndex = index;
            }
        }
        //add the options to the dropdown
        resolutionDrop.AddOptions(options);
        //set the current value to the current res
        resolutionDrop.value = currentResIndex;
        //refresh the visible value to be accurate
        resolutionDrop.RefreshShownValue();
        //listener to watch for changes
        resolutionDrop.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        //set the screen resolution from the current index
        Resolution resolution = resolutions[resolutionIndex];
        currentResolution = resolution;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    
  
}
