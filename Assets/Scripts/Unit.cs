using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    protected static MapManager map;
    public Vector3Int currentGridPos;

    public static void setTilemap(MapManager map) { Unit.map = map; }

    public int movementRange;
    public bool isSelected;
    //shows if any unit is selected
    public static bool oneSelected;

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
