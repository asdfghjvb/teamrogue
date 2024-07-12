using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public float sens;
    [SerializeField] int lockVertMin, lockVertMax;
    public bool invertY;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        invertY = GameManager.instance.invertY;
    }

    // Update is called once per frame
    void Update()
    {
        sens = GameManager.instance.sensitivity * 1000;
        invertY = GameManager.instance.invertY;
        //get input
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;

        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        //clamp rot x on x-axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //rotate cam on x axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        // rotate player on y axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}

