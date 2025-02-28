using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMenu : MonoBehaviour
{
    [SerializeField]
    Button attackButton;
    [SerializeField]
    Button waitButton;
    [SerializeField]
    Button backButton;

    [SerializeField]
    MapManager mapManager;

    Unit currentUnit;
    Vector3Int currentUnitGridPos;

    bool waitForAttack = false;
    public bool hasEnemy;
    public static bool isMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = mapManager.groundMap.WorldToCell(mousePosition);
                
                //this is for making sure the place can be attacked
                if (mapManager.highlighterMap.GetTile(gridPosition) == mapManager.attackHighlight)
                {
                    //a bit too long please fix
                    Vector2 enemyPos = new Vector2(mapManager.groundMap.CellToWorld(gridPosition).x + 0.5f, mapManager.groundMap.CellToWorld(gridPosition).y + 0.5f);
                    currentUnit.Attack(enemyPos);
                    waitForAttack = false;
                    currentUnit.hasMoved = true;
                    CloseAttackMenu();
                }
            }
        }
    }
    
    /// <summary>
    /// Called whenever the Attack menu should open
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="unitPos"></param>
    public void OpenAttackMenu(Unit unit, Vector2 unitPos)
    {
        isMenu = true;
        currentUnit = unit;
        currentUnitGridPos = mapManager.groundMap.WorldToCell(unitPos);
        //checks for if there is an enemy in range, if so, bring up attack as well
        mapManager.StartPlaceHighlight(currentUnitGridPos, unit.attackRange, unit, true);
        mapManager.highlighterMap.ClearAllTiles();
        if (hasEnemy)
        {
            attackButton.gameObject.SetActive(true);
            Vector2 pos = waitButton.transform.position;
            pos.y -= 0.22f;
            waitButton.transform.position = pos;
            pos.y -= 0.22f;
            backButton.transform.position = pos;
        }

    }

    /// <summary>
    /// Called Whenever the Atttack Menu should close
    /// </summary>
    public void CloseAttackMenu()
    {
        if (hasEnemy)
        {
            attackButton.gameObject.SetActive(false);
            Vector2 pos = backButton.transform.position;
            pos.y += 0.22f;
            backButton.transform.position = pos;
            pos.y += 0.22f;
            waitButton.transform.position = pos;
            hasEnemy = false;
        }
        mapManager.highlighterMap.ClearAllTiles();
        gameObject.SetActive(false);
        isMenu = false;
    }

    public void AttackButton()
    {
        mapManager.StartPlaceHighlight(currentUnitGridPos, currentUnit.attackRange, currentUnit, true);
        List<Vector3Int> attackableEnemies = mapManager.GetEnemyUnitPos();
        for(int i = 0; i < attackableEnemies.Count; i++)
        {
            mapManager.highlighterMap.SetTile(attackableEnemies[i], mapManager.attackHighlight);
        }
        waitForAttack = true;

        currentUnit.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void WaitButton()
    {
        currentUnit.hasMoved = true;
        currentUnit.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        CloseAttackMenu();
    }

    public void BackButton()
    {
        currentUnit.gameObject.transform.position = currentUnit.pastPos;
        CloseAttackMenu();
    }
}
