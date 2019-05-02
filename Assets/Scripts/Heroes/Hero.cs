using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    public int team = 0;

    public bool canPlay = true;



    public int MoveRange
    {
        get { return data.mount.moveDistance; }
    }

    public int AttackRange
    {
        get { return data.weapon.range; }
    }
    
    //Dictionary<Stat, int> stats = new Dictionary<Stat, int>();

    private int m_life = 0;
    public int life
    {
        get { return m_life; }
        set { m_life = Mathf.Clamp(value, 0, maxLife); }
    }

    public bool isAlive { get { return life > 0; } }

    public int experience { get { return GetStatValueByName("Exp"); } }
    public int maxLife { get { return GetStatValueByName("Hp"); } }
    public int attack { get { return GetStatValueByName("Atk"); } }
    public int defense { get { return GetStatValueByName("Def"); } }
    public int resistance { get { return GetStatValueByName("Res"); } }
    public int speed { get { return GetStatValueByName("Spd"); } }

    public int level = 1;
    public int currentExp = 0;
    public int previousExp = 0;
    public bool expChanged { get { return currentExp != previousExp; } }
    public int nextLevelExp {  get { return GetStatValueByName("Exp"); } }

    // Start is called before the first frame update
    void Start()
    {
        m_currentPosition.value = MapManager.GetTileCenter(MapManager.GetTileAtPosition(transform.position));
        m_gridPosition = MapManager.GetTileAtPosition(m_currentPosition.target);

        AnimatedSprite sprite = GetComponent<AnimatedSprite>();
        if (data != null)
        {
            life = maxLife;
            sprite.sheet = data.spriteSheet;
        }
        else
        {
            Debug.LogWarning("No data on hero");
        }

        sprite.flipX = team == 0;

        MapManager.RegisterHero(this);
        
    }
    
    private void OnEnable()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null && data != null && data.spriteSheet != null)
        {
            sr.sprite = data.spriteSheet[0, 0];
        }
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

    public LevelInfo CheckCurrentLevel()
    {
        int expToNextLevel = 0;

        LevelInfo info = new LevelInfo();

        info.hero = this;
        info.prevExp = previousExp;
        info.prevLevel = level;

        do
        {
            expToNextLevel = GetStatValueByName("Exp");
            if (currentExp >= expToNextLevel)
            {
                currentExp -= expToNextLevel;
                level++;
            }
        } while (currentExp >= GetStatValueByName("Exp"));

        info.newExp = currentExp;
        info.newLevel = level;

        previousExp = currentExp;

        return info;
    }

    public bool Walkable(ZoneData zone)
    {
        return zone == null ||
            (zone.height >= data.mount.minWalkableHeight 
            && zone.height <= data.mount.maxWalkableHeight);
    }

    public List<TileClass> GetReachableTiles()
    {
        List<TileClass> reachableTiles = new List<TileClass>();
        Queue<TileClass> openTiles = new Queue<TileClass>();
        
        TileClass begin = new TileClass(m_gridPosition, TileType.Walkable, false);

        openTiles.Enqueue(begin);
        reachableTiles.Add(begin);

        bool isRightTeam = team == DataManager.instance.teamPlaying;

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
                        reachableTiles.Add(newTile);

                        Hero occuper = MapManager.GetHeroAtTile(tilePos);
                        bool hasEnemy = occuper != null && occuper.team != team;

                        if (k >= MoveRange || hasEnemy)
                        {
                            newTile.type = TileType.Attackable;
                            newTile.enabled = isRightTeam && hasEnemy;
                        }
                        else
                        {
                            newTile.type = TileType.Walkable;
                            newTile.enabled = isRightTeam && occuper == null;
                        }

                        if(!hasEnemy)
                        {
                            openTiles.Enqueue(newTile);
                        }
                    }
                }
            }
        }

        return reachableTiles;
    }



    private int GetStatValueByName(string _name)
    {
        if (data != null)
        {
            List<Stat> stats = data.stats.Where(x => x.stat.displayName == _name).ToList();
            if (stats.Count > 0)
            {
                return Mathf.RoundToInt(stats[0].curve.Evaluate(level / 100.0f) * (stats[0].stat.maxValue - stats[0].stat.minValue) + stats[0].stat.minValue);
            }
            else
            {
                Debug.LogError("No stat found with name: " + _name);
            }
        }
        return -1;
    }


    public void GetDamage(Hero _from)
    {
        if(isAlive)
        {
            int def = data.weapon.damageType == DamageType.Physical ? defense : resistance;
            int damage = _from.attack - def;
            life -= damage;
            Debug.Log(name + " take " + damage + " damages : " + life + "/" + maxLife);

            _from.currentExp += 50;
            if (!isAlive)
            {
                DataManager.instance.onHeroDeath.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}
