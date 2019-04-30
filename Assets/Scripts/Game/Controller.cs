using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(100)]
public class Controller : MonoBehaviour
{
    [SerializeField] SpriteRenderer prefabSelectable;
    [SerializeField] SpriteRenderer prefabHighlight;
    [SerializeField] SpriteRenderer tileSelection;
    public Hero selectedHero = null;

    List<SpriteRenderer> highlights = new List<SpriteRenderer>();
    List<SpriteRenderer> selectables = new List<SpriteRenderer>();

    List<TileClass> reachableTiles = new List<TileClass>();

    private List<Hero> playingHeroes = new List<Hero>();
    bool posChanged = false;
    Vector3Int selectedTile = Vector3Int.zero;
    public bool blockInput = false;


    // Start is called before the first frame update
    void Awake()
    {
        DataManager.instance.onStartTurn.AddListener(StartTurn);
    }

    // Update is called once per frame
    void Update()
    {
        if(!blockInput && Input.GetMouseButtonDown(0))
        {
            Vector3Int tilePos = MapManager.GetTileUnderMouse();
            Hero heroOnTile = MapManager.GetHeroAtTile(tilePos);

            if (selectedHero == null || !selectedHero.canPlay || selectedHero.team != DataManager.instance.teamPlaying)
            {
                SelectHero(heroOnTile);
            }
            else if(selectedHero.canPlay)
            {
                TileClass tile = reachableTiles.Find(x => x.position == tilePos);
                if(tile != null)
                {
                    switch(tile.type)
                    {
                        case TileType.Walkable:
                            if (posChanged && selectedTile == tilePos)
                            {
                                selectedHero.position = tilePos;
                                selectedHero.canPlay = false;
                                DeselectHero();
                            }
                            else if (tile.enabled)
                            {
                                selectedHero.targetPosition = tilePos;
                                posChanged = true;
                                selectedTile = tilePos;
                                tileSelection.transform.position = MapManager.GetTileCenter(tilePos);
                                FillPath(tile);
                            }
                            else if (heroOnTile == selectedHero)
                            {
                                DeselectHero();
                            }
                            break;

                        case TileType.Attackable:
                            TileClass destTile = Utils.GetFirstWalkableTile(tile);
                            if (posChanged && selectedTile == tilePos)
                            {
                                selectedHero.position = destTile.position;
                                selectedHero.canPlay = false;
                                DeselectHero();
                            }
                            else if (tile.enabled)
                            {
                                selectedHero.targetPosition = destTile.position;
                                posChanged = true;
                                selectedTile = tilePos;
                                tileSelection.transform.position = MapManager.GetTileCenter(tilePos);
                                FillPath(destTile);
                            }
                            break;
                        default: break;
                    }
                }
                else
                {
                    DeselectHero();
                }
            }
        }
    }


    void StartTurn(int _team)
    {
        Debug.Log("Start Turn");
        playingHeroes.Clear();

        playingHeroes = MapManager.GetAllHeroes(_team);

        ClearSelectables();
        ShowSelectables();
    }

    void SelectHero(Hero _hero)
    {
        selectedHero = _hero;

        ClearHighlights();
        if (selectedHero != null && selectedHero.canPlay)
        {
            Debug.Log("Selected: " + selectedHero.data.name);

            reachableTiles = selectedHero.GetReachableTiles();
            
            if (selectedHero.team == DataManager.instance.teamPlaying)
            {
                tileSelection.enabled = true;
                tileSelection.transform.position = MapManager.GetTileCenter(selectedHero.gridPosition);
                ClearSelectables();
            }

            ShowHighlights();
        }
    }

    void DeselectHero()
    {
        ClearHighlights();
        if (selectedHero.team == DataManager.instance.teamPlaying)
            ShowSelectables();

        selectedHero.targetPosition = selectedHero.gridPosition;
        selectedHero = null;

        tileSelection.enabled = false;
        posChanged = false;
        MapManager.ResetPath();

        bool canPlay = false;
        foreach(Hero hero in playingHeroes)
        {
            canPlay |= hero.canPlay;
        }

        if(!canPlay)
        {
            DataManager.instance.EndTurn();
        }
    }

    void ClearHighlights()
    {
        foreach (SpriteRenderer sprite in highlights)
        {
            Destroy(sprite.gameObject);
        }
        highlights.Clear();
    }

    void ShowHighlights()
    {
        foreach (TileClass tile in reachableTiles)
        {
            SpriteRenderer sprite = Instantiate(prefabHighlight, transform);
            sprite.transform.position = MapManager.GetTileCenter(tile.position);

            Color color = Color.white;
            switch (tile.type)
            {
                case TileType.Walkable:
                    color = Color.blue;
                    color.a = tile.enabled ? 1.0f : 0.6f;
                    break;
                case TileType.Attackable:
                    color = Color.red;
                    color.a = tile.enabled ? 1.0f : 0.6f;
                    break;
                default:
                    break;
            }

            sprite.color = color;
            highlights.Add(sprite);
        }
    }

    void ClearSelectables()
    {
        foreach (SpriteRenderer sprite in selectables)
        {
            Destroy(sprite.gameObject);
        }
        selectables.Clear();
    }

    void ShowSelectables()
    {
        foreach (Hero hero in playingHeroes)
        {
            if (hero.canPlay)
            {
                SpriteRenderer sprite = Instantiate(prefabSelectable, transform);
                sprite.transform.position = MapManager.GetTileCenter(hero.gridPosition);
                selectables.Add(sprite);
            }
        }
    }


    void FillPath(TileClass _dest)
    {
        MapManager.ResetPath();
        TileClass current = _dest;
        while (current != null)
        {
            MapManager.SetPathTo(current.position);
            current = current.previous;
        }
    }

}
