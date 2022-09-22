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

    public bool hasEnemy;
    public static bool isMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
        mapManager.StartPlaceHighlight(mapManager.groundMap.WorldToCell(unitPos), unit.attackRange, unit, true);
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
            Vector2 pos = waitButton.transform.position;
            pos.y += 0.22f;
            waitButton.transform.position = pos;
            pos.y += 0.22f;
            backButton.transform.position = pos;
            hasEnemy = false;
        }
        mapManager.highlighterMap.ClearAllTiles();
        gameObject.SetActive(false);
        isMenu = false;
    }

    public void AttackButton()
    {
        currentUnit.hasMoved = true;
        currentUnit.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        CloseAttackMenu();
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
