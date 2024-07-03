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
    [Range(1, 10)] public int staffManaCost;
    public GameObject bullet;
    public ParticleSystem hitEffect;

    public AudioClip staffShootEffect;
    public float staffShootVol;

   

    private int staffBaseDamage;
    private int staffBaseDistance;
    private float staffBaseSpeed;
   

    public void InitializeStaffValues()
    {
        staffBaseDamage = staffDamage;
        staffBaseDistance = staffDistance;
        staffBaseSpeed = staffSpeed;
      
    }

    public void ResetStaffStats()
    {
        staffDamage = staffBaseDamage;
        staffDistance = staffBaseDistance;
        staffSpeed = staffBaseSpeed;
        
    }
}
