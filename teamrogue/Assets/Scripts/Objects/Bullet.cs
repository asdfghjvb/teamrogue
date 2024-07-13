using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    [SerializeField] bool destroyOnInpact = true;

    // Start is called before the first frame update
    void Start()
    {

        Vector3 playerPos = GameManager.instance.player.transform.position;
        rb.velocity = (new Vector3(playerPos.x, playerPos.y + 0.5f, playerPos.z) - transform.position).normalized * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damage);

            if (!destroyOnInpact) //check is here so it will still be destroyed when hitting structures
                return;
        }

        Destroy(gameObject);
    }
}