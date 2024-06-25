using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] float openDuration;

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
        if (other.CompareTag("Player")){
            door1.transform.position = new Vector3(Mathf.Lerp(door1.transform.position.x, door1.transform.position.x - 3, openDuration), door1.transform.position.y, door1.transform.position.z);
            door2.transform.position = new Vector3(Mathf.Lerp(door2.transform.position.x, door2.transform.position.x + 6, openDuration), door2.transform.position.y, door2.transform.position.z);
            GameManager.instance.navMeshBakerScript.rebakeNavMesh();
            gameObject.SetActive(false);
        }

    }
}
