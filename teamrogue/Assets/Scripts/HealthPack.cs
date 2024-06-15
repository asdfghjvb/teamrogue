using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    
    //[SerializeField] Rigidbody rb;
   
    [SerializeField] int healing;
    [SerializeField] int destroyTime;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
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
