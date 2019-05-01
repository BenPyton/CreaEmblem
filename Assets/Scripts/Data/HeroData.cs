using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stat
{
    public StatData stat;
    [SerializeField] public AnimationCurve curve;
}

[CreateAssetMenu(fileName = "HeroData", menuName = "Data/Hero")]
public class HeroData : ScriptableObject
{
    public string displayName;
    public Sprite icon;

    public SpriteSheet spriteSheet;
    public WeaponData weapon;
    public MountData mount;

    [SerializeField] public List<Stat> stats = new List<Stat>();
}
