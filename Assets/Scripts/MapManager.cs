using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    BattleController battleController;


    public Tilemap groundMap;
    public Tilemap highlighterMap;
    [SerializeField]
    List<TileData> tileDatas; // gets all the tileData's
    [SerializeField]
    TileBase grassTile;
    [SerializeField]
    TileBase mountainTile;
    [SerializeField]
    TileBase iceTile;
    [SerializeField]
    TileBase desertTile;
    [SerializeField]
    TileBase moveHighlight;
    [SerializeField]
    TileBase attackHighlight;

    int MapSizeX = 10;
    int MapSizeY = 10;
    Dictionary<TileBase, TileData> dataFromTiles;
    List<Vector3Int> unitPoses = new List<Vector3Int>();


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
        CreateMap();

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

    void CreateMap()
    {
        int rand;
        TileBase tile;
        for(int x = -MapSizeX/2; x < MapSizeX/2; x++)
        {
            for(int y = -MapSizeY/2; y < MapSizeY/2; y++)
            {
                rand = Random.Range(0, 4);
                tile = null;
                if(rand == 0)
                {
                    tile = grassTile;
                }
                else if(rand == 1)
                {
                    tile = mountainTile;
                }
                else if (rand == 2)
                {
                    tile = desertTile;
                }
                else if (rand == 3)
                {
                    tile = iceTile;
                }
                groundMap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    private int getMovementCost(Vector3Int tilePos)
    {
        return dataFromTiles[groundMap.GetTile(tilePos)].movementCost;
    }

    public void StartPlaceHighlight(Vector3Int tilePos, int movementLeft)
    {
        unitPoses.Clear();
        print("here");
        for (int x = 0; x < battleController.Units.Count; x++)
        {
            if(battleController.Units[x].currentGridPos != tilePos)
                unitPoses.Add(battleController.Units[x].currentGridPos);
        }
        PlaceHighlight(tilePos, movementLeft);
    }

    //places the movement Highlight
    void PlaceHighlight(Vector3Int tilePos, int movementLeft)
    {
        for (int i = 0; i < unitPoses.Count; i++)
            if (tilePos == unitPoses[i])
                return;

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
