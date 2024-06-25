using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.door1Condition){
            door1.transform.position = new Vector3(.5f, 0, 0);
            door2.transform.position = new Vector3(.5f, 0, 0);
        }
        GameManager.instance.navMeshBakerScript.rebakeNavMesh();

    }
}
