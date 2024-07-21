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

    private Dictionary<Button, Vector2> originalButtonSizes = new Dictionary<Button, Vector2>();
    private Dictionary<Image, Vector2> originalImageSizes = new Dictionary<Image, Vector2>();

    void Start()
    {
        // Store original sizes
        foreach (Button button in buttons)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            originalButtonSizes[button] = rectTransform.sizeDelta;
        }

        foreach (Image image in images)
        {
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            originalImageSizes[image] = rectTransform.sizeDelta;
        }
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
            rectTransform.sizeDelta = originalButtonSizes[button] * scaleFactor;
        }
        foreach (Image image in images)
        {
            RectTransform rectTransform2 = image.GetComponent<RectTransform>();
            rectTransform2.sizeDelta = originalImageSizes[image] * scaleFactor;
        }
    }
}
