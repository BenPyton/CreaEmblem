using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stat
{
    public string name;
    public int value;
    public AnimationCurve curve;

    public Stat(string _name, int _value, AnimationCurve _curve)
    {
        name = _name;
        value = _value;
        curve = _curve;
    }
}

[CreateAssetMenu(fileName = "HeroData", menuName = "Data/Hero")]
public class HeroData : ScriptableObject
{
    public string displayName;
    public Sprite icon;

    public Sprite spriteSheet;
    public WeaponData weapon;
    public MountData mount;

    [SerializeField] public List<Stat> stats = new List<Stat>
    {
        new Stat("Level", 0, new AnimationCurve()),
        new Stat("Life", 0, new AnimationCurve()),
        new Stat("Attack", 0, new AnimationCurve()),
        new Stat("Defense", 0, new AnimationCurve()),
        new Stat("Resistance", 0, new AnimationCurve()),
        new Stat("Speed", 0, new AnimationCurve())
    };
}
