using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    protected static BattleController battleController;
    protected static MapManager map;
    public Vector3Int currentGridPos;

    public static void setBattleController(BattleController b) { battleController = b; }
    public static void setTilemap(MapManager map) { Unit.map = map; }

    public bool isSelected;
    //shows if any unit is selected
    public static bool oneSelected;
    //for calcelling a move
    public Vector2 pastPos;
    public bool hasMoved;

    //STATS SHOWN TO PLAYER
    public int movementRange;
    public int attackRange;
    public int attackDamage;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //will be deleted soon
        if (Time.deltaTime < 0.1f)
            currentGridPos = map.groundMap.WorldToCell(gameObject.transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            //cancels if the attack menu is up OR if the unit has moved.
            if (AttackMenu.isMenu || hasMoved)
                return;
            //for unselecting the unit
            if (isSelected)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = map.groundMap.WorldToCell(mousePosition);

                map.highlighterMap.ClearAllTiles();
                isSelected = false;
                Unit.oneSelected = false;
            }
        }

    }

    void OnMouseDown()
    {
        //cancels if the attack menu is up OR if the unit has moved.
        if (AttackMenu.isMenu || hasMoved)
            return;
        if (isSelected)
            return;
        if (oneSelected)
        {
            StartCoroutine(WaitforOtherUnitMovement());
            return;
        }

        //places all the highlights
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = map.groundMap.WorldToCell(mousePosition);
        map.highlighterMap.ClearAllTiles();
        map.StartPlaceHighlight(gridPosition, movementRange, this, false);
        oneSelected = true;
        StartCoroutine(WaitforNextFrame());
    }

    /// <summary>
    /// The Unit attacks another unit
    /// </summary>
    public void Attack(Vector2 enemyPos)
    {
        print(enemyPos);
        EnemyUnit enemy;
        enemy = battleController.enemyUnits[0];

        //finds the enemy at the position
        foreach (EnemyUnit enemyUnit in battleController.enemyUnits)
        {
            if((Vector2)enemyUnit.transform.position == enemyPos)
            {
                enemy = enemyUnit;
                break;
            }
            else { return; }
        }
        enemy.TakeDamage(attackDamage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitforNextFrame()
    {
        yield return new WaitForEndOfFrame();
        isSelected = true;
    }

    IEnumerator WaitforOtherUnitMovement()
    {
        yield return new WaitForEndOfFrame();
        OnMouseDown();
    }
}
