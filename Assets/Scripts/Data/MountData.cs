using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MountData", menuName = "Data/Mount")]
public class MountData : ScriptableObject
{
    public string displayName;
    public Sprite icon;

    public int moveDistance;
    public int minWalkableHeight = 0;
    public int maxWalkableHeight = 0;

    public List<WeaponData> weaknesses;
    public List<WeaponData> resistances;

}