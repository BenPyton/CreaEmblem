using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZoneData", menuName = "Data/Zone")]
public class ZoneData : ScriptableObject
{
    public string displayName;
    public Sprite icon;

    public int height;
    public Sprite spriteSheet;
}

