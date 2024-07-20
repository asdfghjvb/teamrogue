using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject teleportEffect;
    [SerializeField] Collider portalCollider;

    [Tooltip("How far away the player can be while still opening the door")]
    [SerializeField] float usableDist = 5.0f;

    [Tooltip("How long the delay between activating the portal and teleporting away is")]
    [SerializeField] float teleportDelay = 3.0f;

    bool teleporting = false;

    // Update is called once per frame
    void Update()
    {
        if (Camera.main == null)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, usableDist))
        {
            GameManager.instance.playerScript.objectView(usableDist);

            if (Input.GetKey("e") && hit.collider == portalCollider && !teleporting)
            {
                StartCoroutine(Teleport());
            }
        }
    }

    IEnumerator Teleport()
    {
        teleporting = true;

        GameObject effect = Instantiate(teleportEffect, GameManager.instance.player.transform.position, Quaternion.identity);
        effect.transform.parent = GameManager.instance.player.transform;

        yield return new WaitForSeconds(teleportDelay);

        SceneManager.LoadScene("Hub");

        teleporting = false;
    }
}
