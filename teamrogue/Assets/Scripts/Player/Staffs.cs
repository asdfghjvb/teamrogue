using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Staffs : ScriptableObject
{
    public GameObject staffModel;
    [Range(1, 10)] public int staffDamage;
    [Range(1, 100)] public int staffDistance;
    [Range(0.1f, 10)] public float staffSpeed;
    public GameObject bullet;
    public ParticleSystem hitEffect;

    //[Range(1, 50)] public int maxAmmoInClip; // Máxima munición en el clip
    //[HideInInspector] public int currentAmmoInClip; // Munición actual en el clip

    [Range (1, 10)] public int manaCost;
    private int staffBaseDamage;
    private int staffBaseDistance;
    private float staffBaseSpeed;
    private int staffBaseMaxAmmoInClip;

    public void InitializeStaffValues()
    {
        staffBaseDamage = staffDamage;
        staffBaseDistance = staffDistance;
        staffBaseSpeed = staffSpeed;
        //staffBaseMaxAmmoInClip = maxAmmoInClip;
        //currentAmmoInClip = maxAmmoInClip; // Inicializar munición actual
    }

    public void ResetStaffStats()
    {
        staffDamage = staffBaseDamage;
        staffDistance = staffBaseDistance;
        staffSpeed = staffBaseSpeed;
        //maxAmmoInClip = staffBaseMaxAmmoInClip;
        //currentAmmoInClip = maxAmmoInClip;
    }
}
