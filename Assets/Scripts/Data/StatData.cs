using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatData", menuName = "Data/Stat")]
public class StatData : ScriptableObject
{
    public string displayName;
    public Sprite icon;

    public int minValue;
    public int maxValue;
}