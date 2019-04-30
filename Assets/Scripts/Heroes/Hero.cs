using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] public HeroData data;

    private SmoothVector3 m_currentPosition = new SmoothVector3(Vector3.zero, 0.1f);
    public Vector3 position
    {
        get { return targetPosition; }
        set {
            targetPosition = value;
            m_gridPosition = MapManager.GetTileAtPosition(m_currentPosition.target);
        }
    }

    public Vector3 targetPosition
    {
        get { return m_currentPosition.target; }
        set { m_currentPosition.target = MapManager.GetTileCenter(MapManager.GetTileAtPosition(value)); }
    }

    private Vector3Int m_gridPosition;
    public Vector3Int gridPosition
    {
        get { return m_gridPosition; }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        position = MapManager.GetTileCenter(MapManager.GetTileAtPosition(transform.position));
        MapManager.RegisterHero(this);
    }

    private void OnDestroy()
    {
        MapManager.UnregisterHero(this);
    }

    // Update is called once per frame
    void Update()
    {
        m_currentPosition.Update();
        transform.position = m_currentPosition.value;
    }

    public int MoveRange
    {
        get { return data.mount.moveDistance; }
    }

    public int AttackRange
    {
        get { return data.weapon.range; }
    }

    public bool Walkable(ZoneData zone)
    {
        return zone.height >= data.mount.minWalkableHeight 
            && zone.height <= data.mount.maxWalkableHeight;
    }






    public List<TileClass> GetReachableTiles()
    {
        List<TileClass> reachableTiles = new List<TileClass>();
        Queue<TileClass> openTiles = new Queue<TileClass>();
        
        TileClass begin = new TileClass(m_gridPosition, TileType.Walkable, false);

        openTiles.Enqueue(begin);
        reachableTiles.Add(begin);

        for (int k = 0; k < MoveRange + AttackRange; k++)
        {
            int nbTile = openTiles.Count;
            for (int j = 0; j < nbTile; j++)
            {
                TileClass tile = openTiles.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    Vector3Int tilePos = tile.position + Utils.DirectionTile[i];
                    TileClass newTile = new TileClass(tilePos);
                    newTile.previous = tile;
                    if (!reachableTiles.Contains(newTile) && Walkable(MapManager.GetZoneAtPosition(tilePos)))
                    {
                        openTiles.Enqueue(newTile);
                        reachableTiles.Add(newTile);

                        if (k < MoveRange)
                        {
                            newTile.type = TileType.Walkable;
                            newTile.enabled = !MapManager.GetHeroAtTile(tilePos);
                        }
                        else
                        {
                            newTile.type = TileType.Attackable;
                            newTile.enabled = MapManager.GetHeroAtTile(tilePos);
                        }
                    }
                }
            }
        }

        return reachableTiles;
    }
}
