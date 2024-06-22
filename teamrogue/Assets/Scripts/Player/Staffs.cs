using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class Staffs : ScriptableObject
{
    public GameObject staffModel;
    [Range(1, 10)] public int staffDamage;
    [Range(1, 100)] public int staffDistance;
    [Range(0.1f, 3)] public float staffSpeed;
    public GameObject bullet;

    public ParticleSystem hitEffect;
}
