using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


[DefaultExecutionOrder(-100)]
public class MapManager : MonoBehaviour
{
    static MapManager instance = null;

    [SerializeField] Hero prefabHero;
    [SerializeField] MapData map;
    [Header("Path")]
    [SerializeField] Tilemap path;
    [SerializeField] TileBase pathTile;
    
    //[SerializeField] HeroStart prefabHeroStart;

    GridInformation gridInfo = null;
    Grid grid = null;

    // List containing all heroes currently on the map
    List<Hero> heroes = new List<Hero>();

    // List containing all player starts on the map
    List<HeroStartInfo> starts = new List<HeroStartInfo>();

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
        gridInfo = GetComponent<GridInformation>();
        grid = GetComponent<Grid>();

        LoadMap();
        SpawnHeroes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void LoadMap()
    {
        Tilemap tileMap = GetComponentInChildren<Tilemap>();
        if (tileMap != null && map != null && map.palette != null && map.Load())
        {
            Debug.Log("Map loaded !");
            tileMap.ClearAllTiles();
            for (int x = 0; x < MapData.width; x++)
            {
                for (int y = 0; y < MapData.height; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    int value = map.data[x, y];
                    if (value >= 0)
                    {
                        tileMap.SetTile(position, map.palette[value].tile);
                        gridInfo.SetPositionProperty(position, "data", map.palette[value].data as Object);
                    }
                    else
                    {
                        //No tile at this position
                    }
                }
            }

            starts.Clear();
            foreach (HeroStartInfo info in map.heroStarts)
            {
                starts.Add(info);
            }
        }
    }



    void SpawnHeroes()
    {
        List<HeroStartInfo> starts = GetAllHeroStarts();

        foreach (HeroTeam h in DataManager.instance.heroToSpawn)
        {
            HeroStartInfo start;
            if (GetFirstHeroStart(out start, h.team, starts))
            {
                starts.Remove(start);
                Hero hero = Instantiate(prefabHero);
                hero.data = h.data;
                hero.team = h.team;
                hero.transform.position = new Vector3(start.position.x, start.position.y);
            }
            else
            {
                Debug.LogError("Error: can't find a player start for a hero in team " + h.team);
            }
        }
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

    public static List<HeroStartInfo> GetAllHeroStarts(int _team = -1)
    {
        if (_team >= 0)
        {
            return instance.starts.Where(h => h.team == _team).ToList();
        }
        else
        {
            return instance.starts.ToList(); // ToList permit to return a copy instead of the singleton list reference
        }
    }

    public static bool GetFirstHeroStart(out HeroStartInfo _start, int _team = -1, List<HeroStartInfo> startList = null)
    {
        bool success = false;
        _start = new HeroStartInfo();

        List<HeroStartInfo> starts = startList != null ? startList : instance.starts;

        if (_team < 0 && starts.Count > 0)
        {
            _start = instance.starts[0];
            success = true;
        }
        else if (_team >= 0)
        {
            int index = starts.FindIndex(x => x.team == _team);
            if(index >= 0)
            {
                _start = starts[index];
                success = true;
            }
        }

        return success;
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
