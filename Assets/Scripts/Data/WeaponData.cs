using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Physical,
    Magical
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon")]
public class WeaponData : ScriptableObject
{
    public string displayName;
    public Sprite icon;

    public int range = 1;
    public DamageType damageType;
    public List<WeaponData> weaknesses;
    public List<WeaponData> resistances;


    public bool IsWeakAgainst(WeaponData _other)
    {
        return weaknesses.Contains(_other);
    }
    
    public bool IsStrongAgainst(WeaponData _other)
    {
        return resistances.Contains(_other);
    }
}
