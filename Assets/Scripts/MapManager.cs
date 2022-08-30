using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    public Tilemap groundMap;
    public Tilemap highlighterMap;
    [SerializeField]
    List<TileData> tileDatas; // gets all the tileData's
    [SerializeField]
    TileBase moveHighlight;
    [SerializeField]
    TileBase attackHighlight;

    Dictionary<TileBase, TileData> dataFromTiles;


    void Awake()
    {
        //gets all the tiles (and their data) in a dictionary
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(TileData tileData in tileDatas)
        {
            foreach(Tile tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //testing code
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = groundMap.WorldToCell(mousePosition);

            //For groundMap testing
            TileBase tile = groundMap.GetTile(gridPosition);

            int move = dataFromTiles[tile].movementCost;
            
            print(tile + ", " + move);

            


        }
    }

    private int getMovementCost(Vector3Int tilePos)
    {
        return dataFromTiles[groundMap.GetTile(tilePos)].movementCost;
    }

    //places the movement Highlight
    public void PlaceHighlight(Vector3Int tilePos, int movementLeft)
    {
        Vector3Int left = new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z);
        Vector3Int up = new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z);
        Vector3Int right = new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z);
        Vector3Int down = new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z);

        highlighterMap.SetTile(tilePos, moveHighlight);

        if (movementLeft == 0)
        {
            return;
        }


        //left
        if(groundMap.HasTile(left) && movementLeft >= getMovementCost(left))
        {
            PlaceHighlight(left, movementLeft - getMovementCost(left));
        }
        //up
        if (groundMap.HasTile(up) && movementLeft >= getMovementCost(up))
        {
            PlaceHighlight(up, movementLeft - getMovementCost(up));
        }
        //right
        if (groundMap.HasTile(right) && movementLeft >= getMovementCost(right))
        {
            PlaceHighlight(right, movementLeft - getMovementCost(right));
        }
        //down
        if (groundMap.HasTile(down) && movementLeft >= getMovementCost(down))
        {
            PlaceHighlight(down, movementLeft - getMovementCost(down));
        }
    }

}
