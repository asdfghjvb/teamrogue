using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGate : MonoBehaviour
{
    [Tooltip("All enemies you want connected to the spawn gate should be dragged here")]
    [SerializeField] List<GameObject> Enemies;

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
