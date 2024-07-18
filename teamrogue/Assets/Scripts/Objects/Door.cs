using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Collider doorway;
    [SerializeField] Collider doorSlab;

    [Tooltip("How far away the player can be while still opening the door")]
    [SerializeField] float usableDist;

    [Tooltip("Time in seconds to wait before allowing another open/close action")]
    [SerializeField] float cooldownTime = 1.0f;

    bool isOpen = false;
    private bool isCooldown = false;

    private void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        if(meshFilters.Length > 0)
        {
            CombineMeshFilters(meshFilters);
        }
    }

    void Update()
    {
        if (isCooldown) return;
        if (Camera.main == null)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, usableDist))
        {
            if (hit.collider.CompareTag("Door") && Input.GetKey("e"))
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

    public void CombineMeshFilters(MeshFilter[] meshFilters)
    {
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        MeshFilter parentMeshFilter = gameObject.GetComponent<MeshFilter>();

        if (parentMeshFilter == null)
            parentMeshFilter = gameObject.AddComponent<MeshFilter>();

        parentMeshFilter.mesh = combinedMesh;

        //disable the child mesh filters cause weird visual bugs happen otherwise
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].gameObject != gameObject)
            {
                meshFilters[i].gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
}
