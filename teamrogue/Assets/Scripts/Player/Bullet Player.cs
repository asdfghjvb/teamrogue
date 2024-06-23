using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] float destroyTime;

    [SerializeField] bool destroyOnInpact = true;

    // Start is called before the first frame update
    void Start()
    {

        //Vector3 playerPos = GameManager.instance.player.transform.position;
        rb.velocity = (transform.forward).normalized * speed;
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