using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject menuButtons;
    [SerializeField] GameObject credits;
    public float scrollSpeed = 50f;
    private RectTransform trans;
    private float startingPos;


    // Start is called before the first frame update
    void Start()
    {
       trans = GetComponent<RectTransform>();
        startingPos = trans.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        trans.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        

        if (trans.anchoredPosition.y  > Screen.height)
        {
            menuButtons.SetActive(true);
            trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, startingPos);
            credits.SetActive(false);
        }
    }
}
