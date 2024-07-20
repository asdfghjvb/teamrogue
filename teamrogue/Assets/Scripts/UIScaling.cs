using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaling : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public Button[] buttons;
    public Image[] images;
    public Vector2 referenceResolution = new Vector2(1920, 1080);

    void Start()
    {
        //currentResolution = GetComponent<Resolution>();
        ScaleButtons();
    }

    public void ScaleButtons()
    {
        float widthRatio = Screen.width / referenceResolution.x;
        float heightRatio = Screen.height / referenceResolution.y;
        float scaleFactor = Mathf.Min(widthRatio, heightRatio);

        foreach (Button button in buttons)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.sizeDelta *= scaleFactor;
        }
        foreach (Image image in images)
        {
            RectTransform rectTransform2 = image.GetComponent<RectTransform>();
            rectTransform2.sizeDelta *= scaleFactor;
        }
    }
}
