using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshObstacle obstacle;

    [SerializeField] Collider doorway;
    [SerializeField] Collider doorSlab;

    [Tooltip("How far away the player can be while still opening the door")]
    [SerializeField] float usableDist = 5.0f;

    [Tooltip("Time in seconds to wait before allowing another open/close action")]
    [SerializeField] float cooldownTime = 1.0f;

    public NavMeshSurface navMeshSurface;

    bool isOpen = false;
    private bool isCooldown = false;

    void Update()
    {
        if (isCooldown) return;
        if (Camera.main == null)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, usableDist))
        {
            if (hit.collider.CompareTag("Door") && Input.GetKey("e") && (hit.collider == doorway || hit.collider == doorSlab))
            {
                if (isOpen)
                {
                    animator.SetTrigger("Close");
                    doorway.enabled = true;
                }
                if (!isOpen)
                {
                    animator.SetTrigger("Open");
                    doorway.enabled = false;
                }

                isOpen = !isOpen;
                StartCoroutine(Cooldown());
            }
        }
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);

        UpdateNavMesh();

        isCooldown = false;
    }

    void UpdateNavMesh()
    {
        //navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
    }
}
