using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    public static Vector3 SnapToGrid(Vector3 position, float gridSize = 1)
    {
        return new Vector3(
            (0.5f + Mathf.Floor(position.x / gridSize)) * gridSize,
            (0.5f + Mathf.Floor(position.y / gridSize)) * gridSize,
            0);
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    public static int Dist(Vector3Int _a, Vector3Int _b)
    {
        return Length(_b - _a);
    }

    public static int Length(Vector3Int _v)
    {
        return Mathf.Abs(_v.x) + Mathf.Abs(_v.y) + Mathf.Abs(_v.z);
    }

    public static Vector3Int[] DirectionTile = new Vector3Int[4]
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0)
    };


    public static TileClass GetNearestTile(Vector3Int _fromTile, List<TileClass> _tiles)
    {
        TileClass nearestTile = null;

        if (_tiles != null && _tiles.Count > 0)
        {
            nearestTile = _tiles[0];
            foreach (TileClass tile in _tiles)
            {
                if (tile.type == TileType.Walkable && 
                    Dist(tile.position, _fromTile) <= Dist(nearestTile.position, _fromTile))
                {
                    nearestTile = tile;
                }
            }
        }

        return nearestTile;
    }

    public static TileClass GetFirstWalkableTile(TileClass _fromTile)
    {
        TileClass nearestTile = null;
        TileClass current = _fromTile;
        while(current != null && nearestTile == null)
        {
            if(current.type == TileType.Walkable)
            {
                nearestTile = current;
            }
            current = current.previous;
        }

        return nearestTile;
    }
}


public enum TileType
{
    None,
    Walkable,
    Attackable,
    Buffable,
    NbType
}

public class TileClass
{
    public Vector3Int position = Vector3Int.zero;
    public bool enabled = true;
    public TileType type = TileType.None;

    public TileClass previous = null;

    public TileClass(Vector3Int _pos, TileType _type = TileType.None, bool _enabled = true)
    {
        position = _pos;
        type = _type;
        enabled = _enabled;
    }

    public override bool Equals(object obj)
    {
        if(obj.GetType() == typeof(TileClass))
        {
            return (obj as TileClass).position == position;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return 1206833562 + EqualityComparer<Vector3Int>.Default.GetHashCode(position);
    }
}