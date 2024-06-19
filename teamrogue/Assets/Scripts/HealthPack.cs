using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] int healing;
    [SerializeField] int destroyTime;

    [SerializeField] float hoverHeight;

    [Tooltip("The speed in degrees per frame at which the health pack rotates")]
    [SerializeField] float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Vector3 newPosition = hit.point + Vector3.up * hoverHeight;
            transform.position = newPosition;
        }

        Vector3 spawnRotation = new Vector3(270, 0, 0);
        transform.rotation = Quaternion.Euler(spawnRotation);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage heal = other.GetComponent<IDamage>();


        if (heal != null)
        {
            heal.takeDamage(-healing);
        }

        Destroy(gameObject);
    }
}
