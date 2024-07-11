using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TrophyData : ScriptableObject
{
    [SerializeField] public string trophyName;
    [SerializeField] public string trophyDescription;
    [SerializeField] public Color color;
}
