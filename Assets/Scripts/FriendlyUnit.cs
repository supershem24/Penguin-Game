using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyUnit : Unit
{
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

                //this is for selecting a place for the unit to move
                if (map.highlighterMap.GetTile(gridPosition) == map.moveHighlight)
                {
                    Vector2 high = map.highlighterMap.CellToWorld(gridPosition);
                    gameObject.transform.position = new Vector2(high.x + 0.5f, high.y + 0.5f);
                    currentGridPos = gridPosition;
                }
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
        map.StartPlaceHighlight(gridPosition, movementRange, false);
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
