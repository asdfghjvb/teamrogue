using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void takeDamage(int amount);
    void knockback(Vector3 direction, float force);
}

