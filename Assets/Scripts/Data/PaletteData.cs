using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TileInfo
{
    [SerializeField] public TileBase tile;
    [SerializeField] public ZoneData data;
}

[CreateAssetMenu(fileName="PaletteData", menuName="Data/Palette")]
public class PaletteData : ScriptableObject
{
    public int id = 0;
    public string displayName;

    [SerializeField] private List<TileInfo> tileInfos = new List<TileInfo>();


    public TileInfo this[int _index]
    {
        get
        {
            TileInfo info;
            GetTileInfo(_index, out info);
            return info;
        }
    }

    public ZoneData GetZoneFromTile(TileBase _tile)
    {
        return tileInfos.Find(x => x.tile == _tile).data;
    }

    public bool GetTileInfo(int _paletteIndex, out TileInfo _infos)
    {
        _infos = new TileInfo();
        if(_paletteIndex >= 0 && _paletteIndex < tileInfos.Count)
        {
            _infos = tileInfos[_paletteIndex];
            return true;
        }
        return false;
    }
}
