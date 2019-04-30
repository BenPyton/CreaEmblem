using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Controller : MonoBehaviour
{
    [SerializeField] SpriteRenderer prefabSelecable;
    [SerializeField] SpriteRenderer prefabHighlight;
    [SerializeField] SpriteRenderer tileSelection;
    public Hero selectedHero = null;

    List<SpriteRenderer> highlights = new List<SpriteRenderer>();

    List<TileClass> reachableTiles = new List<TileClass>();

    bool posChanged = false;
    Vector3Int selectedTile = Vector3Int.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3Int tilePos = MapManager.GetTileUnderMouse();
            if (selectedHero == null)
            {
                selectedHero = GetHeroUnderMouse();

                ClearHighlights();
                if (selectedHero != null)
                {
                    Debug.Log("Selected: " + selectedHero.data.name);

                    reachableTiles = selectedHero.GetReachableTiles();

                    tileSelection.enabled = true;
                    tileSelection.transform.position = MapManager.GetTileCenter(selectedHero.gridPosition);

                    ShowHighlights();
                }
            }
            else
            {
                Hero heroOnTile = MapManager.GetHeroAtTile(tilePos);
                TileClass tile = reachableTiles.Find(x => x.position == tilePos);
                if(tile != null)
                {
                    switch(tile.type)
                    {
                        case TileType.Walkable:
                            if (posChanged && selectedTile == tilePos)
                            {
                                selectedHero.position = tilePos;
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

    void DeselectHero()
    {
        selectedHero.targetPosition = selectedHero.gridPosition;
        selectedHero = null;
        tileSelection.enabled = false;
        ClearHighlights();
        posChanged = false;
        MapManager.ResetPath();
    }

    Hero GetHeroUnderMouse()
    {
        Hero hero = null;

        Collider2D collider = Physics2D.OverlapPoint(Utils.GetMouseWorldPosition());

        if (collider != null)
        {
            hero = collider.GetComponent<Hero>();
        }

        return hero;
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
