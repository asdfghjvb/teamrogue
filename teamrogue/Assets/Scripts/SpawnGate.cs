using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] List<GameObject> Enemies;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var enemy in Enemies)
        { //deactivate all the enemies connected to the gate
            enemy.SetActive(false); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return; //if not the player, do nothing

        foreach(var enemy in Enemies)
        {
            enemy.SetActive(true);
        }

        Destroy(gameObject);
    }
}
