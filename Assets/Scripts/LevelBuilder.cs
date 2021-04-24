using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/** Instantiates walls automatically from a dedicated tilemap tile to speed up development **/
public class LevelBuilder : MonoBehaviour
{
    public Tilemap tilemap;
    public string wallTileName;
    public GameObject wallPrefab;

    public GameObject fighterFigure;

    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] tiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tiles[x + y * bounds.size.x];
                if (tile != null && tile.name == wallTileName)
                {
                    //Debug.Log("Wall found");
                    //TODO: Make this more elegant
                    Instantiate(wallPrefab, new Vector3(x-28,y-10,0), Quaternion.identity, transform);
                }
            }
        }

        GameSession.instance.fighterFigure = fighterFigure; // bit of a hack to put it here

    }
}
