using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TileDataStruct
{
    public TileBase tile;
    public ZoneData data;
}

[DefaultExecutionOrder(-100)]
public class MapManager : MonoBehaviour
{
    static MapManager instance = null;

    [SerializeField] Tilemap path;
    [SerializeField] TileBase pathTile;

    [SerializeField] List<TileDataStruct> tileData = new List<TileDataStruct>();

    GridInformation gridInfo = null;
    Grid grid = null;

    // List containing all heroes currently on the map
    List<Hero> heroes = new List<Hero>();

    // List containing all player starts on the map
    List<PlayerStart> starts = new List<PlayerStart>();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DataManager.instance.onEndTurn.AddListener((int _team) =>
            {
                foreach(Hero hero in heroes)
                {
                    hero.canPlay = true;
                }
            });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Tilemap tileMap = GetComponentInChildren<Tilemap>();
        gridInfo = GetComponent<GridInformation>();
        grid = GetComponent<Grid>();

        if (tileMap != null)
        {
            for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
            {
                for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, 0));
                    if (tileMap.HasTile(localPlace))
                    {
                        ZoneData data = tileData.Find(x => x.tile == tileMap.GetTile(localPlace)).data;

                        gridInfo.SetPositionProperty(localPlace, "data", data as Object);
                        
                    }
                    else
                    {
                        //No tile at "place"
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }






    /*  ===========================================================
     *              STATIC METHODS ACCESSIBLE ANYWHERE
     *  ===========================================================
     */

    public static ZoneData GetZoneAtPosition(Vector3Int _position)
    {
        return instance.gridInfo.GetPositionProperty<ZoneData>(_position, "data", null);
    }

    public static ZoneData GetZoneUnderMouse()
    {
        return GetZoneAtPosition(GetTileUnderMouse());
    }


    public static Vector3Int GetTileAtPosition(Vector3 _position)
    {
        return instance.grid.WorldToCell(_position);
    }

    public static Vector3Int GetTileUnderMouse()
    {
        return GetTileAtPosition(Utils.GetMouseWorldPosition());
    }

    public static Vector3 GetTileCenter(Vector3Int _tile)
    {
        return instance.grid.GetCellCenterWorld(_tile);
    }


    public static void RegisterHero(Hero _hero)
    {
        if(!instance.heroes.Contains(_hero))
        {
            instance.heroes.Add(_hero);
        }
    }

    public static void UnregisterHero(Hero _hero)
    {
        if (instance.heroes.Contains(_hero))
        {
            instance.heroes.Remove(_hero);
        }
    }

    public static void RegisterPlayerStart(PlayerStart _start)
    {
        if (!instance.starts.Contains(_start))
        {
            instance.starts.Add(_start);
        }
    }

    public static void UnregisterPlayerStart(PlayerStart _start)
    {
        if (instance.starts.Contains(_start))
        {
            instance.starts.Remove(_start);
        }
    }

    public static List<PlayerStart> GetAllPlayerStarts(int _team = -1)
    {
        if (_team >= 0)
        {
            return instance.starts.Where(h => h.team == _team).ToList();
        }
        else
        {
            return instance.starts;
        }
    }

    public static Hero GetHeroAtTile(Vector3Int _tile)
    {
        return instance.heroes.Find(x => x.gridPosition == _tile);
    }


    public static void ResetPath()
    {
        instance.path.ClearAllTiles();
    }

    public static void SetPathTo(Vector3Int _tile)
    {
        instance.path.SetTile(_tile, instance.pathTile);
    }

    public static List<Hero> GetAllHeroes(int team = -1)
    {
        if (team >= 0)
        {
            return instance.heroes.Where(h => h.team == team).ToList();
        }
        else
        {
            return instance.heroes;
        }
    }
}
