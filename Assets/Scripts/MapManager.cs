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
    public TileBase moveHighlight;
    [SerializeField]
    public TileBase attackHighlight;

    int MapSizeX = 30;
    int MapSizeY = 30;
    Dictionary<TileBase, TileData> dataFromTiles;
    public List<Vector3Int> friendlyUnitPoses = new List<Vector3Int>();
    public List<Vector3Int> enemyUnitPoses = new List<Vector3Int>();

    List<Vector3Int> attackPoses = new List<Vector3Int>();


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
            
            //print(tile + ", " + move);

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
                rand = Random.Range(0, 10);
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
                else if (rand >= 3)
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

    // isAttack, if true will go to PlaceAttackHighlight, if false, it won't
    public void StartPlaceHighlight(Vector3Int tilePos, int movementLeft, Unit unit, bool isAttack)
    {
        friendlyUnitPoses.Clear();
        enemyUnitPoses.Clear();

        //if summoned by a friendly Unit
        if(unit.GetType() == typeof(FriendlyUnit))
        {
            for (int x = 0; x < battleController.friendlyUnits.Count; x++)
            {
                if (battleController.friendlyUnits[x] != unit)
                    friendlyUnitPoses.Add(battleController.friendlyUnits[x].currentGridPos);
            }
            for (int x = 0; x < battleController.enemyUnits.Count; x++)
            {
                enemyUnitPoses.Add(battleController.enemyUnits[x].currentGridPos);
            }
        }
        else //if summoned by an enemy unit (friendlyUnitPoses are enemies)
        {
            for (int x = 0; x < battleController.enemyUnits.Count; x++)
            {
                if (battleController.enemyUnits[x] != unit)
                    friendlyUnitPoses.Add(battleController.enemyUnits[x].currentGridPos);
            }
            for (int x = 0; x < battleController.friendlyUnits.Count; x++)
            {
                enemyUnitPoses.Add(battleController.friendlyUnits[x].currentGridPos);
            }
        }

        if (!isAttack)
        {
            PlaceHighlight(tilePos, movementLeft);
            for(int i = 0; i < attackPoses.Count; i++)
            {
                PlaceAttackHighlight(attackPoses[i], unit.attackRange);
            }
            attackPoses.Clear();
        }
        else if (isAttack)
        {
            PlaceAttackHighlight(tilePos, movementLeft);
        }
    }

    //places the movement Highlight
    void PlaceHighlight(Vector3Int tilePos, int movementLeft)
    {
        
        for (int i = 0; i < enemyUnitPoses.Count; i++)
        {
            if (tilePos == enemyUnitPoses[i])
            {
                //print(movementLeft);
                return;
            }
        }
        
        bool hasFriendlyUnit = HasFriendlyUnit(tilePos);
        
        if (!hasFriendlyUnit)
            highlighterMap.SetTile(tilePos, moveHighlight);
        
        if (movementLeft == 0)
        {
            if(!hasFriendlyUnit)
                AddAttackPos(tilePos);
            return;
        }

        Vector3Int left = new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z);
        Vector3Int up = new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z);
        Vector3Int right = new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z);
        Vector3Int down = new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z);

        
        for(int i = 0; i < 4; i++)
        {
            //checks all directions
            Vector3Int direction = tilePos;
            switch (i)
            {
                case 0: direction = left; break;
                case 1: direction = up; break;
                case 2: direction = right; break;
                case 3: direction = down; break;
            }

            /*1. checks if there is a tile in said direction: Yes, Continue; No, Leave
              2. checks if movementLeft is less that the tile's movmentCost in that same direction: 
                  Yes, (Check for friendly unit on current position, if false, add an AttackPos position and leave); No, continue
              3. checks if there is a friendly unit in said direction: Yes, add an AttackPos position; No, PlaceHighlight*/
            if (groundMap.HasTile(direction))
            {
                if (movementLeft < getMovementCost(direction)) { if (!hasFriendlyUnit) AddAttackPos(tilePos); continue; }
                if (HasFriendlyUnit(direction)) { AddAttackPos(tilePos); }
                PlaceHighlight(direction, movementLeft - getMovementCost(direction));
            }
        }

        
    }

    void PlaceAttackHighlight(Vector3Int tilePos, int attackrangeLeft)
    {
        /*for (int i = 0; i < unitPoses.Count; i++)
            if (tilePos == unitPoses[i])*/

        Vector3Int left = new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z);
        Vector3Int up = new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z);
        Vector3Int right = new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z);
        Vector3Int down = new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z);

        if(!highlighterMap.HasTile(tilePos))
            highlighterMap.SetTile(tilePos, attackHighlight);

        if (attackrangeLeft == 0)
        {
            return;
        }


        for (int i = 0; i < 4; i++)
        {
            //checks all directions
            Vector3Int direction = tilePos;
            switch (i)
            {
                case 0: direction = left; break;
                case 1: direction = up; break;
                case 2: direction = right; break;
                case 3: direction = down; break;
            }

            /*1. checks if there is a tile in said direction: Yes, Continue; No, Leave
              2. checks if movementLeft is less that the tile's movmentCost in that same direction: 
                  Yes, check for friendly unit; No, PlaceHighlight
              3. checks if there is a friendly unit in said direction: Yes, add an AttackPos position; No, Leave*/
            if (groundMap.HasTile(direction))
            {
                PlaceAttackHighlight(direction, attackrangeLeft - 1);
            }
        }


        /*//left
        if (groundMap.HasTile(left))
        {
            PlaceAttackHighlight(left, attackrangeLeft - 1);
        }
        //up
        if (groundMap.HasTile(up))
        {
            PlaceAttackHighlight(up, attackrangeLeft - 1);
        }
        //right
        if (groundMap.HasTile(right))
        {
            PlaceAttackHighlight(right, attackrangeLeft - 1);
        }
        //down
        if (groundMap.HasTile(down))
        {
            PlaceAttackHighlight(down, attackrangeLeft - 1);
        }*/
    }

    /// <summary>
    /// Checks if there is a friendly unit on the space provided
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    bool HasFriendlyUnit(Vector3Int tilePos)
    {
        for (int i = 0; i < friendlyUnitPoses.Count; i++)
        {
            if (tilePos == friendlyUnitPoses[i])
            {
                return true;
            }
        }
        return false;
    }

    //
    void AddAttackPos(Vector3Int tilePos)
    {
        if(!attackPoses.Contains(tilePos))
            attackPoses.Add(tilePos);
    }

    void Message()
    {
        print("This is message, it is long intentionally");
    }
}


